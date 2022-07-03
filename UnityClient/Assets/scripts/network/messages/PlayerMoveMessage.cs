using DarkRift;
using UnityEngine;

public class PlayerMoveMessage : IDarkRiftSerializable {
  public ushort ID { get; set; }

  public Vector3 vrCamera { get; set; }
  public Vector3 leftHand { get; set; }
  public Vector3 rightHand { get; set; }

  public PlayerMoveMessage() {}

  public PlayerMoveMessage(
    float camX, float camY, float camZ,
    float lhX, float lhY, float lhZ,
    float rhX, float rhY, float rhZ
  ) {
    this.vrCamera = new Vector3(camX, camY, camZ);
    this.leftHand = new Vector3(lhX, lhY, lhZ);
    this.rightHand = new Vector3(rhX, rhY, rhZ);
  }

  public void Deserialize(DeserializeEvent e) {
    ID = e.Reader.ReadUInt16();
    vrCamera = new Vector3(e.Reader.ReadSingle(), e.Reader.ReadSingle(), e.Reader.ReadSingle());
    leftHand = new Vector3(e.Reader.ReadSingle(), e.Reader.ReadSingle(), e.Reader.ReadSingle());
    rightHand = new Vector3(e.Reader.ReadSingle(), e.Reader.ReadSingle(), e.Reader.ReadSingle());
  }

  public void Serialize(SerializeEvent e) {
    e.Writer.Write(vrCamera.x);
    e.Writer.Write(vrCamera.y);
    e.Writer.Write(vrCamera.z);
    e.Writer.Write(leftHand.x);
    e.Writer.Write(leftHand.y);
    e.Writer.Write(leftHand.z);
    e.Writer.Write(rightHand.x);
    e.Writer.Write(rightHand.y);
    e.Writer.Write(rightHand.z);
  }
}