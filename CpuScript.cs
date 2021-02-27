using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TMPro;
using UnityEngine;

internal static class CpuScriptExt
{
    internal static double Time(this Process process) => process.TotalProcessorTime.TotalMilliseconds;

    internal static double Time(this IEnumerable<Process> processes) => processes.Select(x => x.Time()).Sum();
}

public class CpuScript : MonoBehaviour
{
    public bool currentProcessOnly = true;
    public float pollingFrequency = 1f;

    public TMP_Text tmpText;

    private readonly RingQueue<double> _history = new RingQueue<double>(100);
    
    private void Start()
    {
        if (tmpText == null)
            tmpText = GetComponent<TMP_Text>();
        
        StartCoroutine(Poll());
    }
    
    private double GetCpuTime() => currentProcessOnly ? Process.GetCurrentProcess().Time() : Process.GetProcesses().Time();

    private IEnumerator Poll()
    {
        var procCount = Environment.ProcessorCount;
        
        var prevSysTime = DateTime.Now;
        var prevCpuTime = GetCpuTime();

        while (true)
        {
            yield return new WaitForSeconds(pollingFrequency);
            
            var currSysTime = DateTime.Now;
            var currCpuTime = GetCpuTime();

            var sysTime = currSysTime - prevSysTime;
            var cpuTime = currCpuTime - prevCpuTime;

            var cpu = cpuTime * 100.0 / (procCount * sysTime.TotalMilliseconds);
            
            _history.Enqueue(cpu);
            
            if (tmpText)
                tmpText.SetText($"CPU: {cpu:F1}% ({_history.Average():F1}%)");
            
            prevSysTime = currSysTime;
            prevCpuTime = currCpuTime;
        }
    }
}
