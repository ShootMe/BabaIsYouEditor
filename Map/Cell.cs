using System;
using System.Collections.Generic;
using System.Drawing;
namespace BabaIsYou.Map {
	public class Cell {
		public short Position;
		public List<Item> Objects = new List<Item>();

		public Cell(short position) {
			Position = position;
		}

		public bool HasLevelPath() {
			for (int i = 0; i < Objects.Count; i++) {
				Item item = Objects[i];
				if (item is Level || item is LevelPath) {
					return true;
				}
			}
			return false;
		}
		public bool HasObject<T>() {
			for (int i = 0; i < Objects.Count; i++) {
				Item item = Objects[i];
				if (item is T) {
					return true;
				}
			}
			return false;
		}
		public int CountOfType<T>() {
			int count = 0;
			for (int i = 0; i < Objects.Count; i++) {
				Item item = Objects[i];
				if (item is T) {
					count++;
				}
			}
			return count;
		}
		public int LayerCount() {
			int count = 0;
			for (int i = 0; i < Objects.Count; i++) {
				Item item = Objects[i];
				if (!(item is Level) && !(item is LevelPath) && !(item is Special)) {
					count++;
				}
			}
			return count;
		}
		public Item GetNextObject(Item current) {
			Item next = null;
			for (int i = Objects.Count - 1; i >= 0; i--) {
				Item item = Objects[i];
				if (item.ID != 0) {
					if (next == null || current == null || item.Layer < current.Layer || (item.Layer == current.Layer && item.ID > current.ID)) {
						if (next != null || current == null) {
							return item;
						}
						next = item;
					}
				}
			}
			return next;
		}
		public Point GetLocation(int width, int height) {
			return new Point(Position % width, Position / width);
		}
		public Item GetExtraObject() {
			for (int i = 0; i < Objects.Count; i++) {
				Item test = Objects[i];
				if (test is Level || test is LevelPath || test is Special) {
					return test;
				}
			}
			return null;
		}
		public T GetExtraObject<T>() where T : Item {
			for (int i = 0; i < Objects.Count; i++) {
				Item test = Objects[i];
				if (test is T) {
					return (T)test;
				}
			}
			return null;
		}
		public Item GetObject(Item item) {
			for (int i = 0; i < Objects.Count; i++) {
				Item test = Objects[i];
				if (test.GetType() == item.GetType() && (test.GetType() != typeof(Item) || test.ID == item.ID)) {
					return test;
				}
			}
			return null;
		}
		public bool ContainsObject(Item item) {
			for (int i = 0; i < Objects.Count; i++) {
				Item test = Objects[i];
				if (test.GetType() == item.GetType() && (test.GetType() != typeof(Item) || test.ID == item.ID)) {
					return true;
				}
			}
			return false;
		}
		public bool RemoveObjectOfType(Item item) {
			for (int i = Objects.Count - 1; i >= 0; i--) {
				Item test = Objects[i];
				if (test.GetType() == item.GetType() && (test.GetType() != typeof(Item) || test.ID == item.ID)) {
					Objects.RemoveAt(i);
					return true;
				}
			}
			return false;
		}
		public override string ToString() {
			return $"{Position} [{Objects.Count}]";
		}
		public override bool Equals(object obj) {
			return obj != null && (obj is Cell cell) && cell.Position == Position;
		}
		public override int GetHashCode() {
			return Position;
		}
		public static bool operator ==(Cell one, Cell two) {
			bool oneNull = (object)one == null;
			bool twoNull = (object)two == null;
			return oneNull == twoNull && (oneNull || one.Position == two.Position);
		}
		public static bool operator !=(Cell one, Cell two) {
			bool oneNull = (object)one == null;
			bool twoNull = (object)two == null;
			return oneNull != twoNull || (!oneNull && one.Position != two.Position);
		}
	}
}