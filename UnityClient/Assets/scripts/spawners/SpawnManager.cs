using UnityEngine;

namespace gg.icarus {

  public class SpawnManager : MonoBehaviour {

    [SerializeField] private ScriptableObject enterMech;

    void Awake() {
      Instantiate(enterMech);
    }

  }

}
