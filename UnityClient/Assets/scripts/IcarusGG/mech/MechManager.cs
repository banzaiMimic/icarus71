using System.Collections.Generic;
//using UnityEngine;

namespace IcarusGG {

  public sealed class MechManager {

    private static readonly MechManager instance = new MechManager();

    static MechManager() {}
    private MechManager(){}

    public static MechManager INSTANCE {
      get {
        return instance;
      }
    }

    private Dictionary<string, Mech> mechs = new Dictionary<string, Mech>();

    public void addMech(Mech mech) {
      System.Diagnostics.Debug.WriteLine($"adding Mech with uid: {mech.uid}");
      //Debug.Log($"adding Mech with uid: {mech.uid}");
      mechs.Add(mech.uid, mech);
    }

    public Mech getMech(string uid) {
      return mechs[uid];
    }

  }
}