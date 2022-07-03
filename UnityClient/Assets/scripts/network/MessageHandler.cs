using DarkRift;
using DarkRift.Client;

public sealed class MessageHandler {

  private static readonly MessageHandler instance = new MessageHandler();

  static MessageHandler() {}
  private MessageHandler(){}

  public static MessageHandler INSTANCE { get { return instance; } }

  public void MessageReceived(object sender, MessageReceivedEventArgs e) {

    using (Message message = e.GetMessage() as Message) {

      //@Todo should be able to do this using a map(?)

      if (message.Tag == Tags.PlayerConnect) {
        PlayerConnect.INSTANCE.Execute(sender, e);
      } else if (message.Tag == Tags.PlayerMove) {
        PlayerMove.INSTANCE.Execute(sender, e);
      } else if (message.Tag == Tags.PlayerDisconnect) {
        PlayerDisconnect.INSTANCE.Execute(sender, e);
      }

    }

    // Update the UI with connected players [this was for playerName text]
    //UIManager.singleton.PopulateConnectedPlayers (networkPlayers);
  }

}