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

  private GameObject playerVr;
  private List<GameObject> networkPlayers = new List<GameObject>();

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
      panelLogin
    );

  }

  public void SpawnPlayerVr(Vector3 position) {
    playerVr = Instantiate(Resources.Load("PlayerVR"), position, Quaternion.identity) as GameObject;
    SpawnVrMenuManager(position);
  }

  public GameObject SpawnNetworkPlayer(Vector3 position) {
    GameObject networkPlayer = Instantiate(Resources.Load("NetworkPlayer"), position, Quaternion.identity) as GameObject;
    this.networkPlayers.Add(networkPlayer);
    return networkPlayer;
  }

  public void SpawnPlayerParrel(Vector3 position) {
    GameObject parrelPlayer = SpawnNetworkPlayer(position);
    parrelPlayer.AddComponent<ParrelAutoMove>();
    parrelPlayer.AddComponent<Camera>();
  }

}
