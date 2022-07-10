using System;
using UnityEngine;

namespace IcarusGG {

  public sealed class Dispatcher {

    private static readonly Dispatcher instance = new Dispatcher();
    public event Action<string, int> ConnectToServerAction;

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

    public void Log(string msg) {
      Debug.Log($"[Dispatcher] {msg}");
    }
  }
}