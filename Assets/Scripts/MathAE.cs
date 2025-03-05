using UnityEngine;

public static class MathAE
{
    public static float RemapFloat(float value, float initialMin, float initialMax, float targetMin, float targetMax)
    {
        float t = MathAE.InverseLerpUnclamped(initialMin, initialMax, value);
        return Mathf.LerpUnclamped(targetMin, targetMax, t);
    }

    public static float RemapFloatClamped(float value, float initialMin, float initialMax, float targetMin, float targetMax)
    {
        float t = MathAE.InverseLerpUnclamped(initialMin, initialMax, value);
        return Mathf.Clamp(Mathf.LerpUnclamped(targetMin, targetMax, t), targetMin, targetMax);
    }

    public static float InverseLerpUnclamped(float rangeMin, float rangeMax, float value)
    {
        return (value - rangeMin) / (rangeMax - rangeMin);
    }
}
