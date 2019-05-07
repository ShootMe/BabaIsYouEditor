using System;
using System.IO;
namespace BabaIsYou.Zip {
	public class DeflaterOutputStream : Stream {
		public DeflaterOutputStream() : this(null) {
		}
		/// <summary>
		/// Creates a new DeflaterOutputStream with a default Deflater and default buffer size.
		/// </summary>
		/// <param name="baseOutputStream">
		/// the output stream where deflated output should be written.
		/// </param>
		public DeflaterOutputStream(Stream baseOutputStream)
			: this(baseOutputStream, new Deflater(), 512) {
		}

		/// <summary>
		/// Creates a new DeflaterOutputStream with the given Deflater and
		/// default buffer size.
		/// </summary>
		/// <param name="baseOutputStream">
		/// the output stream where deflated output should be written.
		/// </param>
		/// <param name="deflater">
		/// the underlying deflater.
		/// </param>
		public DeflaterOutputStream(Stream baseOutputStream, Deflater deflater)
			: this(baseOutputStream, deflater, 512) {
		}

		/// <summary>
		/// Creates a new DeflaterOutputStream with the given Deflater and
		/// buffer size.
		/// </summary>
		/// <param name="baseOutputStream">
		/// The output stream where deflated output is written.
		/// </param>
		/// <param name="deflater">
		/// The underlying deflater to use
		/// </param>
		/// <param name="bufferSize">
		/// The buffer size in bytes to use when deflating (minimum value 512)
		/// </param>
		/// <exception cref="ArgumentOutOfRangeException">
		/// bufsize is less than or equal to zero.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// baseOutputStream does not support writing
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// deflater instance is null
		/// </exception>
		public DeflaterOutputStream(Stream baseOutputStream, Deflater deflater, int bufferSize) {
			if (bufferSize < 512) {
				throw new ArgumentOutOfRangeException(nameof(bufferSize));
			}

			this.baseOutputStream = baseOutputStream;
			buffer = new byte[bufferSize];
			this.deflater = deflater ?? throw new ArgumentNullException(nameof(deflater));
		}

		/// <summary>
		/// Finishes the stream by calling finish() on the deflater.
		/// </summary>
		/// <exception cref="SharpZipBaseException">
		/// Not all input is deflated
		/// </exception>
		public virtual void Finish() {
			deflater.Finish();
			while (!deflater.IsFinished) {
				int len = deflater.Deflate(buffer, 0, buffer.Length);
				if (len <= 0) {
					break;
				}

				baseOutputStream.Write(buffer, 0, len);
			}

			if (!deflater.IsFinished) {
				throw new Exception("Can't deflate all input?");
			}

			baseOutputStream.Flush();
		}

		/// <summary>
		/// Gets or sets a flag indicating ownership of underlying stream.
		/// When the flag is true <see cref="Stream.Dispose()" /> will close the underlying stream also.
		/// </summary>
		/// <remarks>The default value is true.</remarks>
		public bool IsStreamOwner { get; set; } = true;

		///	<summary>
		/// Allows client to determine if an entry can be patched after its added
		/// </summary>
		public bool CanPatchEntries {
			get {
				return baseOutputStream.CanSeek;
			}
		}

		/// <summary>
		/// Deflates everything in the input buffers.  This will call
		/// <code>def.deflate()</code> until all bytes from the input buffers
		/// are processed.
		/// </summary>
		protected void Deflate() {
			while (!deflater.IsNeedingInput) {
				int deflateCount = deflater.Deflate(buffer, 0, buffer.Length);

				if (deflateCount <= 0) {
					break;
				}

				baseOutputStream.Write(buffer, 0, deflateCount);
			}

			if (!deflater.IsNeedingInput) {
				throw new Exception("DeflaterOutputStream can't deflate all input?");
			}
		}

		/// <summary>
		/// Gets value indicating stream can be read from
		/// </summary>
		public override bool CanRead {
			get {
				return false;
			}
		}

		/// <summary>
		/// Gets a value indicating if seeking is supported for this stream
		/// This property always returns false
		/// </summary>
		public override bool CanSeek {
			get {
				return false;
			}
		}

		/// <summary>
		/// Get value indicating if this stream supports writing
		/// </summary>
		public override bool CanWrite {
			get {
				return baseOutputStream.CanWrite;
			}
		}

		/// <summary>
		/// Get current length of stream
		/// </summary>
		public override long Length {
			get {
				return baseOutputStream.Length;
			}
		}

		/// <summary>
		/// Gets the current position within the stream.
		/// </summary>
		/// <exception cref="NotSupportedException">Any attempt to set position</exception>
		public override long Position {
			get {
				return baseOutputStream.Position;
			}
			set {
				throw new NotSupportedException("Position property not supported");
			}
		}

		/// <summary>
		/// Sets the current position of this stream to the given value. Not supported by this class!
		/// </summary>
		/// <param name="offset">The offset relative to the <paramref name="origin"/> to seek.</param>
		/// <param name="origin">The <see cref="SeekOrigin"/> to seek from.</param>
		/// <returns>The new position in the stream.</returns>
		/// <exception cref="NotSupportedException">Any access</exception>
		public override long Seek(long offset, SeekOrigin origin) {
			throw new NotSupportedException("DeflaterOutputStream Seek not supported");
		}

		/// <summary>
		/// Sets the length of this stream to the given value. Not supported by this class!
		/// </summary>
		/// <param name="value">The new stream length.</param>
		/// <exception cref="NotSupportedException">Any access</exception>
		public override void SetLength(long value) {
			throw new NotSupportedException("DeflaterOutputStream SetLength not supported");
		}

		/// <summary>
		/// Read a byte from stream advancing position by one
		/// </summary>
		/// <returns>The byte read cast to an int.  THe value is -1 if at the end of the stream.</returns>
		/// <exception cref="NotSupportedException">Any access</exception>
		public override int ReadByte() {
			throw new NotSupportedException("DeflaterOutputStream ReadByte not supported");
		}

		/// <summary>
		/// Read a block of bytes from stream
		/// </summary>
		/// <param name="buffer">The buffer to store read data in.</param>
		/// <param name="offset">The offset to start storing at.</param>
		/// <param name="count">The maximum number of bytes to read.</param>
		/// <returns>The actual number of bytes read.  Zero if end of stream is detected.</returns>
		/// <exception cref="NotSupportedException">Any access</exception>
		public override int Read(byte[] buffer, int offset, int count) {
			throw new NotSupportedException("DeflaterOutputStream Read not supported");
		}

		/// <summary>
		/// Flushes the stream by calling <see cref="DeflaterOutputStream.Flush">Flush</see> on the deflater and then
		/// on the underlying stream.  This ensures that all bytes are flushed.
		/// </summary>
		public override void Flush() {
			deflater.Flush();
			Deflate();
			baseOutputStream.Flush();
		}

		/// <summary>
		/// Calls <see cref="Finish"/> and closes the underlying
		/// stream when <see cref="IsStreamOwner"></see> is true.
		/// </summary>
		protected override void Dispose(bool disposing) {
			if (!isClosed) {
				isClosed = true;

				try {
					Finish();
				} finally {
					if (IsStreamOwner) {
						baseOutputStream.Dispose();
					}
				}
			}
		}

		/// <summary>
		/// Writes a single byte to the compressed output stream.
		/// </summary>
		/// <param name="value">
		/// The byte value.
		/// </param>
		public override void WriteByte(byte value) {
			byte[] b = new byte[1];
			b[0] = value;
			Write(b, 0, 1);
		}

		/// <summary>
		/// Writes bytes from an array to the compressed stream.
		/// </summary>
		/// <param name="buffer">
		/// The byte array
		/// </param>
		/// <param name="offset">
		/// The offset into the byte array where to start.
		/// </param>
		/// <param name="count">
		/// The number of bytes to write.
		/// </param>
		public override void Write(byte[] buffer, int offset, int count) {
			deflater.SetInput(buffer, offset, count);
			Deflate();
		}
		public void ResetStream(Stream newStream) {
			baseOutputStream = newStream;
			deflater.Reset();
		}

		/// <summary>
		/// This buffer is used temporarily to retrieve the bytes from the
		/// deflater and write them to the underlying output stream.
		/// </summary>
		private byte[] buffer;

		/// <summary>
		/// The deflater which is used to deflate the stream.
		/// </summary>
		protected Deflater deflater;

		/// <summary>
		/// Base stream the deflater depends on.
		/// </summary>
		protected Stream baseOutputStream;
		private bool isClosed;
	}
}