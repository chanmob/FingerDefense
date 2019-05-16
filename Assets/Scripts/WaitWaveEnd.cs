using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitWaveEnd : CustomYieldInstruction
{
    private bool complete;

    public WaitWaveEnd()
    {
        complete = false;
        EventManager.instance.waitForWaveToEndHandler += Handler;
    }

    public override bool keepWaiting
    {
        get {
            return !complete;
        }
    }

    private void Handler()
    {
        complete = true;
        EventManager.instance.waitForWaveToEndHandler -= Handler;
    }
}
