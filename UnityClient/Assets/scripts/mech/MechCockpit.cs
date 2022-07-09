using UnityEngine;

namespace icarus.gg {

  public class MechCockpit {

    private Mech mech;
    private GameObject go;

    public MechCockpit(Mech mech, bool invertY) {
      this.mech = mech;
      this.go = mech.gameObject;
      if (invertY) {
        InvertY();
      }
    }

    public void InvertY() {
      GameObject.Find("s.0744").transform.Rotate(0, 180f, 0);
    }

    public bool CanRotate(Vector3 rotateTo) {

      Debug.Log($"rotation x = {rotateTo.x} vs {mech.xRotationMin} max {mech.xRotationMax}");
      
      return true;
      // Vector3 rotation = rotateTo;
      // if (rotation.x >= mech.xRotationMin && rotation.x >= mech.xRotationMax ) {
      //   return true;
      // }
      // return false;
    }

    public void AimAt(float x, float y, float z) {
      Vector3 point = new Vector3(x, y, z);
      go.transform.LookAt(point);
    }

    public void AimAt(Vector3 point) {
      go.transform.LookAt(point);
      //go.transform.LookAt(point, Vector3.up);
      //go.transform.position = Camera.main.transform.position;
    }
    
  }

}