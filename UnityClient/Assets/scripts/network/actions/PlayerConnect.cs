using System;
using UnityEngine;
using DarkRift;
using DarkRift.Client;

public sealed class PlayerConnect {

  private static readonly PlayerConnect instance = new PlayerConnect();

  static PlayerConnect() {}
  private PlayerConnect(){}

  public static PlayerConnect INSTANCE {
    get {
      return instance;
    }
  }

  public void Execute(object sender, MessageReceivedEventArgs e) {

    using (Message message = e.GetMessage()) {
      
      using (DarkRiftReader reader = message.GetReader()) {
        // Read the message data
        ushort ID = reader.ReadUInt16();
        // string playerName = reader.ReadString();

        //Vector3 position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        Vector3 position = new Vector3(0,0,0);

        // Player / Network Player Spawn
        GameObject player;
        if ( ID == NetworkManager.INSTANCE.DARK_CLIENT.ID ) {
          
          if (ClientInfo.INSTANCE.isParrelClone) {
            Debug.Log("Spawning PlayerParrel at " + position);
            player = PlayerManager.INSTANCE.SpawnPlayerParrel(position);
          } else {
            // core client
            player = PlayerManager.INSTANCE.playerVr;
            player.GetComponent<NetworkPlayer>().isConnected = true;
          }
          
        } else {
          // network player [non-controllable]
          //@TODO instantiate from resources
          //obj = Instantiate(networkPlayerPrefab, position, Quaternion.identity) as GameObject;
          player = PlayerManager.INSTANCE.SpawnNetworkPlayer(position);
        }

        // // Get network entity data of prefab and add to network players store
        NetworkManager.INSTANCE.networkPlayers.Add(ID, player.GetComponent<NetworkPlayer>());

        // // Update player name
        // networkPlayers[ID].SetPlayerName(playerName);
      }
    }
  }

}