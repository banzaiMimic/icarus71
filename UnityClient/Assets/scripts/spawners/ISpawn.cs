using UnityEngine;
/*
  spawns / despawns gameObjects at Vector3 position
*/
namespace gg.icarus {
  
  public interface ISpawn {

    public void spawnAt( Vector3 position);

  }

}