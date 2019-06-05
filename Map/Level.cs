using System;
namespace BabaIsYou.Map {
	public enum LevelStyle : byte {
		Number,
		Letter,
		Dot,
		Icon = 255
	}
	public enum LevelState : byte {
		Hidden,
		Normal,
		Opened
	}
	public class Level : Item {
		public string File;
		public byte X;
		public byte Y;
		public byte Number;
		public byte Style;
		public byte State;
		public byte SpriteNum;
		public byte SpriteNumExtra;

		public override Item Copy() {
			return new Level() {
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
				ArgExtra = ArgExtra,
				ArgType = ArgType,
				File = File,
				X = X,
				Y = Y,
				Number = Number,
				Style = Style,
				State = State,
				SpriteNum = SpriteNum,
				SpriteNumExtra = SpriteNumExtra
			};
		}

		public Level() {
			ID = 2568;
			Name = "level";
			Sprite = "default";
			Tiling = 255;
			Object = "level";
			Layer = 100;
			IsObject = true;
			SpriteInRoot = true;
			Color = 768;
			ActiveColor = 512;
			State = 1;
		}
		public Level(string data) : this() {
			int index = -1;
			string obj = Reader.ParseStringToComma(data, ref index, "level").ToLower();
			if (obj != "level") { throw new Exception("Invalid level object"); }

			File = Reader.ParseStringToComma(data, ref index, "0level");
			Name = File;
			Style = Reader.ParseByte(Reader.ParseStringToComma(data, ref index, "2"));
			Number = Reader.ParseByte(Reader.ParseStringToComma(data, ref index, "0"));
			State = Reader.ParseByte(Reader.ParseStringToComma(data, ref index, "0"));
		}
		public override string ToString() {
			return $"{File} - {Name} - {Number} [{X}, {Y}]";
		}
	}
}