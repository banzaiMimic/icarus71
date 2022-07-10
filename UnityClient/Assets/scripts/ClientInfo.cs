using UnityEngine;
using UnityEngine.XR;
using gg.icarus;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using ParrelSync;
#endif

/// <summary>
/// stores client information
/// </summary>
public class ClientInfo : MonoBehaviour {

  static ClientInfo _instance;
  public static ClientInfo INSTANCE {
    get { 
      if (_instance == null) {
        GameObject go = new GameObject("ClientInfo");
        _instance = go.AddComponent<ClientInfo>();
      }
      return _instance;
    }
  }

  public bool isParrelClone {get; set;} = false;
}


