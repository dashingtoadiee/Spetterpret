using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuskModules.DynamicVariables;

namespace DuskModules.ScreenEffects {

  /// <summary> Settings for a screenshake. </summary>
  [CreateAssetMenu(menuName = "DuskModules/ScreenEffects/ScreenShake")]
  public class ScreenShake : ScriptableObject {

    [Tooltip("Frequency of shake.")]
    [SerializeField]
    protected FloatReference _frequency;
    /// <summary> Frequency of shake. </summary>
    public FloatReference frequency => _frequency;

    [Tooltip("Strength of shake.")]
    [SerializeField]
    protected FloatReference _strength;
    /// <summary> Frequency of shake. </summary>
    public FloatReference strength => _strength;

    [Tooltip("Duration of shake. If zero, it is infinite, and must be ended manually!")]
    [SerializeField]
    protected RandomFloat _time;
    /// <summary> Duration of shake. If zero, it is infinite, and must be ended manually! </summary>
    public RandomFloat time => _time;

    [Tooltip("Strength multiplier over time.")]
    [SerializeField]
    protected AnimationCurveReference _strengthMultiplierOverTime;
    /// <summary> Strength multiplier over time. </summary>
    public AnimationCurveReference strengthMultiplierOverTime => _strengthMultiplierOverTime;

    [Tooltip("Optional random variation of frequency.")]
    [SerializeField]
    protected OptionalRandomFloat _frequencyVariation;
    /// <summary> Optional random variation of frequency. </summary>
    public OptionalRandomFloat frequencyVariation => _frequencyVariation;

    [Tooltip("Optional random variation of strength.")]
    [SerializeField]
    protected OptionalRandomFloat _strengthVariation;
    /// <summary> Optional random variation of strength. </summary>
    public OptionalRandomFloat strengthVariation => _strengthVariation;

    [Tooltip("Sets how much this screenshake is treated as a 3D source of shaking.")]
    [SerializeField]
    [Range(0, 1)]
    protected float _spatialBlend;
    /// <summary> Sets how much this screenshake is treated as a 3D source of shaking. </summary>
    public float spatialBlend => _spatialBlend;

    [Tooltip("Minimum distance of strength roll off.")]
    [SerializeField]
    protected FloatReference _minDistance;
    /// <summary> Minimum distance of strength roll off. </summary>
    public FloatReference minDistance => _minDistance;

    [Tooltip("Maximum distance of strength roll off.")]
    [SerializeField]
    protected FloatReference _maxDistance;
    /// <summary> Maximum distance of strength roll off. </summary>
    public FloatReference maxDistance => _maxDistance;

    [Tooltip("Curve for the strength rolloff, determining the strength based on the distance of the screenshake to a shake view controller.")]
    [SerializeField]
    protected AnimationCurve _strengthRollOff;
    /// <summary> Curve for the strength rolloff, determining the strength based on the distance of the screenshake to a shake view controller. </summary>
    public AnimationCurve strengthRollOff => _strengthRollOff;

    [Tooltip("Strength vector of shake per axis. Z is for angle.")]
    [SerializeField]
    protected Vector3 _strengthVector;
    /// <summary> Strength multiplier of shake per axis. Z is for angle. </summary>
    public Vector3 strengthVector => _strengthVector;

    [Tooltip("Frequency vector of shake per axis. Z is for angle.")]
    [SerializeField]
    protected Vector3 _frequencyVector;
    /// <summary> Frequency multiplier of shake per axis. Z is for angle. </summary>
    public Vector3 frequencyVector => _frequencyVector;

    [Tooltip("Optionally use a direction, in angle (0 - 360).")]
    [SerializeField]
    protected OptionalFloat _direction;
    /// <summary> Optionally use a direction, in angle (0 - 360). </summary>
    public OptionalFloat direction => _direction;

    /// <summary> Plays the screenshake </summary>
    /// <param name="frequencyMultiplier">  Multiplier for how fast the shake should be. </param>
    /// <param name="strengthMultiplier">  Multiplier for how powerful the shake should be. </param>
    /// <param name="angle"> Direction modifier for shake, if it uses direction. </param>
    public ScreenShakePlayer Play(float frequencyMultiplier = 1, float strengthMultiplier = 1, float angle = 0) {
      return Play(Vector3.zero, frequencyMultiplier, strengthMultiplier, angle);
    }

    /// <summary> Plays the screenshake </summary>
    /// <param name="position"> Position to centre it at </param>
    /// <param name="frequencyMultiplier">  Multiplier for how fast the shake should be. </param>
    /// <param name="strengthMultiplier">  Multiplier for how powerful the shake should be. </param>
    /// <param name="angle"> Direction modifier for shake, if it uses direction. </param>
    public ScreenShakePlayer Play(Vector3 position, float frequencyMultiplier = 1, float strengthMultiplier = 1, float angle = 0) {
      return ScreenShakeManager.instance.PlayShake(this, position, frequencyMultiplier, strengthMultiplier, angle);
    }

    // Constructor for 
    public ScreenShake() {
      _frequency = new FloatReference();
      _strength = new FloatReference();
      _time = new RandomFloat();
      _minDistance = new FloatReference();
      _maxDistance = new FloatReference();

      frequency.SetConstant(40);
      strength.SetConstant(100);
      time.SetConstant(0.25f);
      minDistance.SetConstant(1);
      maxDistance.SetConstant(500);
      _strengthVector = new Vector3(1, 1, 0);
      _frequencyVector = new Vector3(1, 1, 1);

      Keyframe[] keys = new Keyframe[11];
      float t = 0;
      float v = 1;
      for (int i = 0; i < keys.Length; i++) {
        keys[i] = new Keyframe(t, v);
        v /= 2;
        if (t == 0) t++;
        else t *= 2;
      }
      _strengthRollOff = new AnimationCurve(keys);
    }

  }
}