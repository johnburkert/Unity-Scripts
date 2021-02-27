using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public class BenchmarkScript : MonoBehaviour
{
    public TMP_Text tmpText;

    private bool _active;
    private long _frames;
    private double _average;
    private readonly RingQueue<long> _history = new RingQueue<long>(100);
    
    private void Start()
    {
        Debug.Log($"[Benchmark] Resolution: {Screen.width}x{Screen.height}");
        Debug.Log($"[Benchmark] QualitySetting: {QualitySettings.names[QualitySettings.GetQualityLevel()]}");

        if (tmpText == null)
            tmpText = GetComponent<TMP_Text>();

        StartCoroutine(GuiRoutine());
    }

    private void Update()
    {
        if (_active)
            _frames++;
    }

    private IEnumerator GuiRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            if (tmpText)
                tmpText.SetText($"FRAMES: {_frames} ({_average:F1})");
        }
    }
    
    public void StartBenchmark()
    {
        // disable vsync
        QualitySettings.vSyncCount = 0;

        // if stop wasn't called last time
        StopBenchmark();

        // reset frame counter
        _frames = 0;
        
        // set as active
        _active = true;
    }
    
    public void StopBenchmark()
    {
        // if not active with frames, exit
        if (!(_active && _frames > 0))
            return;
        
        // add to history
        _history.Enqueue(_frames);

        // log result
        Debug.Log($"[Benchmark] Score: {_frames}");

        // calculate average
        _average = _history.Average();
        
        // set as inactive
        _active = false;
    }
}