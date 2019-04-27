namespace BabaIsYou.Zip {
	public class PendingBuffer {
		/// <summary>
		/// Internal work buffer
		/// </summary>
		private readonly byte[] buffer;
		private int start;
		private int end;
		private uint bits;
		private int bitCount;

		/// <summary>
		/// construct instance using default buffer size of 4096
		/// </summary>
		public PendingBuffer() : this(4096) {
		}

		/// <summary>
		/// construct instance using specified buffer size
		/// </summary>
		/// <param name="bufferSize">
		/// size to use for internal buffer
		/// </param>
		public PendingBuffer(int bufferSize) {
			buffer = new byte[bufferSize];
		}

		/// <summary>
		/// Clear internal state/buffers
		/// </summary>
		public void Reset() {
			start = end = bitCount = 0;
		}

		/// <summary>
		/// Write a byte to buffer
		/// </summary>
		/// <param name="value">
		/// The value to write
		/// </param>
		public void WriteByte(int value) {
			buffer[end++] = unchecked((byte)value);
		}

		/// <summary>
		/// Write a short value to buffer LSB first
		/// </summary>
		/// <param name="value">
		/// The value to write.
		/// </param>
		public void WriteShort(int value) {
			buffer[end++] = unchecked((byte)value);
			buffer[end++] = unchecked((byte)(value >> 8));
		}

		/// <summary>
		/// write an integer LSB first
		/// </summary>
		/// <param name="value">The value to write.</param>
		public void WriteInt(int value) {
			buffer[end++] = unchecked((byte)value);
			buffer[end++] = unchecked((byte)(value >> 8));
			buffer[end++] = unchecked((byte)(value >> 16));
			buffer[end++] = unchecked((byte)(value >> 24));
		}

		/// <summary>
		/// Write a block of data to buffer
		/// </summary>
		/// <param name="block">data to write</param>
		/// <param name="offset">offset of first byte to write</param>
		/// <param name="length">number of bytes to write</param>
		public void WriteBlock(byte[] block, int offset, int length) {
			System.Array.Copy(block, offset, buffer, end, length);
			end += length;
		}

		/// <summary>
		/// The number of bits written to the buffer
		/// </summary>
		public int BitCount {
			get {
				return bitCount;
			}
		}

		/// <summary>
		/// Align internal buffer on a byte boundary
		/// </summary>
		public void AlignToByte() {
			if (bitCount > 0) {
				buffer[end++] = unchecked((byte)bits);
				if (bitCount > 8) {
					buffer[end++] = unchecked((byte)(bits >> 8));
				}
			}
			bits = 0;
			bitCount = 0;
		}

		/// <summary>
		/// Write bits to internal buffer
		/// </summary>
		/// <param name="b">source of bits</param>
		/// <param name="count">number of bits to write</param>
		public void WriteBits(int b, int count) {
			bits |= (uint)(b << bitCount);
			bitCount += count;
			if (bitCount >= 16) {
				buffer[end++] = unchecked((byte)bits);
				buffer[end++] = unchecked((byte)(bits >> 8));
				bits >>= 16;
				bitCount -= 16;
			}
		}

		/// <summary>
		/// Write a short value to internal buffer most significant byte first
		/// </summary>
		/// <param name="s">value to write</param>
		public void WriteShortMSB(int s) {
			buffer[end++] = unchecked((byte)(s >> 8));
			buffer[end++] = unchecked((byte)s);
		}

		/// <summary>
		/// Indicates if buffer has been flushed
		/// </summary>
		public bool IsFlushed {
			get {
				return end == 0;
			}
		}

		/// <summary>
		/// Flushes the pending buffer into the given output array.  If the
		/// output array is to small, only a partial flush is done.
		/// </summary>
		/// <param name="output">The output array.</param>
		/// <param name="offset">The offset into output array.</param>
		/// <param name="length">The maximum number of bytes to store.</param>
		/// <returns>The number of bytes flushed.</returns>
		public int Flush(byte[] output, int offset, int length) {
			if (bitCount >= 8) {
				buffer[end++] = unchecked((byte)bits);
				bits >>= 8;
				bitCount -= 8;
			}

			if (length > end - start) {
				length = end - start;
				System.Array.Copy(buffer, start, output, offset, length);
				start = 0;
				end = 0;
			} else {
				System.Array.Copy(buffer, start, output, offset, length);
				start += length;
			}
			return length;
		}

		/// <summary>
		/// Convert internal buffer to byte array.
		/// Buffer is empty on completion
		/// </summary>
		/// <returns>
		/// The internal buffer contents converted to a byte array.
		/// </returns>
		public byte[] ToByteArray() {
			AlignToByte();

			byte[] result = new byte[end - start];
			System.Array.Copy(buffer, start, result, 0, result.Length);
			start = 0;
			end = 0;
			return result;
		}
	}
}