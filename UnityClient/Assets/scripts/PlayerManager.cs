using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Valve.VR.Extras;

public class PlayerManager : MonoBehaviour {

  static PlayerManager _instance;
  public static PlayerManager INSTANCE {
    get { 
      if (_instance == null) {
        GameObject go = new GameObject("PlayerManager");
        _instance = go.AddComponent<PlayerManager>();
      }
      return _instance;
    }
  }

  public GameObject playerVr;
  private List<GameObject> networkPlayers = new List<GameObject>();
  private GameObject teleporting;

  void Awake() {

  }

  private void SpawnVrMenuManager(Vector3 position) {
    
    SteamVR_LaserPointer leftLaserPointer = playerVr.transform.Find("SteamVRObjects/LeftHand").gameObject.AddComponent<SteamVR_LaserPointer>();
    SteamVR_LaserPointer rightLaserPointer = playerVr.transform.Find("SteamVRObjects/RightHand").gameObject.AddComponent<SteamVR_LaserPointer>();
    GameObject vrMenuManager = VrMenuManager.INSTANCE.go;
    TextMeshProUGUI username = GameObject.Find("panel_login/username").GetComponent<TextMeshProUGUI>();
    Panel panelMain = GameObject.Find("panel_main").GetComponent<Panel>();
    Panel panelLogin = GameObject.Find("panel_login").GetComponent<Panel>();

    VrMenuManager.INSTANCE.Init(
      leftLaserPointer,
      rightLaserPointer,
      username,
      panelMain,
      panelLogin,
      position
    );

  }

  public GameObject SpawnPlayerVr(Vector3 position) {
    playerVr = Instantiate(Resources.Load("PlayerVR"), position, Quaternion.identity) as GameObject;
    Vector3 vrMenuPosition = new Vector3(position.x, position.y + 1.5f, 2);
    SpawnVrMenuManager(vrMenuPosition);
    Vector3 teleportPos = new Vector3(0, 0.01f, 0);
    teleporting = Instantiate(Resources.Load("Teleporting"), teleportPos, Quaternion.identity) as GameObject;
    return playerVr;
  }

  public void EnterMech(Vector3 visorPosition) {
    playerVr.transform.Find("SteamVRObjects/VrCameraWrap").gameObject.transform.position = visorPosition;
  }

  public GameObject SpawnNetworkPlayer(Vector3 position) {
    GameObject networkPlayer = Instantiate(Resources.Load("NetworkPlayer"), position, Quaternion.identity) as GameObject;
    this.networkPlayers.Add(networkPlayer);
    return networkPlayer;
  }

  public GameObject SpawnPlayerParrel(Vector3 position) {
    GameObject parrelPlayer = SpawnNetworkPlayer(position);
    parrelPlayer.GetComponent<NetworkPlayer>().isConnected = true;
    parrelPlayer.AddComponent<ParrelAutoMove>();
    parrelPlayer.AddComponent<Camera>();
    return parrelPlayer;
  }

}
