using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayer : MonoBehaviour {

  [SerializeField]
  private GameObject vrCamera;
  [SerializeField]
  private GameObject leftHand;
  [SerializeField]
  private GameObject rightHand;

  private string playerName { get; set; }

  void Update() {
    Vector3 cam = new Vector3(vrCamera.transform.position.x, vrCamera.transform.position.y, vrCamera.transform.position.z );
    Vector3 lh = new Vector3(leftHand.transform.position.x, leftHand.transform.position.y, leftHand.transform.position.z );
    Vector3 rh = new Vector3(rightHand.transform.position.x, rightHand.transform.position.y, rightHand.transform.position.z );
    NetworkManager.INSTANCE.SendPlayerMoveMessage( cam, lh, rh );
  }

}
