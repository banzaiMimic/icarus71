using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechController : MonoBehaviour {

  public GameObject vrCamera;
  public GameObject leftHand;
  public GameObject rightHand;
  public string playerName { get; private set; }
  public bool isConnected = false;

  void Awake() {
    if (!vrCamera.scene.IsValid()) {
      throw new Exception("vrCamera not set in MechController");
    }
  }

  void Update() {
    if (isConnected) {
      // Vector3 cam = new Vector3(vrCamera.transform.position.x, vrCamera.transform.position.y, vrCamera.transform.position.z );
      // Vector3 lh = new Vector3(leftHand.transform.position.x, leftHand.transform.position.y, leftHand.transform.position.z );
      // Vector3 rh = new Vector3(rightHand.transform.position.x, rightHand.transform.position.y, rightHand.transform.position.z );
      
      // PlayerMove.INSTANCE.Dispatch( 
      //   cam.x, cam.y, cam.z,
      //   lh.x, lh.y, lh.z,
      //   rh.x, rh.y, rh.z
      // );
    }
  }

  public void SetPlayerName(string playerName) {
    this.playerName = playerName;
  }

  public void Destroy() {
    Destroy(this);
  }

}
