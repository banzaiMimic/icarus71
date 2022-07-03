using UnityEngine;
using DarkRift;
using DarkRift.Client;

public sealed class PlayerMove {

  private static readonly PlayerMove instance = new PlayerMove();

  static PlayerMove() {}
  private PlayerMove(){}

  public static PlayerMove INSTANCE {
    get {
      return instance;
    }
  }

  public void Execute(object sender, MessageReceivedEventArgs e) {

    using (Message message = e.GetMessage()) {

      using (DarkRiftReader reader = message.GetReader()) {

        PlayerMoveMessage playerMoveMessage = reader.ReadSerializable<PlayerMoveMessage>();
        NetworkPlayer player = NetworkManager.INSTANCE.networkPlayers[playerMoveMessage.ID];

        Vector3 vrCamPos = new Vector3(playerMoveMessage.vrCamera.x, playerMoveMessage.vrCamera.y, playerMoveMessage.vrCamera.z);
        Vector3 leftHandPos = new Vector3(playerMoveMessage.leftHand.x, playerMoveMessage.leftHand.y, playerMoveMessage.leftHand.z);
        Vector3 rightHandPos = new Vector3(playerMoveMessage.rightHand.x, playerMoveMessage.rightHand.y, playerMoveMessage.rightHand.z);

        player.vrCamera.transform.position = vrCamPos;
        player.leftHand.transform.position = leftHandPos;
        player.rightHand.transform.position = rightHandPos;
        
      }
    }
  }

}