# UnityClient
VR-ready template for Unity connecting to DarkRift standalone server

### development
- Unity 2021.3.4f1
- original VR template using steamVR unity plugin https://valvesoftware.github.io/steamvr_unity_plugin/

### ref
- check automated artifact building for both SteamVR and Oculus Quest -> https://exyte.com/blog/automated-artifact-building-for-both-steamvr-and-oculus-quest
- laser pointer --> https://setzeus.medium.com/tutorial-steamvr-2-0-laser-pointer-bbc816ebeec5

## dev.notes
misc notes picked up along the way (plan to move this to some cookbook later?)

### interacting with canvas / buttons / ui / etc.
- we can use steamVR plugin's `SteamVR_LaserPointer` for the heavy lifting here.
- example is inside of our `MenuManager`, basically you add an event listener for the PointerClick event i.e. `leftLaserPointer.PointerClick += PointerClick;`
- steamVR controller needs to have `SteamVR_LaserPointer` script attached & dragged into MenuManager so that it can be accessed
- only thing left is on the canvas / ui / button etc. it needs to have a `Box Collider` so that the laser can interract with it

### commenting
```
/// <summary>
/// some summary
/// <example>
/// For example:
/// <code>
/// Point p = new Point(3,5);
/// p.Translate(-1,3);
/// </code>
/// results in <c>p</c>'s having the value (2,8).
/// </example>
/// </summary>
```