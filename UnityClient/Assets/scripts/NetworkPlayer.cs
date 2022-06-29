using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayer : MonoBehaviour {

  public GameObject vrCamera { get; private set; }
  public GameObject leftHand { get; private set; }
  public GameObject rightHand { get; private set; }

  public string playerName { get; private set; }

  void Update() {
    Vector3 cam = new Vector3(vrCamera.transform.position.x, vrCamera.transform.position.y, vrCamera.transform.position.z );
    Vector3 lh = new Vector3(leftHand.transform.position.x, leftHand.transform.position.y, leftHand.transform.position.z );
    Vector3 rh = new Vector3(rightHand.transform.position.x, rightHand.transform.position.y, rightHand.transform.position.z );
    NetworkManager.INSTANCE.SendPlayerMoveMessage( cam, lh, rh );
  }

  public void SetPlayerName(string playerName) {
    this.playerName = playerName;
  }

}