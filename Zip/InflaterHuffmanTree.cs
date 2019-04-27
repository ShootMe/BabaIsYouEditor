using System;
using System.Collections.Generic;
namespace BabaIsYou.Zip {
	public class InflaterHuffmanTree {
		private const int MAX_BITLEN = 15;
		private short[] tree;
		public static InflaterHuffmanTree defLitLenTree;
		public static InflaterHuffmanTree defDistTree;
		private static readonly byte[] bit4Reverse = { 0, 8, 4, 12, 2, 10, 6, 14, 1, 9, 5, 13, 3, 11, 7, 15 };

		static InflaterHuffmanTree() {
			try {
				byte[] codeLengths = new byte[288];
				int i = 0;
				while (i < 144) {
					codeLengths[i++] = 8;
				}
				while (i < 256) {
					codeLengths[i++] = 9;
				}
				while (i < 280) {
					codeLengths[i++] = 7;
				}
				while (i < 288) {
					codeLengths[i++] = 8;
				}
				defLitLenTree = new InflaterHuffmanTree(codeLengths);

				codeLengths = new byte[32];
				i = 0;
				while (i < 32) {
					codeLengths[i++] = 5;
				}
				defDistTree = new InflaterHuffmanTree(codeLengths);
			} catch (Exception) {
				throw new Exception("InflaterHuffmanTree: static tree length illegal");
			}
		}

		/// <summary>
		/// Constructs a Huffman tree from the array of code lengths.
		/// </summary>
		/// <param name = "codeLengths">
		/// the array of code lengths
		/// </param>
		public InflaterHuffmanTree(IList<byte> codeLengths) {
			BuildTree(codeLengths);
		}

		private void BuildTree(IList<byte> codeLengths) {
			int[] blCount = new int[MAX_BITLEN + 1];
			int[] nextCode = new int[MAX_BITLEN + 1];

			for (int i = 0; i < codeLengths.Count; i++) {
				int bits = codeLengths[i];
				if (bits > 0) {
					blCount[bits]++;
				}
			}

			int code = 0;
			int treeSize = 512;
			for (int bits = 1; bits <= MAX_BITLEN; bits++) {
				nextCode[bits] = code;
				code += blCount[bits] << (16 - bits);
				if (bits >= 10) {
					/* We need an extra table for bit lengths >= 10. */
					int start = nextCode[bits] & 0x1ff80;
					int end = code & 0x1ff80;
					treeSize += (end - start) >> (16 - bits);
				}
			}

			/* Now create and fill the extra tables from longest to shortest
			* bit len.  This way the sub trees will be aligned.
			*/
			tree = new short[treeSize];
			int treePtr = 512;
			for (int bits = MAX_BITLEN; bits >= 10; bits--) {
				int end = code & 0x1ff80;
				code -= blCount[bits] << (16 - bits);
				int start = code & 0x1ff80;
				for (int i = start; i < end; i += 1 << 7) {
					tree[BitReverse(i)] = (short)((-treePtr << 4) | bits);
					treePtr += 1 << (bits - 9);
				}
			}

			for (int i = 0; i < codeLengths.Count; i++) {
				int bits = codeLengths[i];
				if (bits == 0) {
					continue;
				}
				code = nextCode[bits];
				int revcode = BitReverse(code);
				if (bits <= 9) {
					do {
						tree[revcode] = (short)((i << 4) | bits);
						revcode += 1 << bits;
					} while (revcode < 512);
				} else {
					int subTree = tree[revcode & 511];
					int treeLen = 1 << (subTree & 15);
					subTree = -(subTree >> 4);
					do {
						tree[subTree | (revcode >> 9)] = (short)((i << 4) | bits);
						revcode += 1 << bits;
					} while (revcode < treeLen);
				}
				nextCode[bits] = code + (1 << (16 - bits));
			}
		}
		public static short BitReverse(int toReverse) {
			return (short)(bit4Reverse[toReverse & 0xF] << 12 |
							bit4Reverse[(toReverse >> 4) & 0xF] << 8 |
							bit4Reverse[(toReverse >> 8) & 0xF] << 4 |
							bit4Reverse[toReverse >> 12]);
		}

		/// <summary>
		/// Reads the next symbol from input.  The symbol is encoded using the
		/// huffman tree.
		/// </summary>
		/// <param name="input">
		/// input the input source.
		/// </param>
		/// <returns>
		/// the next symbol, or -1 if not enough input is available.
		/// </returns>
		public int GetSymbol(StreamManipulator input) {
			int lookahead, symbol;
			if ((lookahead = input.PeekBits(9)) >= 0) {
				symbol = tree[lookahead];
				int bitlen = symbol & 15;

				if (symbol >= 0) {
					if (bitlen == 0) {
						throw new Exception("Encountered invalid codelength 0");
					}
					input.DropBits(bitlen);
					return symbol >> 4;
				}
				int subtree = -(symbol >> 4);
				if ((lookahead = input.PeekBits(bitlen)) >= 0) {
					symbol = tree[subtree | (lookahead >> 9)];
					input.DropBits(symbol & 15);
					return symbol >> 4;
				} else {
					int bits = input.AvailableBits;
					lookahead = input.PeekBits(bits);
					symbol = tree[subtree | (lookahead >> 9)];
					if ((symbol & 15) <= bits) {
						input.DropBits(symbol & 15);
						return symbol >> 4;
					} else {
						return -1;
					}
				}
			} else // Less than 9 bits
			  {
				int bits = input.AvailableBits;
				lookahead = input.PeekBits(bits);
				symbol = tree[lookahead];
				if (symbol >= 0 && (symbol & 15) <= bits) {
					input.DropBits(symbol & 15);
					return symbol >> 4;
				} else {
					return -1;
				}
			}
		}
	}
}