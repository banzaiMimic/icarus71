using UnityEngine;
using DarkRift;
using DarkRift.Client;

public sealed class PlayerDisconnect {

  private static readonly PlayerDisconnect instance = new PlayerDisconnect();

  static PlayerDisconnect() {}
  private PlayerDisconnect(){}

  public static PlayerDisconnect INSTANCE {
    get {
      return instance;
    }
  }

  public void Execute(object sender, MessageReceivedEventArgs e) {

    using (Message message = e.GetMessage()) {

      using (DarkRiftReader reader = message.GetReader()) {

        ushort ID = reader.ReadUInt16();
        NetworkManager.INSTANCE.networkPlayers[ID].Destroy();
        NetworkManager.INSTANCE.networkPlayers.Remove (ID);

      }
    }
  }

}