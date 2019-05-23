using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using System.Threading;
using System.Collections.Concurrent;

public class LogSender : MonoBehaviour {
    private static LogSender _instance = null;
    private string _serverUrl;

    private WebSocket _socket;
    private ConcurrentQueue<string> _logs = new ConcurrentQueue<string>();

    private volatile Thread _thread;
    private volatile bool _end = false;


    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void OnInitialize()
    {
        if(_instance != null) {
            Debug.LogError("already initialized");
            return;
        }

        var config = Resources.Load("LogSender") as LogSenderConfiguration;
        if(config == null) {
            Debug.LogError("not found configuration");
            return;
        }

        if (!config.enable) {
            Debug.Log("disable LogSender");
            return;
        }

        Debug.Log("enable LogSender");

        var go = new GameObject("_LogSender");
        var logSender = go.AddComponent<LogSender>();
        logSender._Setup(config);

        _instance = logSender;
                
        DontDestroyOnLoad(go);
    }

    public static void Log(string message)
    {
        if (_instance == null) return;
        _instance._OnLog(message, null, LogType.Log);
    }

    public static void LogFormat(string format, params object[] args)
    {
        if (_instance == null) return;
        _instance._OnLog(string.Format(format, args), null, LogType.Log);
    }

    void Awake()
    {
        if(_instance != null) {
            Debug.LogError("dont call AddComponent<LogSender>(), use LogSenderConfiguration");
            Destroy(this);
            return;
        }

        Application.logMessageReceivedThreaded += _OnLog;
        _thread = new Thread(_WorkerThread);
        _thread.Start();

    }

    private void _OnLog(string condition, string stackTrace, LogType type)
    {
        if (type == LogType.Exception || type == LogType.Error) {
            _logs.Enqueue(condition + "\n" + stackTrace);
        } else {
            _logs.Enqueue(condition);
        }
    }

    private void _WorkerThread()
    {
        try {
            _socket = new WebSocket(_serverUrl);
            while (!_end) {
                while (!_end && !_socket.IsConnected) {
                    Thread.Sleep(100);
                    _socket.Connect();
                }

                while (!_end && _socket.IsConnected) {
                    Thread.Sleep(100);
                    var log = (string)null;
                    while (_logs.Count > 0) {
                        if(_logs.TryDequeue(out log)) {
                            _socket.Send(log);
                        } else {
                            break;
                        }
                    }
                }
            }

            if (_socket != null) {
                _socket.Close();
                _socket = null;
            }
        } catch(System.Exception) {
            if (!_end) {
                _thread = new Thread(_WorkerThread);
                _thread.Start();
            }
        }
    }
    
    void OnDestroy()
    {
        _end = true;

        Application.logMessageReceivedThreaded -= _OnLog;

        if (this == _instance) {
            _instance = null;
        }
    }

    private void _Setup(LogSenderConfiguration config)
    {
        _serverUrl = config.GetWebsocketUrl();
    }
}