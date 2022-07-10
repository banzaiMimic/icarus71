using UnityEngine;

namespace IcarusGG {

  public class EnterMech : MonoBehaviour, IInterractable {

    [SerializeField]
    private Mech mech;
    private string mechId;

    void Awake() {
      this.mechId = mech.uid;
      this.gameObject.SetActive(false);
    }

    void IInterractable.onTriggerEnter(Collider other) {
      Debug.Log($"EnterMech trigger enter by {other.tag}.tag");
    }

    void IInterractable.onTriggerExit(Collider other) {
      Debug.Log($"EnterMech trigger exit by {other.tag}.tag");
    }

    void IInterractable.onCollisionEnter(Collision other) {
      Debug.Log($"EnterMech collider enter by {other.collider.tag}.tag");
    }

    void IInterractable.onCollisionExit(Collision other) {
      Debug.Log($"EnterMech collider exit by {other.collider.tag}.tag");
    }

  }

}