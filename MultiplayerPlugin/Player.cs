using System;
using DarkRift;

namespace MultiplayerPlugin
{
	public class Player : IDarkRiftSerializable
	{
		public ushort ID { get; set; }
		public string playerName { get; set; }
		public bool isReady { get; set; }

		public float X { get; set; }
		public float Y { get; set; }
		public float Z { get; set; }

		public byte ColorR { get; set; }
		public byte ColorG { get; set; }
		public byte ColorB { get; set; }

		public Player()
    {

    }

		public Player(ushort _ID, string _playerName)
		{
			ID = _ID;
			playerName = _playerName;
			isReady = false;

			Random r = new Random();

			X = Math.Ceiling((float)r.NextDouble() * 5f);
			Y = 0;
			Z = 0;

			ColorR = (byte)r.Next(0, 200);
			ColorG = (byte)r.Next(0, 200);
			ColorB = (byte)r.Next(0, 200);

      Debug.Log($"New Player created x{X} y{Y} z{Z}");
		}

		public void Deserialize(DeserializeEvent e)
    {
			ID = e.Reader.ReadUInt16();
			playerName = e.Reader.ReadString();

			X = e.Reader.ReadSingle();
			Y = e.Reader.ReadSingle();
			Z = e.Reader.ReadSingle();

			ColorR = e.Reader.ReadByte();
			ColorG = e.Reader.ReadByte();
			ColorB = e.Reader.ReadByte();
		}

		public void Serialize(SerializeEvent e)
    {
			e.Writer.Write(ID);
			e.Writer.Write(playerName);

			e.Writer.Write(X);
			e.Writer.Write(Y);
			e.Writer.Write(Z);

			e.Writer.Write(ColorR);
			e.Writer.Write(ColorG);
			e.Writer.Write(ColorB);
		}
	}
}
