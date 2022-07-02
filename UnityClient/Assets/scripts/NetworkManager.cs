using System;
using System.Collections;
using System.Collections.Generic;
using DarkRift;
using DarkRift.Client;
using DarkRift.Client.Unity;
using UnityEngine;
using UnityEngine.XR;
using icarus.gg;
#if UNITY_EDITOR
using ParrelSync;
#endif

/*
@Recall - need to send PlayerVR into resources folder... to test this via paralelsync we need the client to spawn in the vrCam, leftHand,
and rightHand but as a single object without any VRHeadset functionality.
- move PlayerVR to resources folder and spawn it in on client connection (we can move connect logic to login later)
- attach a basic keyboard handler to the dev-playerVr object... the send messages get sent on update right now so this will work.
- if parallelsync has 'client' param or 'dev' etc -> spawn the devPlayerVr
- run test in parallel to see if positions are updated in both clients
*/
public class NetworkManager : MonoBehaviour
{
  public static NetworkManager INSTANCE;
  //public readonly string HOST = "157.245.94.24";
  public readonly string HOST = "localhost";
  public readonly int PORT = 4296;

  private UnityClient drClient { get; set; }
  public bool isParrelClone = false;

  public Dictionary<ushort, NetworkPlayer> networkPlayers = new Dictionary<ushort, NetworkPlayer>();

  void Awake() {
    if (INSTANCE != null) {
      Destroy (gameObject);
      return;
    }

    INSTANCE = this;

    drClient = GetComponent<UnityClient>();
    drClient.MessageReceived += MessageReceived;
    #if UNITY_EDITOR
      if (ClonesManager.IsClone()) {
        Debug.Log("this is a clone project via parrelSync local development");
        this.isParrelClone = true;
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
    if (this.isParrelClone) {
      VrKill();
      Dispatcher.INSTANCE.connectToServer( HOST, PORT);
    } else {
      if (IsVrAvailable()) {
        Debug.Log("VR available-- loading PlayerVR");
        PlayerManager.INSTANCE.SpawnPlayerVr(playerStartPoint);
        Dispatcher.INSTANCE.connectToServer( HOST, PORT);
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
      drClient.Connect(host, port, false);
    } catch(Exception e) {
      Debug.LogError(e.Message);
    }
  }

  void MessageReceived(object sender, MessageReceivedEventArgs e)
  {
    using (Message message = e.GetMessage() as Message)
    {
      if (message.Tag == Tags.PlayerConnect)
      {
        PlayerConnect (sender, e);
      }
      else if (message.Tag == Tags.PlayerMove)
      {
        PlayerMove (sender, e);
      }
      else if (message.Tag == Tags.PlayerDisconnect)
      {
        PlayerDisconnect (sender, e);
      }
      // else if (message.Tag == Tags.PlayerInformationTag) {
      //  PlayerInformation(sender, e);
      // } else if (message.Tag == Tags.StartGameTag) {
      // StartGame(sender, e);
    }

    // Update the UI with connected players [this was for playerName text]
    //UIManager.singleton.PopulateConnectedPlayers (networkPlayers);
  }

  void PlayerConnect(object sender, MessageReceivedEventArgs e)
  {
    using (Message message = e.GetMessage())
    {
      using (DarkRiftReader reader = message.GetReader())
      {
        // Read the message data
        ushort ID = reader.ReadUInt16();
        // string playerName = reader.ReadString();

        //Vector3 position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        Vector3 position = new Vector3(0,0,0);

        // Player / Network Player Spawn
        GameObject player;
        if (ID == drClient.ID) {
          // playerConnected's client
          if (this.isParrelClone) {
            Debug.Log("Spawning PlayerParrel at " + position);
            player = PlayerManager.INSTANCE.SpawnPlayerParrel(position);
          } else {
            player = PlayerManager.INSTANCE.playerVr;
          }
          
        } else {
          // network player [non-controllable]
          //@TODO instantiate from resources
          //obj = Instantiate(networkPlayerPrefab, position, Quaternion.identity) as GameObject;
          player = PlayerManager.INSTANCE.SpawnNetworkPlayer(position);
        }

        // // Get network entity data of prefab and add to network players store
        networkPlayers.Add(ID, player.GetComponent<NetworkPlayer>());

        // // Update player name
        // networkPlayers[ID].SetPlayerName(playerName);
      }
    }
  }

  void PlayerMove(object sender, MessageReceivedEventArgs e)
  {
    using (Message message = e.GetMessage())
    {
      using (DarkRiftReader reader = message.GetReader())
      {
        PlayerMoveMessage playerMoveMessage = reader.ReadSerializable<PlayerMoveMessage>();
        NetworkPlayer player = networkPlayers[playerMoveMessage.ID];

        Vector3 vrCamPos = new Vector3(playerMoveMessage.vrCamera.x, playerMoveMessage.vrCamera.y, playerMoveMessage.vrCamera.z);
        Vector3 leftHandPos = new Vector3(playerMoveMessage.leftHand.x, playerMoveMessage.leftHand.y, playerMoveMessage.leftHand.z);
        Vector3 rightHandPos = new Vector3(playerMoveMessage.rightHand.x, playerMoveMessage.rightHand.y, playerMoveMessage.rightHand.z);

        player.vrCamera.transform.position = vrCamPos;
        player.leftHand.transform.position = leftHandPos;
        player.rightHand.transform.position = rightHandPos;
        
      }
    }
  }

  void PlayerDisconnect(object sender, MessageReceivedEventArgs e)
  {
    using (Message message = e.GetMessage())
    {
      using (DarkRiftReader reader = message.GetReader())
      {
        ushort ID = reader.ReadUInt16();
        Destroy(networkPlayers[ID].gameObject);
        networkPlayers.Remove (ID);
      }
    }
  }

  private class PlayerMoveMessage : IDarkRiftSerializable {
    public ushort ID { get; set; }

    public Vector3 vrCamera { get; set; }
    public Vector3 leftHand { get; set; }
    public Vector3 rightHand { get; set; }

    public PlayerMoveMessage() {}

    public PlayerMoveMessage(
      float camX, float camY, float camZ,
      float lhX, float lhY, float lhZ,
      float rhX, float rhY, float rhZ
    ) {
      this.vrCamera = new Vector3(camX, camY, camZ);
      this.leftHand = new Vector3(lhX, lhY, lhZ);
      this.rightHand = new Vector3(rhX, rhY, rhZ);
    }

    public void Deserialize(DeserializeEvent e) {
      ID = e.Reader.ReadUInt16();
      vrCamera = new Vector3(e.Reader.ReadSingle(), e.Reader.ReadSingle(), e.Reader.ReadSingle());
      leftHand = new Vector3(e.Reader.ReadSingle(), e.Reader.ReadSingle(), e.Reader.ReadSingle());
      rightHand = new Vector3(e.Reader.ReadSingle(), e.Reader.ReadSingle(), e.Reader.ReadSingle());
    }

    public void Serialize(SerializeEvent e) {
      e.Writer.Write(vrCamera.x);
      e.Writer.Write(vrCamera.y);
      e.Writer.Write(vrCamera.z);
      e.Writer.Write(leftHand.x);
      e.Writer.Write(leftHand.y);
      e.Writer.Write(leftHand.z);
      e.Writer.Write(rightHand.x);
      e.Writer.Write(rightHand.y);
      e.Writer.Write(rightHand.z);
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
        drClient.SendMessage(message, SendMode.Unreliable);
      }
    }
  }
}
