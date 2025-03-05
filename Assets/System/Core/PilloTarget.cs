using System;
using System.Collections;
using System.Collections.Generic;
using DuskModules.DynamicVariables;
using UnityEngine;

namespace PilloPlay.Core {

  /// <summary> Any behaviour that can become the target of a Pillo, responding all connected Pillo's pressure as one. </summary>
  public class PilloTarget : MonoBehaviour {

    /// <summary> Event called when Pillo locked onto this target is pressed </summary>
    public event Action<PilloTarget> onPressed;
    /// <summary> Event called when Pillo locked onto this target is released </summary>
    public event Action<PilloTarget> onReleased;
    /// <summary> Event called when Pillo locked onto this target is triggered </summary>
    public event Action<PilloTarget> onTrigger;
    /// <summary> Event called when this target is locked and can not be assigned </summary>
    public event Action<PilloTarget> onLocked;
    /// <summary> Event called when this target is unlocked and can be assigned again </summary>
    public event Action<PilloTarget> onUnlocked;
    /// <summary> Called for each pillo that is attached </summary>
    public event Action<Pillo> onPilloAttached;
    /// <summary> Called for each pillo that is detached </summary>
    public event Action<Pillo> onPilloDetached;
    /// <summary> Update fired when pressure changes </summary>
    public event Action<PilloTarget> onPressureChange;

    [Tooltip("Maximum amount of attached pillos.")]
    public OptionalInt maximumAttachedPillos;
    [Tooltip("Determine which pillos may attach to this target.")]
    public PilloTargetMode targetMode;

    /// <summary> Mode of Pillo Target </summary>
    public enum PilloTargetMode {
      attachPressedAutoDetach,
      attachPressed,
      attachAny
    }

    /// <summary> All pillos of this target </summary>
    public List<Pillo> pillos { get; protected set; }
    /// <summary> Wehther the target is currently controlled </summary>
    public bool isControlled => pillos.Count > 0;

    /// <summary> Whether the target is locked and cannot be assigned Pillos </summary>
    public bool isLocked {
      get => _locked;
      set {
        if (value) Lock();
        else Unlock();
      }
    }
    private bool _locked;

    /// <summary> Whether this target can attach new targets </summary>
    public bool canAttachNew {
      get {
        if (_locked) return false;
        if (maximumAttachedPillos.inUse && pillos != null && pillos.Count >= maximumAttachedPillos.value) return false;
        return true;
      }
    }
    /// <summary> Whether it will auto detach </summary>
    public bool willAutoDetach => targetMode == PilloTargetMode.attachPressedAutoDetach;

    /// <summary> Highest pressure applying to this target </summary>
    public float pressure {
      get {
        float highest = 0;
        for (int i = 0; i < pillos.Count; i++) {
          if (pillos[i].pressure > highest) highest = pillos[i].pressure;
        }
        return highest;
      }
    }
    /// <summary> Pressure used in visual effects </summary>
    public float displayPressure => (isLocked && !willAutoDetach) || delayShowPressure.isRunning ? 0 : pressure;

    /// <summary> Whether any pillo is pressed on this target. </summary>
    public bool isPressed {
      get {
        for (int i = 0; i < pillos.Count; i++) {
          if (pillos[i].isPressed) return true;
        }
        return false;
      }
    }

    private float lastPressure;
    private TimerValue delayShowPressure;

    /// <summary> Setup </summary>
    protected virtual void Awake() {
      if (pillos == null)
        pillos = new List<Pillo>();
      delayShowPressure = new TimerValue(PilloConfig.instance.buttonShowPressureDelay);
      Pillo.CheckRequireTarget();
    }
    /// <summary> Clean up on destroy </summary>
    protected virtual void OnDestroy() {
      for (int i = 0; i < pillos.Count; i++) {
        pillos[i].DetachedTarget();
      }
    }

    /// <summary> Locks the target </summary>
    public void Lock(bool detach = true) {
      if (_locked) return;
      _locked = true;
      if (detach) DetachAll();
      onLocked?.Invoke(this);
    }

    /// <summary> Unlocks the target </summary>
    public void Unlock() {
      if (!_locked) return;
      _locked = false;
      onUnlocked?.Invoke(this);

      // Press any pillos still attached that are pressed but couldn't be.
      for (int i = 0; i < pillos.Count; i++) {
        if (pillos[i].isPressed) Press(pillos[i]);
      }

      // If it can attach new targets, tell pillos to look.
      if (canAttachNew && PilloConfig.instance.buttonTriggerMode == DuskModules.InteractionType.released)
        Pillo.CheckRequireTarget();
    }

    /// <summary> Whether it can attach the given pillo </summary>
    public bool CanAttachPillo(Pillo pillo) {
      if (!canAttachNew) return false;
      if (targetMode != PilloTargetMode.attachAny && pillo.pressure <= pillo.config.zeroPressureThreshold) return false;
      if (pillos != null && pillos.Contains(pillo)) return false;
      return true;
    }

    /// <summary> Attaches the pillo. </summary>
    internal void AttachPilloInternal(Pillo pillo) {
      if (pillos == null) pillos = new List<Pillo>();
      pillos.Add(pillo);
      onPilloAttached?.Invoke(pillo);
      OnAttached(pillo);
      if (pressure > 0) {
        if (delayShowPressure == null)
          delayShowPressure = new TimerValue(PilloConfig.instance.buttonShowPressureDelay);
        delayShowPressure.Run();
      }
    }

    /// <summary> Detaches all pillos </summary>
    public void DetachAll() {
      List<Pillo> cleared = new List<Pillo>(pillos);
      pillos = new List<Pillo>();
      for (int i = 0; i < cleared.Count; i++) {
        cleared[i].DetachedTarget();
        onPilloDetached?.Invoke(cleared[i]);
        OnDetached(cleared[i]);
      }
    }

    /// <summary> Detaches the Pillo </summary>
    internal void DetachPilloInternal(Pillo pillo) {
      int index = pillos.IndexOf(pillo);
      if (index < 0 || index >= pillos.Count) return;
      pillos.RemoveAt(index);
      onPilloDetached?.Invoke(pillo);
      OnDetached(pillo);
    }

    // Called when any Pillo is pressed
    internal void Press(Pillo pillo) {
      if (_locked || pillo.triggerLocked) return;
      OnPressed(pillo);
      onPressed?.Invoke(this);
    }

    // Called when any Pillo is released
    internal void Release(Pillo pillo) {
      OnReleased(pillo);
      onReleased?.Invoke(this);
    }

    // Called when any Pillo is triggered
    internal void Trigger(Pillo pillo) {
      if (_locked || pillo.triggerLocked) return;
      pillo.TriggerLock();
      OnTrigger(pillo);
      onTrigger?.Invoke(this);
    }

    /// <summary> Inheritable method called when pillo attached </summary>
    protected virtual void OnAttached(Pillo pillo) { }
    /// <summary> Inheritable method called when pillo detached </summary>
    protected virtual void OnDetached(Pillo pillo) { }

    /// <summary> Inheritable method called when pressed </summary>
    protected virtual void OnPressed(Pillo pillo) { }
    /// <summary> Inheritable method called when released </summary>
    protected virtual void OnReleased(Pillo pillo) { }
    /// <summary> Inheritable method called when triggered </summary>
    protected virtual void OnTrigger(Pillo pillo) { }

    /// <summary> Update frame skips </summary>
    protected virtual void Update() {
      // Short delay before showing pressure in case it's already above 0
      delayShowPressure.Update();
      if (delayShowPressure.isRunning) {
        if (pressure == 0)
          delayShowPressure.Stop();
      }

      // Apply pressure to effects
      float usePressure = displayPressure;
      if (usePressure != lastPressure) {
        lastPressure = usePressure;
        onPressureChange?.Invoke(this);
      }
    }

  }
}