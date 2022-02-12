using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

class Timer : IDisposable {
    
    private readonly string _text;
    private Stopwatch _stopwatch;
 
    public Timer(string text) {
        _text = text;
        _stopwatch = Stopwatch.StartNew();
    }
 
    public void Dispose() {
        _stopwatch.Stop();
        Debug.Log(string.Format("Profiled {0}: {1:0.00}ms", _text, _stopwatch.ElapsedMilliseconds));
    }
}