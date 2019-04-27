using System;
namespace BabaIsYou.Map {
	public enum Direction : byte {
		Right,
		Up,
		Left,
		Down,
		None
	}
	public enum Tiling : byte {
		Directional,
		Tiled,
		Character,
		Animated,
		SingleAnimated,
		None = 255
	}
	public enum TextType : byte {
		Noun,
		Link,
		Action,
		Before,
		Not,
		Letter,
		And,
		Extra
	}
	public class Item : IComparable, IComparable<Item> {
		public static Item EMPTY = new Item() { ID = -1, Name = "empty", Object = "empty" };
		public short ID;
		public short Position;
		public string Object;
		public string Name;
		public string Sprite;
		public bool SpriteInRoot;
		public bool IsObject;
		public bool Active;
		public bool Changed;
		public bool Sleeping;
		public byte Type;
		public byte Tiling;
		public byte Layer;
		public byte Direction;
		public short Color;
		public short ActiveColor;

		public Item() {
			Active = true;
			ActiveColor = -1;
		}
		public virtual Item Copy() {
			return new Item() {
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
				ActiveColor = ActiveColor
			};
		}
		public int CompareTo(object obj) {
			if (obj is Item item) {
				return CompareTo(item);
			}
			return -1;
		}
		public int CompareTo(Item other) {
			if (Layer > other.Layer) {
				return 1;
			} else if (Layer < other.Layer) {
				return -1;
			}
			return ID < other.ID ? 1 : ID > other.ID ? -1 : 0;
		}
		public override string ToString() {
			return $"{ID} {Name} {Direction} {Active}";
		}
		public override bool Equals(object obj) {
			return obj != null && obj == (object)this;
		}
		public override int GetHashCode() {
			return ID;
		}
		public static bool operator ==(Item one, Item two) {
			return (object)one == (object)two;
		}
		public static bool operator !=(Item one, Item two) {
			return (object)one != (object)two;
		}
	}
}