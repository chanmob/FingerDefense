using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LogSender.asset", menuName = "LogSender", order = int.MaxValue)]
public class LogSenderConfiguration : ScriptableObject
{
    public bool enable = true;
    public string ip = "192.168.0.2";
    public int port = 11230;

    public string GetWebsocketUrl()
    {
        return string.Format("ws://{0}:{1}", ip, port);
    }
}