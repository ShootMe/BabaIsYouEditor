namespace BabaIsYou.Map {
	public enum PathGate : byte {
		None,
		Level,
		Area,
		Orb
	}
	public enum PathStyle : byte {
		Hidden,
		Visible
	}
	public class LevelPath : Item {
		public byte X;
		public byte Y;
		public byte Style;
		public byte Gate;
		public byte Requirement;

		public LevelPath() {
			Object = "object117";
		}
		public void UpdatePath() {
			Item item = Reader.DefaultsByObject[Object];
			ID = item.ID;
			Sprite = item.Sprite;
			Name = item.Name;
			SpriteInRoot = item.SpriteInRoot;
			IsObject = item.IsObject;
			Type = item.Type;
			Color = item.Color;
			ActiveColor = item.ActiveColor;
			Tiling = item.Tiling;
			Layer = item.Layer;
		}

		public override Item Copy() {
			return new LevelPath() {
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
				ArgType = ArgType,
				X = X,
				Y = Y,
				Style = Style,
				Gate = Gate,
				Requirement = Requirement
			};
		}
	}
}