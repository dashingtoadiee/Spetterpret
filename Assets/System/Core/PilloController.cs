using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Hulan.PilloSDK.DeviceManager;

namespace PilloPlay.Core {

  /// <summary> Controller for all virtual Pillos </summary>
  public class PilloController : MonoBehaviour {

    /// <summary> Config file of all pillo data </summary>
    public PilloConfig config => PilloConfig.instance;

    /// <summary> The PilloController </summary>
    public static PilloController instance { get; protected set; }

    /// <summary> All connected Pillos </summary>
    public List<Pillo> pillos { get; protected set; }

    /// <summary> Whether target events are prevented </summary>
    internal bool preventTargetSeeking { get; set; }

    // Dictionary of pillos per peripheral identifiers
    private Dictionary<string, Pillo> pilloDictionary;

    /// <summary> Initialize  </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void RuntimeInitializeOnLoad() {
      GameObject obj = new GameObject("PilloController");
      DontDestroyOnLoad(obj);
      instance = obj.AddComponent<PilloController>();
      instance.Initialize();
    }

    /// <summary> Called when PilloController is initialized </summary>
    private void Initialize() {
      pillos = new List<Pillo>();
      pilloDictionary = new Dictionary<string, Pillo>();

      PilloDeviceManager.onPeripheralDidConnect += OnPilloConnected;
      PilloDeviceManager.onPeripheralDidDisconnect += OnPilloDisconnected;
      PilloDeviceManager.onPeripheralDidFailToConnect += OnPilloFailConnect;
      PilloDeviceManager.onPeripheralFirmwareVersionDidChange += OnPilloFirmwareVersionUpdate;
      PilloDeviceManager.onPeripheralHardwareVersionDidChange += OnPilloHardwareVersionUpdate;
      PilloDeviceManager.onPeripheralPressureDidChange += OnPilloPressureUpdate;
      PilloDeviceManager.onPeripheralBatteryLevelDidChange += OnPilloBatteryLevelUpdate;
      PilloDeviceManager.onPeripheralChargingStateDidChange += OnPilloChargingStateUpdate;

      Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
    // Destroy
    private void OnDestroy() {
      PilloDeviceManager.onPeripheralDidConnect -= OnPilloConnected;
      PilloDeviceManager.onPeripheralDidDisconnect -= OnPilloDisconnected;
      PilloDeviceManager.onPeripheralDidFailToConnect -= OnPilloFailConnect;
      PilloDeviceManager.onPeripheralFirmwareVersionDidChange -= OnPilloFirmwareVersionUpdate;
      PilloDeviceManager.onPeripheralHardwareVersionDidChange -= OnPilloHardwareVersionUpdate;
      PilloDeviceManager.onPeripheralPressureDidChange -= OnPilloPressureUpdate;
      PilloDeviceManager.onPeripheralBatteryLevelDidChange -= OnPilloBatteryLevelUpdate;
      PilloDeviceManager.onPeripheralChargingStateDidChange -= OnPilloChargingStateUpdate;
    }


    //================[ Events ]================\\
    // Called when Pillo is connected. Adds virtual pillo
    private void OnPilloConnected(string identifier) {
      // Find first pillo that exists but doesn't have a connected peripheral.
      for (int i = 0; i < pillos.Count; i++) {
        if (!pillos[i].hasConnectedPeripheral) {
          pillos[i].ConnectPeripheral(identifier);
          pilloDictionary.Add(identifier, pillos[i]);
          return;
        }
      }

      // Find first player index not used by pillos
      int playerIndex = 0;
      for (int i = 0; i < pillos.Count; i++) {
        if (pillos[i].playerIndex == playerIndex) {
          playerIndex++;
          i = -1;
        }
      }

      AddPillo(playerIndex, identifier);
    }

    // Called when Pillo is disconnected
    private void OnPilloDisconnected(string identifier) {
      if (pilloDictionary.TryGetValue(identifier, out Pillo pillo)) {
        pilloDictionary.Remove(identifier);
        pillo.DisconnectPeripheral();

        if (!pillo.isExistingByKey) {
          pillos.Remove(pillo);
          pillo.Disconnected();
        }
      }
    }

    // Called when pillo connection failed
    private void OnPilloFailConnect(string identifier) {
      Pillo.PilloFailConnect();
    }

    // Called when a Pillo firmware version updates
    private void OnPilloFirmwareVersionUpdate(string identifier, string firmwareVersion) {
      if (pilloDictionary.TryGetValue(identifier, out Pillo pillo)) {
        pillo.UpdateFirmwareVersion(firmwareVersion);
      }
    }

    // Called when a Pillo hardware version updates
    private void OnPilloHardwareVersionUpdate(string identifier, string hardwareVersion) {
      if (pilloDictionary.TryGetValue(identifier, out Pillo pillo)) {
        pillo.UpdateHardwareVersion(hardwareVersion);
      }
    }

    // Called when pillo pressure updates
    private void OnPilloPressureUpdate(string identifier, int pressure) {
      if (pilloDictionary.TryGetValue(identifier, out Pillo pillo)) {
        pillo.UpdatePressure(pressure);
      }
    }

    // Called when pillo battery level updates
    private void OnPilloBatteryLevelUpdate(string identifier, int batteryLevel) {
      if (pilloDictionary.TryGetValue(identifier, out Pillo pillo)) {
        pillo.UpdateBatteryLevel(batteryLevel);
      }
    }

    // Called when pillo charging state updates
    private void OnPilloChargingStateUpdate(string identifier, ChargingState chargingState) {
      if (pilloDictionary.TryGetValue(identifier, out Pillo pillo)) {
        pillo.UpdateChargingState(chargingState);
      }
    }


    //================[ Pillo Management ]================\\
    // Adds a new virtual pillo to the system
    private void AddPillo(int playerIndex, string identifier) {
      List<PilloConfig.PilloKeySettings> settings = PilloConfig.instance.buttonKeys;
      List<KeyCode> inputKeys = playerIndex < settings.Count ? settings[playerIndex].pressureKeys : null;

      // Add Pillo
      Pillo pillo = new Pillo(playerIndex, identifier, inputKeys);
      pillos.Add(pillo);
      pillos = pillos.OrderBy(p => p.playerIndex).ToList();
      pillo.Connected();

      // If it was added by peripheral, add to dictionary
      if (identifier != null) {
        pilloDictionary.Add(identifier, pillo);
      }
    }


    //================[ Update ]================\\
    // Update input
    private void Update() {
      UpdatePilloKeyboardInput();

      // Update pillos
      for (int i = 0; i < pillos.Count; i++) {
        pillos[i].Update();
      }
    }

    // Updates Pillo keyboard input
    private void UpdatePilloKeyboardInput() {
      if (PilloConfig.instance.allowKeyInput) {
        // Reset key pillos
        if (Input.GetKeyDown(PilloConfig.instance.resetKeyPillosKey)) {
          for (int i = 0; i < pillos.Count; i++) {
            if (!pillos[i].hasConnectedPeripheral) {
              Pillo pillo = pillos[i];
              pillos.RemoveAt(i);
              i--;
              pillo.Disconnected();
            }
          }
        }

        // If key is pressed but no pillo exists, create pillo
        List<PilloConfig.PilloKeySettings> settings = PilloConfig.instance.buttonKeys;
        for (int i = 0; i < settings.Count; i++) {
          for (int k = 0; k < settings[i].pressureKeys.Count; k++) {
            if (Input.GetKeyDown(settings[i].pressureKeys[k])) {
              // Add Pillo if pressed but doesn't exist
              bool exists = false;
              for (int p = 0; p < pillos.Count; p++) {
                if (pillos[p].playerIndex == i) {
                  exists = true;
                  break;
                }
              }
              if (!exists) {
                AddPillo(i, null);
                break;
              }
            }
          }
        }
      }
    }

  }
}