#region License

/*
 * Copyright 2021 JanusGraph.Net Authors
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#endregion

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Gremlin.Net.Structure.IO.GraphBinary;
using Gremlin.Net.Structure.IO.GraphBinary.Types;

namespace JanusGraph.Net.IO.GraphBinary.Types
{
    /// <summary>
    ///     Base class for GraphBinary serializers of JanusGraph types.
    /// </summary>
    public abstract class JanusGraphTypeSerializer : CustomTypeSerializer
    {
        private readonly GraphBinaryType _type;

        /// <summary>
        ///     Initializes a new instance of the <see cref="JanusGraphTypeSerializer" /> class.
        /// </summary>
        /// <param name="type">The GraphBinaryType for this serializer.</param>
        protected JanusGraphTypeSerializer(GraphBinaryType type)
        {
            _type = type;
        }

        /// <inheritdoc />
        public override async Task WriteAsync(object value, Stream stream, GraphBinaryWriter writer,
            CancellationToken cancellationToken = default)
        {
            await stream.WriteIntAsync(_type.TypeId, cancellationToken).ConfigureAwait(false);

            await WriteValueAsync(value, stream, writer, true, cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public override async Task WriteValueAsync(object value, Stream stream, GraphBinaryWriter writer, bool nullable,
            CancellationToken cancellationToken = default)
        {
            if (value == null)
            {
                if (!nullable)
                {
                    throw new IOException("Unexpected null value when nullable is false");
                }

                await writer.WriteValueFlagNullAsync(stream, cancellationToken).ConfigureAwait(false);
                return;
            }

            if (nullable)
            {
                await writer.WriteValueFlagNoneAsync(stream, cancellationToken).ConfigureAwait(false);
            }

            await WriteNonNullableValueAsync(value, stream, writer, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        ///     Writes a non-nullable value without including type information.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="writer">A <see cref="GraphBinaryWriter"/> that can be used to write nested values.</param>
        /// <param name="cancellationToken">The token to cancel the operation. The default value is None.</param>
        /// <returns>A task that represents the asynchronous write operation.</returns>
        protected abstract Task WriteNonNullableValueAsync(object value, Stream stream, GraphBinaryWriter writer,
            CancellationToken cancellationToken = default);

        /// <inheritdoc />
        public override async Task<object> ReadAsync(Stream stream, GraphBinaryReader reader,
            CancellationToken cancellationToken = default)
        {
            var customTypeInfo = await stream.ReadIntAsync(cancellationToken).ConfigureAwait(false);
            if (customTypeInfo != _type.TypeId)
            {
                throw new IOException(
                    $"Custom type info {customTypeInfo} doesn't match expected type info {_type.TypeId}");
            }

            return await ReadValueAsync(stream, reader, true, cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public override async Task<object> ReadValueAsync(Stream stream, GraphBinaryReader reader, bool nullable,
            CancellationToken cancellationToken = default)
        {
            if (nullable)
            {
                var valueFlag = await stream.ReadByteAsync(cancellationToken).ConfigureAwait(false);
                if ((valueFlag & 1) == 1)
                {
                    return null;
                }
            }

            return await ReadNonNullableValueAsync(stream, reader, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        ///     Reads a non-nullable value from the stream.
        /// </summary>
        /// <param name="stream">The GraphBinary data to parse.</param>
        /// <param name="reader">A <see cref="GraphBinaryReader"/> that can be used to read nested values.</param>
        /// <param name="cancellationToken">The token to cancel the operation. The default value is None.</param>
        /// <returns>The read value.</returns>
        protected abstract Task<object> ReadNonNullableValueAsync(Stream stream, GraphBinaryReader reader,
            CancellationToken cancellationToken = default);

        /// <inheritdoc />
        public override string TypeName => _type.TypeName;
    }
}