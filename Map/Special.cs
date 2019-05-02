using System;
namespace BabaIsYou.Map {
	public enum SpecialType : byte {
		Level,
		Controls,
		Flower,
		Art,
		Unknown
	}
	public class Special : Item {
		public static string FLOWER = "flower,5,3,1";
		public static string CONTROLS = "controls,idle";
		public byte X;
		public byte Y;
		public Special() {
			ID = -2;
			Layer = 150;
		}
		public override Item Copy() {
			return new Special() {
				ID = ID,
				Position = Position,
				Object = Object,
				Name = Name,
				Sprite = Sprite,
				SpriteInRoot = SpriteInRoot,
				IsObject = IsObject,
				Active = Active,
				Changed = Changed,
				Sleeping = Sleeping,
				Type = Type,
				Tiling = Tiling,
				Layer = Layer,
				Direction = Direction,
				Color = Color,
				ActiveColor = ActiveColor,
				OperatorType = OperatorType,
				ArgExtra = ArgExtra,
				X = X,
				Y = Y
			};
		}
		public Level GetLevel() {
			return new Level(Object);
		}
		public Flower GetFlower() {
			return new Flower(Object);
		}
		public GameControl GetGameControl() {
			return new GameControl(Object);
		}
		public override string ToString() {
			return $"{Object}";
		}
	}
	public class Flower {
		public short Color;
		public short InnerColor = 1026;
		public byte Radius;
		public Flower(string data) {
			int index = -1;
			string obj = Reader.ParseStringToComma(data, ref index, "flower").ToLower();
			if (obj != "flower") { throw new Exception("Invalid flower object"); }

			int colorX = Reader.ParseInt(Reader.ParseStringToComma(data, ref index, "0"));
			int colorY = Reader.ParseInt(Reader.ParseStringToComma(data, ref index, "0"));
			Color = (short)((colorY << 8) + colorX);
			Radius = Reader.ParseByte(Reader.ParseStringToComma(data, ref index, "1"));
		}
		public override string ToString() {
			return $"{Color} - {Radius}";
		}
	}
	public enum ControlType : byte {
		Up,
		Left,
		Right,
		Down,
		Idle,
		Undo,
		Pause,
		Unknown
	}
	public class GameControl {
		public ControlType Type;
		public GameControl(string data) {
			int index = -1;
			string obj = Reader.ParseStringToComma(data, ref index, "controls").ToLower();
			if (obj != "controls") { throw new Exception("Invalid controls object"); }

			string type = Reader.ParseStringToComma(data, ref index, "Unknown");
			if (!Enum.TryParse<ControlType>(type, true, out Type)) {
				Type = ControlType.Unknown;
			}
		}
		public override string ToString() {
			return $"{Type.ToString()}";
		}
	}
}