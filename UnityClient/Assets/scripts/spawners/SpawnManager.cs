using UnityEngine;

namespace IcarusGG {

  public class SpawnManager : MonoBehaviour {

    [SerializeField] private ScriptableObject enterMech;

    void Awake() {
      Instantiate(enterMech);
    }

  }

}
