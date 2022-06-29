using System;
using UnityEngine;

namespace icarus.gg {

  public sealed class Dispatcher {

  private static readonly Dispatcher instance = new Dispatcher();
  public event Action<string, int> ConnectToServerAction;

  // Explicit static constructor to tell C# compiler
  // not to mark type as beforefieldinit
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

// public class Dispatcher : MonoBehaviour {

//   public static Dispatcher INSTANCE;
//   public event Action<string, int> ConnectToServerAction;

//   private void Awake() {
//     INSTANCE = this;
//   }

//   public void createProjectile(Vector3 origin, float direction, float forceMultiplier) {
//     if (CreateProjectileAction != null) {
//       CreateProjectileAction( host, origin, direction, forceMultiplier );
//     }
//   }

// }

}