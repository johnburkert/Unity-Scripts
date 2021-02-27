using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public class FpsScript : MonoBehaviour
{
    public TMP_Text tmpText;

    private long _frames;
    private readonly RingQueue<float> _history = new RingQueue<float>(100);
    
    private void Start()
    {
        if (tmpText == null)
            tmpText = GetComponent<TMP_Text>();
        
        StartCoroutine(Poll());
    }

    private void Update() =>_frames++;

    private IEnumerator Poll()
    {
        const float wait = 0.5f;
        
        while (true)
        {
            yield return new WaitForSeconds(wait);

            var fps = _frames / wait;

            _history.Enqueue(fps);
            
            if (tmpText)
                tmpText.SetText($"FPS: {fps:F1} ({_history.Average():F1})");

            _frames = 0;
        }
    }
}