using System;
using System.Collections;
using System.Collections.Generic;
using DarkRift;
using DarkRift.Client;
using DarkRift.Client.Unity;
using UnityEngine;
using UnityEngine.XR;
using icarus.gg;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using ParrelSync;
#endif

/// <summary>
/// initializes Dark Rift 2 client 
/// </summary>
public class NetworkManager : MonoBehaviour {

  /*
  @Recall
  for different scenes need to
  - keep track of every player's active scene
  - only send / react to those messages on the scenes with all players that are sending...
*/

  public static NetworkManager INSTANCE;

  public readonly string HOST = "localhost";
  public readonly int PORT = 4296;

  public UnityClient DARK_CLIENT { get; private set; }

  public Dictionary<ushort, NetworkPlayer> networkPlayers {get; set;} = new Dictionary<ushort, NetworkPlayer>();

  void Awake() {
    if (INSTANCE != null) {
      Destroy (gameObject);
      return;
    }

    INSTANCE = this;

    DARK_CLIENT = GetComponent<UnityClient>();
    DARK_CLIENT.MessageReceived += MessageHandler.INSTANCE.MessageReceived;
    #if UNITY_EDITOR
      if (ClonesManager.IsClone()) {
        Debug.Log("this is a clone project via parrelSync local development");
        ClientInfo.INSTANCE.isParrelClone = true;
      }
    #endif

  }

  void OnEnable() {
    Dispatcher.INSTANCE.ConnectToServerAction += HandleConnectToServer;
  }

  private bool IsVrAvailable() {
    List<XRDisplaySubsystem> displaySubsystems = new List<XRDisplaySubsystem>();
    SubsystemManager.GetInstances<XRDisplaySubsystem>(displaySubsystems);
    foreach (var subsystem in displaySubsystems) {
      if (subsystem.running) {
        return true;
      }
    }
    return false;
  }

  void Start() {
    Vector3 playerStartPoint = new Vector3(0,0,0);
    if (ClientInfo.INSTANCE.isParrelClone) {
      VrKill();
      Dispatcher.INSTANCE.connectToServer( HOST, PORT);
    } else {
      if (IsVrAvailable()) {
        Debug.Log("VR available-- loading PlayerVR");
        PlayerManager.INSTANCE.SpawnPlayerVr(playerStartPoint);
      } else {
        Debug.Log("VR not found-- loading keyboard / mouse");
        VrKill();
      }
    }
  }

  private void VrInit() {
    UnityEngine.XR.Management.XRGeneralSettings.Instance.Manager.InitializeLoaderSync();
    UnityEngine.XR.Management.XRGeneralSettings.Instance.Manager.StartSubsystems();
  }

  private void VrKill() {
    Debug.Log("stopping XR subsystems...");
    UnityEngine.XR.Management.XRGeneralSettings.Instance.Manager.StopSubsystems();
    UnityEngine.XR.Management.XRGeneralSettings.Instance.Manager.DeinitializeLoader();
  }

  void OnDisable() {
    Dispatcher.INSTANCE.ConnectToServerAction -= HandleConnectToServer;
  }

  private void HandleConnectToServer(string host, int port) {
    try {
      DARK_CLIENT.Connect(host, port, false);
    } catch(Exception e) {
      Debug.LogError(e.Message);
    }
  }

  void SceneLoad(object sender, MessageReceivedEventArgs e) {
    using (Message message = e.GetMessage()) {
      using (DarkRiftReader reader = message.GetReader()) {

      }
    }
  }

  public void SendPlayerMoveMessage(
    float camX,
    float camY,
    float camZ,
    float lhX,
    float lhY,
    float lhZ,
    float rhX,
    float rhY,
    float rhZ
  ) {
    using (DarkRiftWriter writer = DarkRiftWriter.Create()) {

      writer.Write(new PlayerMoveMessage(
        camX, camY, camZ,
        lhX, lhY, lhZ,
        rhX, rhY, rhZ
        ));

      using (Message message = Message.Create(Tags.PlayerMove, writer)) {
        DARK_CLIENT.SendMessage(message, SendMode.Unreliable);
      }
    }
  }
}
