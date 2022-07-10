using System;
using UnityEngine;

namespace gg.icarus {

  public sealed class Dispatcher {

    private static readonly Dispatcher instance = new Dispatcher();
    public event Action<string, int> ConnectToServerAction;
    public event Action<float, float, float, float, float, float> UpdateMechCockpitAction;

    static Dispatcher() {}
    private Dispatcher(){}

    public static Dispatcher INSTANCE {
      get {
        return instance;
      }
    }

    public void connectToServer(string host, int port) {
      if (ConnectToServerAction != null) {
        ConnectToServerAction( host, port );
      }
    }
  }
}