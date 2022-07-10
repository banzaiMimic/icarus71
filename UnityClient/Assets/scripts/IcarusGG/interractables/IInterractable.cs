using UnityEngine;

namespace IcarusGG {

  public interface IInterractable {

    public void onTriggerEnter(Collider other);
    public void onTriggerExit(Collider other);
    public void onCollisionEnter(Collision other);
    public void onCollisionExit(Collision other);

  }

}