using System;
using DarkRift;

namespace MultiplayerPlugin
{
	public class Player : IDarkRiftSerializable
	{
		public ushort ID { get; set; }
		public string playerName { get; set; }
		public bool isReady { get; set; }

		public float camX { get; set; }
		public float camY { get; set; }
		public float camZ { get; set; }
    
    public float leftHandX { get; set; }
		public float leftHandY { get; set; }
		public float leftHandZ { get; set; }
    
    public float rightHandX { get; set; }
		public float rightHandY { get; set; }
		public float rightHandZ { get; set; }

		public byte ColorR { get; set; }
		public byte ColorG { get; set; }
		public byte ColorB { get; set; }

		public Player() {}

		public Player(ushort _ID, string _playerName)
		{
			ID = _ID;
			playerName = _playerName;
			isReady = false;

			Random r = new Random();

			// ColorR = (byte)r.Next(0, 200);
			// ColorG = (byte)r.Next(0, 200);
			// ColorB = (byte)r.Next(0, 200);
		}

		public void Deserialize(DeserializeEvent e)
    {
			ID = e.Reader.ReadUInt16();
			playerName = e.Reader.ReadString();

			camX = e.Reader.ReadSingle();
			camY = e.Reader.ReadSingle();
			camZ = e.Reader.ReadSingle();

			// ColorR = e.Reader.ReadByte();
			// ColorG = e.Reader.ReadByte();
			// ColorB = e.Reader.ReadByte();
		}

		public void Serialize(SerializeEvent e)
    {
			e.Writer.Write(ID);
			e.Writer.Write(playerName);

			e.Writer.Write(camX);
			e.Writer.Write(camY);
			e.Writer.Write(camZ);

			// e.Writer.Write(ColorR);
			// e.Writer.Write(ColorG);
			// e.Writer.Write(ColorB);
		}
	}
}
