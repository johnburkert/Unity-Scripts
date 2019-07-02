using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.Linq;
using Debug = UnityEngine.Debug;

internal static class CpuUsageExt
{
    internal static double Time(this Process process) => process.TotalProcessorTime.TotalMilliseconds;

    internal static double Time(this IEnumerable<Process> processes) => processes.Select(x => x.Time()).Sum();
}

public class CpuUsage : MonoBehaviour
{
    public bool currentProcessOnly;
    public float pollingFrequency = 1f;

    private double _cpuUsage;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Poll());
    }
    
    private double GetCpuTime() => currentProcessOnly ? Process.GetCurrentProcess().Time() : Process.GetProcesses().Time();

    private IEnumerator Poll()
    {
        var prevSysTime = DateTime.Now;
        var prevCpuTime = GetCpuTime();

        while (true)
        {
            yield return new WaitForSeconds(pollingFrequency);
            
            var currSysTime = DateTime.Now;
            var currCpuTime = GetCpuTime();

            var sysTime = currSysTime - prevSysTime;
            var cpuTime = currCpuTime - prevCpuTime;
            
            _cpuUsage = cpuTime * 100.0 / (Environment.ProcessorCount * sysTime.TotalMilliseconds);

            prevSysTime = currSysTime;
            prevCpuTime = currCpuTime;
        }
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 50), $"{_cpuUsage:F2}%");
    }
}
