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

  void Awake() {
    Dispatcher.INSTANCE.RotateMechCockpitAction += onRotateMechCockpit;
    mechCockpit.transform.localRotation = Quaternion.identity;
  }

  void Destroy() {
    Dispatcher.INSTANCE.RotateMechCockpitAction -= onRotateMechCockpit;
  }

  void DrawLine(Vector3 start, Vector3 end) {
    
  }

  private void onRotateMechCockpit(float x, float y, float z) {

    // Transform cockpit = mechCockpit.transform;
    // Debug.Log($"[DevTest] - onRotate called ({x},{y},{z})");
    // Debug.Log($"  - cockpit current rotation ({cockpit.rotation.x},{cockpit.rotation.y},{cockpit.rotation.z})");
    // Vector3 rotationUpdate = new Vector3(-x * strength,y * strengthY + 180,z * strength);
    // cockpit.localRotation = Quaternion.Euler(rotationUpdate);

    // get Vector3 point in front of camera view
    //Camera vrCam = PlayerManager.INSTANCE.getVrCam();
    
    // ---
    viewingPoint = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, viewPointDistance));
    
    mechCockpit.transform.LookAt(viewingPoint);
    // ---

    //dev-ref
    //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
    //cube.transform.position = point;
    Vector3[] points = new Vector3[] { mechCockpit.transform.position, viewingPoint };
    lineController.DrawLine( points );


  }

}
