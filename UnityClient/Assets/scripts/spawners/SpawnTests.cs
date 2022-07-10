using UnityEngine;

namespace IcarusGG {

  public class SpawnTests : MonoBehaviour {

    public ScriptableObject SO_Test;

    void Awake() {
      Instantiate(SO_Test);
    }

  }

}