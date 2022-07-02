using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkRift;
using DarkRift.Server;

namespace MultiplayerPlugin
{
  class NetworkManager : Plugin
  {
    public override bool ThreadSafe => false;
    public override Version Version => new Version(0, 0, 1);
    Dictionary<IClient, Player> players = new Dictionary<IClient, Player>();

    public NetworkManager(PluginLoadData pluginLoadData) : base(pluginLoadData)
    {
      ClientManager.ClientConnected += ClientConnected;
      ClientManager.ClientDisconnected += ClientDisconnected;
    }

    void ClientConnected(object sender, ClientConnectedEventArgs e)
    {
      // Set client message callbacks
      e.Client.MessageReceived += OnPlayerInformationMessage;
      e.Client.MessageReceived += OnPlayerReadyMessage;
      e.Client.MessageReceived += OnPlayerMoveMessage;

      // generate new player data on client connect
      Player newPlayer = new Player(e.Client.ID, "default");
      players.Add(e.Client, newPlayer);

      // write player data and tell other connected clients about this player
      using (DarkRiftWriter newPlayerWriter = DarkRiftWriter.Create())
      {
        newPlayerWriter.Write(newPlayer);

        using (Message newPlayerMessage = Message.Create(Tags.PlayerConnectTag, newPlayerWriter))
        {
          foreach (IClient client in ClientManager.GetAllClients().Where(x => x != e.Client))
          {
            client.SendMessage(newPlayerMessage, SendMode.Reliable);
          }
        }
      }

      // tell the client player about all connected players
      foreach (Player player in players.Values)
      {
        Message playerMessage = Message.Create(Tags.PlayerConnectTag, player);
        e.Client.SendMessage(playerMessage, SendMode.Reliable);
      }
    }

    void ClientDisconnected(object sender, ClientDisconnectedEventArgs e)
    {
      // remove player from connected players
      players.Remove(e.Client);

      // tell all clients about player disconnection
      using (DarkRiftWriter writer = DarkRiftWriter.Create())
      {
        writer.Write(e.Client.ID);

        using (Message message = Message.Create(Tags.PlayerDisconnectTag, writer))
        {
          foreach (IClient client in ClientManager.GetAllClients())
          {
            client.SendMessage(message, SendMode.Reliable);
          }
        }
      }
    }

    void OnPlayerInformationMessage(object sender, MessageReceivedEventArgs e)
    {
      using (Message message = e.GetMessage() as Message)
      {
        if (message.Tag == Tags.PlayerInformationTag)
        {
          using (DarkRiftReader reader = message.GetReader())
          {
            string playerName = reader.ReadString();

            // update player information
            players[e.Client].playerName = playerName;

            // update all players
            using (DarkRiftWriter writer = DarkRiftWriter.Create())
            {
              writer.Write(e.Client.ID);
              writer.Write(playerName);

              message.Serialize(writer);
            }

            foreach (IClient client in ClientManager.GetAllClients())
            {
              client.SendMessage(message, e.SendMode);
            }
          }
        }
      }
    }

    void OnPlayerReadyMessage(object sender, MessageReceivedEventArgs e)
    {
      using (Message message = e.GetMessage() as Message)
      {
        if (message.Tag == Tags.PlayerSetReadyTag)
        {
          using (DarkRiftReader reader = message.GetReader())
          {
            bool isReady = reader.ReadBoolean();

            players[e.Client].isReady = isReady;
            CheckAllReady();
          }
        }
      }
    }

    void CheckAllReady()
    {
      foreach (IClient client in ClientManager.GetAllClients())
      {
        if (!players[client].isReady)
        {
          return;
        }
      }

      using (DarkRiftWriter writer = DarkRiftWriter.Create())
      {
        using (Message message = Message.Create(Tags.StartGameTag, writer))
        {
          foreach (IClient client in ClientManager.GetAllClients())
          {
            client.SendMessage(message, SendMode.Reliable);
          }
        }
      }
    }

    //@Recall update this to new (3 vector params) for vrCamera, leftHand, and rightHand
    void OnPlayerMoveMessage(object sender, MessageReceivedEventArgs e)
    {
      using (Message message = e.GetMessage() as Message)
      {
        if (message.Tag == Tags.PlayerMoveTag)
        {
          using (DarkRiftReader reader = message.GetReader())
          {
            float camPosX = reader.ReadSingle();
            float camPosY = reader.ReadSingle();
            float camPosZ = reader.ReadSingle();

            float lhPosX = reader.ReadSingle();
            float lhPosY = reader.ReadSingle();
            float lhPosZ = reader.ReadSingle();

            float rhPosX = reader.ReadSingle();
            float rhPosY = reader.ReadSingle();
            float rhPosZ = reader.ReadSingle();

            Player player = players[e.Client];

            player.camX = camPosX;
            player.camY = camPosY;
            player.camZ = camPosZ;

            player.leftHandX = lhPosX;
            player.leftHandY = lhPosY;
            player.leftHandZ = lhPosZ;

            player.rightHandX = rhPosX;
            player.rightHandY = rhPosY;
            player.rightHandZ = rhPosZ;

            // send this player's updated position back to all clients except the client that sent the message
            using (DarkRiftWriter writer = DarkRiftWriter.Create())
            {
              writer.Write(player.ID);
              writer.Write(player.camX);
              writer.Write(player.camY);
              writer.Write(player.camZ);
              
              writer.Write(player.leftHandX);
              writer.Write(player.leftHandY);
              writer.Write(player.leftHandZ);

              writer.Write(player.rightHandX);
              writer.Write(player.rightHandY);
              writer.Write(player.rightHandZ);

              message.Serialize(writer);
            }

            foreach (IClient client in ClientManager.GetAllClients().Where(x => x != e.Client))
              client.SendMessage(message, e.SendMode);
          }
        }
      }
    }

  }
}