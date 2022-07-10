using UnityEngine;

namespace IcarusGG {

  [CreateAssetMenu(menuName = "gg.InterractableData")]

  public class InterractableData : ScriptableObject {

    [SerializeField] private string id;
    [SerializeField] private GameObject associatedPrefab;

    // GameObject
    // trigger to detect player interract...
    // action to dispatch?

    void spawnAt( Vector3 position) {
      
    }

    private void Awake() {
      Debug.Log($"scriptableObject instantiated - Awake method");
    }

    private void OnEnable() {

    }

    private void OnDisable() {

    }

    private void OnDestroy() {

    }
  
  }

}