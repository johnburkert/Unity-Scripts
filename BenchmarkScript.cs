using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class BenchmarkScript : MonoBehaviour
{
    private long _counter;
    private double _average;
    private readonly Queue<long> _history = new Queue<long>();
    
    public void StartBenchmark()
    {
        // disable vsync
        QualitySettings.vSyncCount = 0;

        // if stop wasn't called last time
        if (_counter > 0)
            StopBenchmark();
    }
    
    public void StopBenchmark()
    {
       // is there previous data?
        if (_counter > 0)
            _history.Enqueue(_counter);

        // keep track of last 10
        if (_history.Count > 10)
            _history.Dequeue();
        
        // calculate average
        if (_history.Count > 0)
            _average = _history.Average();

        // log if we have a result
        if (_counter > 0)
            LogResult();
        
        // reset counter
        _counter = 0;
    }

    private void LogResult()
    {
        Debug.Log($"Score: {_counter}, {Screen.width}x{Screen.height}, {QualitySettings.names[QualitySettings.GetQualityLevel()]}");
    }

    private void OnPostRender()
    {
        // increment counter
        _counter++;
    }

    private void OnGUI()
    {
        GUI.Box(new Rect(10, 10, 150, 25), $"COUNTER: {_counter}");
        GUI.Box(new Rect(10, 40, 150, 25), $"AVERAGE: {_average:F1}");
    }
}