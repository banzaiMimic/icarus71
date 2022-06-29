using System.Collections;
using System.Collections.Generic;
using DarkRift;
using DarkRift.Client;
using DarkRift.Client.Unity;
using UnityEngine;

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

  private UnityClient drClient;

  public Dictionary<ushort, NetworkPlayer> networkPlayers = new Dictionary<ushort, NetworkPlayer>();

  void Awake()
  {
    if (INSTANCE != null)
    {
      Destroy (gameObject);
      return;
    }

    INSTANCE = this;

    drClient = GetComponent<UnityClient>();
    drClient.MessageReceived += MessageReceived;
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
        string playerName = reader.ReadString();

        Vector3 position =
          new Vector3(reader.ReadSingle(),
            reader.ReadSingle(),
            reader.ReadSingle());

        // Player / Network Player Spawn
        GameObject obj;
        if (ID == drClient.ID) {
          // playerConnected's client
          //@TODO don't immediately spawn player singleton for core client -> after 'joining' some server, then instantiate with:
          //obj = Instantiate(localPlayerPrefab, position, Quaternion.identity) as GameObject;
          //@Recall remove below when above is implemented
          obj = GameObject.Find("PlayerVR") as GameObject;
        } else {
          // network player [non-controllable]
          //@TODO instantiate from resources
          //obj = Instantiate(networkPlayerPrefab, position, Quaternion.identity) as GameObject;
          obj =
            Instantiate(Resources.Load("NetworkPlayer"),
            position,
            Quaternion.identity) as
            GameObject;
        }

        // Get network entity data of prefab and add to network players store
        networkPlayers.Add(ID, obj.GetComponent<NetworkPlayer>());

        // Update player name
        networkPlayers[ID].SetPlayerName(playerName);
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

  private class PlayerMoveMessage : IDarkRiftSerializable
  {
    public ushort ID { get; set; }

    public Vector3 vrCamera { get; set; }
    public Vector3 leftHand { get; set; }
    public Vector3 rightHand { get; set; }

    public PlayerMoveMessage() {}

    public PlayerMoveMessage(
      Vector3 vrCamera,
      Vector3 leftHand,
      Vector3 rightHand
    ) {
      this.vrCamera = vrCamera;
      this.leftHand = leftHand;
      this.rightHand = rightHand;
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
    Vector3 vrCamera,
    Vector3 leftHand,
    Vector3 rightHand
  ) {
    using (DarkRiftWriter writer = DarkRiftWriter.Create())
    {
      writer.Write(new PlayerMoveMessage(vrCamera, leftHand, rightHand));
      using (Message message = Message.Create(Tags.PlayerMove, writer))
      {
        drClient.SendMessage(message, SendMode.Unreliable);
      }
    }
  }
}
