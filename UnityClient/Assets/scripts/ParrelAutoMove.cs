using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParrelAutoMove : MonoBehaviour {
  private GameObject vrCamera;
  private GameObject leftHand;
  private GameObject rightHand;
  private bool dirRight = true;
  private float speed = 2.0f;

  void Awake() {
    vrCamera = GameObject.Find("vrCamera");
    leftHand = GameObject.Find("leftHand");
    rightHand = GameObject.Find("rightHand");
  }

  void Update() {
    if (dirRight) {
      vrCamera.transform.Translate(Vector2.right * speed * Time.deltaTime);
    } else {
      vrCamera.transform.Translate(-Vector2.right * speed * Time.deltaTime);
    }
    if (transform.position.x >= 4.0f) {
      dirRight = false;
    }

    if (transform.position.x <= -4){
      dirRight = true;
    }
  }
}
