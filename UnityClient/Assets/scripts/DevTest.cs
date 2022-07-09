using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using icarus.gg;

/**
 reference obj / dev tests 
*/
public class DevTest : MonoBehaviour {

  public GameObject mechCockpit;
  public float strength = 1f;
  public float strengthY = 1f;
  public float viewPointDistance = 6.0f;
  private Vector3 viewingPoint = new Vector3(0, 0, 0);
  [SerializeField]
  private LineController lineController;
  private bool lockMechCockpitRotation = false;
  private float mcRotationMaxX = 334f;
  private float mcRotationMinX = 34f;
  private float mcRotationYLimit = 60f;

  void Awake() {
    Dispatcher.INSTANCE.UpdateMechCockpitAction += onUpdateMechCockpit;
    mechCockpit.transform.localRotation = Quaternion.identity;
  }

  void Destroy() {
    Dispatcher.INSTANCE.UpdateMechCockpitAction -= onUpdateMechCockpit;
  }

  void DrawLine(Vector3 start, Vector3 end) {
    
  }

/*
recall individual limit checks (lock x OR y not all?)
- @Todo figure out how to map origins / pivot points better and not have to invert ys  (facing direction)
*/
  private bool isOverRotating(Vector3 vrCameraRotation) {
    Debug.Log("---");
    Debug.Log($"checking rotation.x {vrCameraRotation.x} vs mcRotationXMax {mcRotationMaxX} and mcRotationMinX {mcRotationMinX}");
    Debug.Log($"checking rotation.x {vrCameraRotation.y}");
    if (
      vrCameraRotation.x >= mcRotationMinX && vrCameraRotation.x <= mcRotationMaxX 
      
    ) {
      return true;
    }
    // if (
    //   vrCameraRotation.x >= mcRotationXMax || vrCameraRotation.x <= mcRotationXMax ||
    //   //vrCameraRotation.y >= mcRotationYLimit || vrCameraRotation.y <= mcRotationYLimit
    // ) {
    //   return true;
    // }
    return false;
  }

  private void Update() {
    Vector3 camRot = Camera.main.transform.localEulerAngles;
    Vector3 camPos = Camera.main.transform.position;

    if (isOverRotating(camRot)) {
      
    } else {
      mechCockpit.transform.LookAt(viewingPoint);
      mechCockpit.transform.position = camPos;
    }
  }

  private void onUpdateMechCockpit(float rx, float ry, float rz, float px, float py, float pz) {

    Vector3 camPos = Camera.main.transform.position;
    viewingPoint = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, viewPointDistance));

    //@Recall we dont need any dispatches here we have access to Camera.main -_-
    // ---

    //dev-ref
    //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
    //cube.transform.position = point;
    Vector3[] points = new Vector3[] { mechCockpit.transform.position, viewingPoint };
    lineController.DrawLine( points );


  }

}
