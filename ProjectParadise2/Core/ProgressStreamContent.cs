using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProjectParadise2
{
    /// <summary>
    /// Represents a custom <see cref="HttpContent"/> that allows reporting progress while streaming content to the HTTP response stream.
    /// </summary>
    public class ProgressStreamContent : HttpContent
    {
        private readonly Stream _stream;
        private readonly int _bufferSize;
        private readonly IProgress<long> _progress;
        private readonly long _totalLength;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressStreamContent"/> class.
        /// </summary>
        /// <param name="stream">The stream containing the data to be sent.</param>
        /// <param name="totalLength">The total length of the data being sent (used to report progress).</param>
        /// <param name="progress">An <see cref="IProgress{T}"/> instance used to report progress.</param>
        /// <param name="bufferSize">The buffer size used for reading from the stream (default is 8192 bytes).</param>
        public ProgressStreamContent(Stream stream, long totalLength, IProgress<long> progress, int bufferSize = 8192)
        {
            _stream = stream ?? throw new ArgumentNullException(nameof(stream));
            _totalLength = totalLength;
            _progress = progress;
            _bufferSize = bufferSize;
        }

        /// <summary>
        /// Asynchronously serializes the content to the provided stream, reporting progress as data is read.
        /// </summary>
        /// <param name="stream">The stream to which the content is serialized.</param>
        /// <param name="context">The transport context (not used in this implementation).</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            var buffer = new byte[_bufferSize];
            long totalBytesRead = 0;

            // Read the stream and write it to the output stream, reporting progress
            while (true)
            {
                var bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);
                if (bytesRead == 0) break;

                await stream.WriteAsync(buffer, 0, bytesRead);
                totalBytesRead += bytesRead;

                _progress?.Report(totalBytesRead);
            }
        }

        /// <summary>
        /// Attempts to compute the length of the content.
        /// </summary>
        /// <param name="length">The computed length of the content.</param>
        /// <returns>True if the length was successfully computed, otherwise false.</returns>
        protected override bool TryComputeLength(out long length)
        {
            length = _totalLength;
            return true;
        }
    }
}