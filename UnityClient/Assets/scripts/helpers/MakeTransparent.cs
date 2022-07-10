namespace gg.icarus {
  using UnityEngine;

  public class MakeTransparent : MonoBehaviour {
//@Recall test transparency after attaching this to game object 
// interractable smaller collider 
// check if that fires off to player...
    [SerializeField]
    private float alpha = 0.25f;

    void Awake() {
      Material mat = this.gameObject.GetComponent<MeshRenderer>().material;
      Color color = mat.color;
      Color colorUpdate = new Color(color.r, color.g, color.b, alpha);
      mat.SetColor("_Color", colorUpdate);
    }
    
  }

}