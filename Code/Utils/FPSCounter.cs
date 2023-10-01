using System.Collections.Generic;
using System.Linq;

namespace topdown1;

public static class FPSCounter
{
    public static long TotalFrames { get; private set; }
    public static float TotalSeconds { get; private set; }
    public static float AverageFramesPerSecond { get; private set; }
    public static float CurrentFramesPerSecond { get; private set; }

    public const int MaximumSamples = 100;

    private static Queue<float> _sampleBuffer = new();

    public static void Update(float deltaTime)
    {
        CurrentFramesPerSecond = 1.0f / deltaTime;

        _sampleBuffer.Enqueue(CurrentFramesPerSecond);

        if (_sampleBuffer.Count > MaximumSamples)
        {
            _sampleBuffer.Dequeue();
            AverageFramesPerSecond = _sampleBuffer.Average(i => i);
        }
        else
        {
            AverageFramesPerSecond = CurrentFramesPerSecond;
        }

        TotalFrames++;
        TotalSeconds += deltaTime;
    }
}