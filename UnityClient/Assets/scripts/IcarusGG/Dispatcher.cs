using System;
using UnityEngine;

namespace IcarusGG {

  public sealed class Dispatcher {

    private static readonly Dispatcher instance = new Dispatcher();
    public event Action<string, int> ConnectToServerAction;
    public event Action<Collider> OnHandTriggerEnterAction;
    public event Action<Collider> OnHandTriggerExitAction;
    public event Action<Collision> OnHandCollisionEnterAction;
    public event Action<Collision> OnHandCollisionExitAction;

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

    public void onHandTriggerEnter(Collider other) {
      if (OnHandTriggerEnterAction != null) {
        OnHandTriggerEnterAction(other);
      }
      if (other.tag == "Interractable") {
        Debug.Log($"interractable onHandTriggerEnter on Dispatcher for {other.tag}");
        IInterractable iInterractable = other.gameObject.GetComponent("IInterractable") as IInterractable;
        IInterractable test = other.gameObject.GetComponent("EnterMech") as IInterractable;
        if (iInterractable != null) {
          iInterractable.onTriggerEnter(other);
        } else {
          Debug.Log("ehhhhhhh interractablething was not found...");
        }

        if (test != null) {
          Debug.Log("ehhhwhwfeifjwoeifjwefoweifwef EnterMech found...");
        } else {
          Debug.Log("enterMech not found either");
        }
        //if((gameObject.GetComponent("YourDesiredScript") as YourDesiredScript) != null)

      }
    }

    public void onHandTriggerExit(Collider other) {
      if (OnHandTriggerExitAction != null) {
        OnHandTriggerExitAction(other);
      }
    }

    public void onHandCollisionEnter(Collision other) {
      if (OnHandCollisionEnterAction != null) {
        OnHandCollisionEnterAction(other);
      }
    }

    public void onHandCollisionExit(Collision other) {
      if (OnHandCollisionExitAction != null) {
        OnHandCollisionExitAction(other);
      }
    }

  }
}