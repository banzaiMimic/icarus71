using UnityEngine;

namespace IcarusGG {

  public class EnterMechInterractable : MonoBehaviour {

    void Awake() {
      this.gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other) {
      Debug.Log($"EnterMech trigger enter by {other.tag}.tag");
    }

    void OnTriggerExit(Collider other) {
      Debug.Log($"EnterMech trigger exit by {other.tag}.tag");
    }

    void OnCollisionEnter(Collider other) {
      Debug.Log($"EnterMech collider enter by {other.tag}.tag");
    }

    void OnCollisionExit(Collider other) {
      Debug.Log($"EnterMech collider exit by {other.tag}.tag");
    }

  }

}