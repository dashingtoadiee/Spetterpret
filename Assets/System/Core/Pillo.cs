using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuskModules;
using Hulan.PilloSDK.DeviceManager;
using DuskModules.DynamicVariables;

namespace PilloPlay.Core {

  /// <summary> Virtual pillo device representing a physical pillo, with local calibration values. </summary>
  [Serializable]
  public class Pillo {

    /// <summary> Event called when Pillo is connected. </summary>
    public static event Action<Pillo> onPilloConnected;
    /// <summary> Event called when Pillo is disconnected. </summary>
    public static event Action<Pillo> onPilloDisconnected;
    /// <summary> Event called when Pillo connection attempt failed </summary>
    public static event Action onPilloConnectFail;
    /// <summary> Event called when Pillo is pressed like a Button. </summary>
    public static event Action<Pillo> onPilloButtonPressed;
    /// <summary> Event called when Pillo is released like a Button. </summary>
    public static event Action<Pillo> onPilloButtonReleased;
    /// <summary> Event called when Pillo is triggered like a Button. </summary>
    public static event Action<Pillo> onPilloButtonTrigger;
    /// <summary> Event called when Pillo is attached to a target </summary>
    public static event Action<Pillo> onPilloAttachedTarget;
    /// <summary> Event called when Pillo is detached from its target </summary>
    public static event Action<Pillo> onPilloDetachedTarget;
    /// <summary> Event called when Pillo charging state updates </summary>
    public static event Action<Pillo> onPilloChargingStateUpdate;
    /// <summary> Event called when Pillo sensitivity is changed </summary>
    public static event Action<Pillo> onPilloSensitivityChange;
    /// <summary> Event called when Pillo lights are changed </summary>
    public static event Action<Pillo> onPilloLightsChange;
    /// <summary> Event called when Pillo is calibrated </summary>
    public static event Action<Pillo> onPilloCalibrated;
    /// <summary> Event called when Pillo calls out to require a target </summary>
    public static event Action<Pillo> onPilloRequiresTarget;

    /// <summary> Quick reference to all active virtual pillos </summary>
    public static List<Pillo> pillos => PilloController.instance.pillos;

    /// <summary> Checks all Pillos whether they require a target or not </summary>
    public static void CheckRequireTarget() {
      for (int i = 0; i < pillos.Count; i++) {
        pillos[i].CheckTarget();
      }
    }

    /// <summary> Config file of all pillo data </summary>
    public PilloConfig config => PilloConfig.instance;

    /// <summary> Player index of Pillo </summary>
    public int playerIndex { get; protected set; }
    /// <summary> Whether this Pillo has a connected peripheral </summary>
    public bool hasConnectedPeripheral { get; protected set; }
    /// <summary> Peripheral identifier </summary>
    public string peripheralIdentifier { get; protected set; }
    /// <summary> Whether this Pillo exists because of key input </summary>
    public bool isExistingByKey { get; private set; }

    /// <summary> Minimum pressure </summary>
    public int minPressure { get; private set; }
    /// <summary> Maximum pressure </summary>
    public int maxPressure { get; private set; }

    /// <summary> Pressure of this pillo </summary>
    public float pressure {
      get {
        float periP = hasConnectedPeripheral ? Mathf.Clamp(((float)peripheralPressureLevel - minPressure) / (maxPressure - minPressure), 0, 1) : 0;
        float keyP = isExistingByKey ? keyPressure : 0;
        return Mathf.Max(periP, keyP);
      }
    }
    /// <summary> Pressure used in Pillo 'button' gameplay. Exceeds 0 and 1! </summary>
    public float buttonPressure => Mathf.Clamp(pressure - config.buttonInputMin, 0, config.buttonInputMax - config.buttonInputMin);
    /// <summary> Maximum battery level </summary>
    public float batteryLevel => hasConnectedPeripheral ? Mathf.Clamp((float)peripheralBatteryLevel / (float)PilloConfig.instance.maximumBattery, 0, 1) : 1;

    /// <summary> Battery charging state of Pillo </summary>
    public ChargingState chargingState { get; protected set; }
    /// <summary> Whether the button is currently pressed </summary>
    public bool isPressed { get; private set; }

    /// <summary> Pillo sensitivity value </summary>
    public PilloSensitivity sensitivity { get; protected set; }
    /// <summary> Whether the lights are on </summary>
    public bool areLEDsOn { get; protected set; }

    /// <summary> Pressure level of the peripheral </summary>
    public int peripheralPressureLevel { get; protected set; }
    /// <summary> Battery level of the peripheral </summary>
    public int peripheralBatteryLevel { get; protected set; }
    /// <summary> Firmware version of the peripheral </summary>
    public string peripheralFirmwareVersion { get; protected set; }
    /// <summary> Hardware version of the peripheral </summary>
    public string peripheralHardwareVersion { get; protected set; }

    /// <summary> Current Pillo target of pillo, if any. </summary>
    public PilloTarget target { get; protected set; }

    /// <summary> Unable to trigger new events until returned below button threshold </summary>
    public bool triggerLocked { get; protected set; }

    private List<KeyCode> pressureKeys;
    private float keyPressure;
    private TimerValue detachDelay;
    private bool zeroPressure;
    private bool isDisconnected;

    /// <summary> Sets up this virtual Pillo with the hardware pillo </summary>
    public Pillo(int playerIndex, string peripheralIdentifier, List<KeyCode> pressureKeys) {
      this.playerIndex = playerIndex;
      this.peripheralIdentifier = peripheralIdentifier;
      hasConnectedPeripheral = peripheralIdentifier != null;
      this.pressureKeys = pressureKeys;

      chargingState = ChargingState.UNKNOWN;
      areLEDsOn = true;
      peripheralPressureLevel = 0;
      peripheralBatteryLevel = -1;

      sensitivity = PilloSensitivity.normal;
      IntRange range = config.ranges.Find(r => r.key == sensitivity).value;
      minPressure = range.min;
      maxPressure = range.max;

      detachDelay = new TimerValue();
      zeroPressure = true;
    }

    // Sets the device, in case the Pillo was created through debug keys before an actual Pillo was connected
    internal void ConnectPeripheral(string peripheralIdentifier) {
      this.peripheralIdentifier = peripheralIdentifier;
      hasConnectedPeripheral = peripheralIdentifier != null;
    }

    // Unsets the device, in case the Pillo was created through debug keys before an actual Pillo was connected
    internal void DisconnectPeripheral() {
      peripheralIdentifier = null;
      hasConnectedPeripheral = false;
      chargingState = ChargingState.UNKNOWN;
    }

    // Called when Pillo is connected
    internal void Connected() {
      onPilloConnected?.Invoke(this);
      onPilloRequiresTarget?.Invoke(this);
    }

    // Called when Pillo is disconnected
    internal void Disconnected() {
      isDisconnected = true;
      DetachTarget();
      onPilloDisconnected?.Invoke(this);
    }

    // Called when a pillo failed to connect
    internal static void PilloFailConnect() {
      onPilloConnectFail?.Invoke();
    }

    // Called when a pillo firmware version is set
    internal void UpdateFirmwareVersion(string firmware) {
      peripheralFirmwareVersion = firmware;
    }

    // Called when a pillo hardware version is set
    internal void UpdateHardwareVersion(string hardware) {
      peripheralHardwareVersion = hardware;
    }

    // Called when a pillo updates pressure from peripheral
    internal void UpdatePressure(int pressure) {
      peripheralPressureLevel = pressure;
    }

    // Called when a pillo updates battery level from peripheral
    internal void UpdateBatteryLevel(int batteryLevel) {
      peripheralBatteryLevel = batteryLevel;
    }

    // Called when a pillo updates charging state from peripheral
    internal void UpdateChargingState(ChargingState chargingState) {
      this.chargingState = chargingState;
      onPilloChargingStateUpdate?.Invoke(this);
    }

    /// <summary> Sets the sensitivity setting of this Pillo </summary>
    public void SetSensitivity(PilloSensitivity sensitivity) {
      this.sensitivity = sensitivity;
      IntRange range = config.ranges.Find(r => r.key == sensitivity).value;
      minPressure = range.min;
      maxPressure = range.max;
      onPilloSensitivityChange?.Invoke(this);
    }

    /// <summary> Set LEDS of this Pillo </summary>
    public void SetLEDS(bool on) {
      areLEDsOn = on;
      PilloDeviceManager.ForcePeripheralLedOff(peripheralIdentifier, !on);
      onPilloLightsChange?.Invoke(this);
    }

    /// <summary> Starts Pillo calibration </summary>
    public void StartCalibration() {
      PilloDeviceManager.StartPeripheralCalibration(peripheralIdentifier);
      onPilloCalibrated?.Invoke(this);
    }

    /// <summary> Called during update </summary>
    internal void Update() {
      UpdatePressureKeys();
      UpdatePilloButtonResponse();
      UpdateAutoDetach();
      FindTargetOnPressure();
    }

    // Update pressure key input
    private void UpdatePressureKeys() {
      // Use debug keys to determine fake pressure
      if (pressureKeys != null && config.allowKeyInput) {
        float targetKeyPressure = 0;
        for (int i = 0; i < pressureKeys.Count; i++) {
          if (Input.GetKey(pressureKeys[i])) {
            float p = (i + 1f) / pressureKeys.Count;
            if (p > targetKeyPressure)
              targetKeyPressure = p;
          }
        }
        if (keyPressure != targetKeyPressure)
          keyPressure = Mathf.Lerp(keyPressure, targetKeyPressure, config.buttonKeyInputSpeed * DuskUtility.interfaceDeltaTime);
        if (targetKeyPressure > 0)
          isExistingByKey = true;
      }
      else
        keyPressure = 0;
    }

    /// <summary> Attaches Pillo to target </summary>
    public bool AttachToTarget(PilloTarget target) {
      if (target == null) return false;
      if (!target.CanAttachPillo(this)) return false;

      if (this.target != null) this.target.DetachPilloInternal(this);
      this.target = target;
      this.target.AttachPilloInternal(this);
      onPilloAttachedTarget?.Invoke(this);
      if (isPressed) {
        HandlePress();
      }

      return true;
    }

    /// <summary> Detaches the target from this pillo. </summary>
    public void DetachTarget() {
      if (target == null) return;
      if (target != null) target.DetachPilloInternal(this);
      DetachedTarget();
    }

    /// <summary> Execute actually detaching target from pillo </summary>
    internal void DetachedTarget() {
      target = null;
      detachDelay.Stop();
      onPilloDetachedTarget?.Invoke(this);
      if (!isDisconnected)
        CheckTarget();
    }

    // checks if it requires a target, and if so, calls out
    private void CheckTarget() {
      if (target != null) return;
      if (triggerLocked) return;
      if (PilloController.instance.preventTargetSeeking) return;
      onPilloRequiresTarget?.Invoke(this);
    }

    // Update pillo button events
    private void UpdatePilloButtonResponse() {
      // Handle Pillo like a button
      if (!isPressed) {
        if (pressure > config.buttonInputMax) {
          isPressed = true;
          HandlePress();
        }
      }
      else {
        if (pressure < config.buttonInputMax - config.buttonDeadzone) {
          isPressed = false;
          HandleRelease();
        }
      }
    }

    // Handles all press events
    private void HandlePress() {
      onPilloButtonPressed?.Invoke(this);
      target?.Press(this);
      if (config.buttonTriggerMode == InteractionType.pressed) {
        onPilloButtonTrigger?.Invoke(this);
        target?.Trigger(this);
      }
    }

    // Handles all release vents
    private void HandleRelease() {
      triggerLocked = false;
      if (config.buttonTriggerMode == InteractionType.released) {
        onPilloButtonTrigger?.Invoke(this);
        target?.Trigger(this);
      }
      onPilloButtonReleased?.Invoke(this);
      target?.Release(this);

      CheckTarget();
    }

    // Updates and checks auto detaching
    private void UpdateAutoDetach() {
      detachDelay.Update();

      if (target != null && target.willAutoDetach) {
        if (pressure <= config.zeroPressureThreshold && !detachDelay.isRunning)
          detachDelay.Run(config.autoDetachTime, DetachTarget);
        else if (pressure > config.zeroPressureThreshold && detachDelay.isRunning)
          detachDelay.Stop();
      }
    }

    // Whenever pressure increases to above 0, check if it requires a target
    private void FindTargetOnPressure() {
      if (zeroPressure && pressure > config.zeroPressureThreshold) {
        zeroPressure = false;
        CheckTarget();
      }
      else if (!zeroPressure && pressure <= config.zeroPressureThreshold) {
        zeroPressure = true;
      }
    }

    /// <summary> Prevents triggering things multiple times with one trigger. </summary>
    internal void TriggerLock() {
      triggerLocked = true;
    }

  }
}