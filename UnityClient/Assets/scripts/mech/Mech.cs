using System;
using UnityEngine;

namespace gg.icarus {

  // currently attached to Mech 3d obj
  public class Mech : MonoBehaviour {

    public String uid = Guid.NewGuid().ToString();
    public float xRotationMax = 30f;
    public float xRotationMin = -40f;

    [SerializeField]
    private GameObject mechCockpit;                           // needs to be set manually for now
    private MechCockpit cockpit;
    private bool isPiloted { get; set; }
    private bool isLooking = false;
    [SerializeField] 
    private bool invertCockpitY = true;
    [SerializeField] 
    private float viewDistance = 6f;

    // - dev.util
    [SerializeField] private bool manualOverride = false;
    [SerializeField] private LineController lineController;
    [SerializeField] private Transform viewTarget;
    [SerializeField] private Transform rotateTarget;

    void Awake() {
      cockpit = new MechCockpit( this, mechCockpit, this.invertCockpitY );
      MechManager.INSTANCE.addMech(this);
    }

    void Update() {
      //@Recall just add an offset for the cam and keep moving lol
      Vector3 viewingPoint = GetViewingPoint(viewDistance);
      Vector3 rotation = Camera.main.transform.localEulerAngles;

      if (manualOverride) {
        // force move the camera so its actual
        viewingPoint = viewTarget.position;
        rotation = rotateTarget.localEulerAngles;
        //Debug.Log($"rotation is : {rotation}");
      }

      MoveCockpit(rotation, viewingPoint);
      DrawVisionLine();
    }

    /**
      moves cockpit if within rotation range
    */
    private void MoveCockpit(Vector3 rotation, Vector3 viewPoint) {
      if ( cockpit.CanRotate(rotation) ) {
        cockpit.AimAt(viewPoint);
      }
    }

    public void ShowEntrance() {

    }

    public void HideEntrance() {

    }

    public void Enter(Transform visor) {
      isPiloted = true;
    }

    // ------------------------------------------------------------
    // -- collisions / triggers
    void OnCollisionEnter(Collision collision) {
      foreach (ContactPoint contact in collision.contacts) {
        Debug.Log($"collision enter at mech {uid}");
        Debug.DrawRay(contact.point, contact.normal, Color.white);
      }
      // if (collision.relativeVelocity.magnitude > 2) {
        
      // }
    }

    void OnTriggerEnter(Collider other) {
      Debug.Log($"trigger enter at mech {uid}");
      if (other.tag == "Player") {
        // collision with PlayerColider inside of VrCameraWarp
      }
    }

    // ------------------------------------------------------------
    // -- util
    // sends out a viewing point from cam to a given distance param
    public Vector3 GetViewingPoint(float distance) {
      return Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, distance));
    }

    public void DrawLine(Vector3 start, Vector3 end) {
      Vector3[] points = new Vector3[] { start, end };
      lineController.DrawLine( points );
    }

    // draws a line to show looking direction
    private void DrawVisionLine() {
      DrawLine( this.gameObject.transform.position, GetViewingPoint(viewDistance));
    }
    
  }

}

/*
Mech
---

showEntrance()

hideEntrance()

Enter()

Exit()

BoostTo()

LookAt()

- Walk()

- Jump()

// unity class to attach
*/