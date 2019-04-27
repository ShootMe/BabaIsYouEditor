namespace BabaIsYou.Map {
	public class Line : Item {
		public byte X;
		public byte Y;
		public byte Style;
		public byte Gate;
		public byte Requirement;

		public Line() {
			Object = "object117";
		}

		public override Item Copy() {
			return new Line() {
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
				X = X,
				Y = Y,
				Style = Style,
				Gate = Gate,
				Requirement = Requirement
			};
		}
	}
}