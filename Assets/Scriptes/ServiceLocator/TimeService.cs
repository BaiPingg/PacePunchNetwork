using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeService : MonoBehaviour, IService
{
    private List<Timer> _timers = new List<Timer>();

    public void AddDelay(DelayTimer delayTimer)
    {
        _timers.Add(delayTimer);
    }

    public void AddDelay(float time, Action OnComplete)
    {
        var delay = new DelayTimer(time);
        delay.OnComplete += () => { OnComplete?.Invoke(); };
        _timers.Add(delay);
    }

    private void Update()
    {
        for (int i = 0; i < _timers.Count; i++)
        {
            var timer = _timers[i];
            if (timer.Execute(Time.deltaTime))
            {
                _timers.Remove(timer);
            }
        }
    }
}