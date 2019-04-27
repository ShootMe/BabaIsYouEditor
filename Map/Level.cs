namespace BabaIsYou.Map {
	public enum LevelStyle : byte {
		Number,
		Text,
		Dot,
		Custom = 255
	}
	public class Level : Item {
		public static Level DEFAULT = new Level() { ID = 2568, Name = "level", Sprite = "default", Tiling = 255, Object = "level", Layer = 30, IsObject = true, SpriteInRoot = true };
		public string File;
		public byte X;
		public byte Y;
		public byte Number;
		public byte Style;

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
				File = File,
				X = X,
				Y = Y,
				Number = Number,
				Style = Style
			};
		}
		public override string ToString() {
			return $"{File} - {Name} - {Number} [{X}, {Y}]";
		}
	}
}