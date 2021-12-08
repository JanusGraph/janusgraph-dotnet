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
using System.Threading.Tasks;
using Gremlin.Net.Structure.IO.GraphBinary;
using Gremlin.Net.Structure.IO.GraphBinary.Types;

namespace JanusGraph.Net.IO.GraphBinary.Types
{
    /// <summary>
    ///     Base class for GraphBinary serializers of JanusGraph types.
    /// </summary>
    /// <typeparam name="T">The JanusGraph type to be serialized.</typeparam>
    public abstract class JanusGraphTypeSerializer<T> : CustomTypeSerializer
    {
        private readonly GraphBinaryType _type;

        /// <summary>
        ///     Initializes a new instance of the <see cref="JanusGraphTypeSerializer{T}" /> class.
        /// </summary>
        /// <param name="type">The GraphBinaryType for this serializer.</param>
        protected JanusGraphTypeSerializer(GraphBinaryType type)
        {
            _type = type;
        }

        /// <inheritdoc />
        public override async Task WriteAsync(object value, Stream stream, GraphBinaryWriter writer)
        {
            await stream.WriteIntAsync(_type.TypeId).ConfigureAwait(false);

            await WriteValueAsync(value, stream, writer, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public override async Task WriteValueAsync(object value, Stream stream, GraphBinaryWriter writer, bool nullable)
        {
            if (value == null)
            {
                if (!nullable)
                {
                    throw new IOException("Unexpected null value when nullable is false");
                }

                await writer.WriteValueFlagNullAsync(stream).ConfigureAwait(false);
                return;
            }

            if (nullable)
            {
                await writer.WriteValueFlagNoneAsync(stream).ConfigureAwait(false);
            }

            await WriteNonNullableValueAsync((T) value, stream, writer).ConfigureAwait(false);
        }

        /// <summary>
        ///     Writes a non-nullable value without including type information.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="writer">A <see cref="GraphBinaryWriter"/> that can be used to write nested values.</param>
        /// <returns>A task that represents the asynchronous write operation.</returns>
        protected abstract Task WriteNonNullableValueAsync(T value, Stream stream, GraphBinaryWriter writer);

        /// <inheritdoc />
        public override async Task<object> ReadAsync(Stream stream, GraphBinaryReader reader)
        {
            var customTypeInfo = await stream.ReadIntAsync().ConfigureAwait(false);
            if (customTypeInfo != _type.TypeId)
            {
                throw new IOException(
                    $"Custom type info {customTypeInfo} doesn't match expected type info {_type.TypeId}");
            }

            return await ReadValueAsync(stream, reader, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public override async Task<object> ReadValueAsync(Stream stream, GraphBinaryReader reader, bool nullable)
        {
            if (nullable)
            {
                var valueFlag = await stream.ReadByteAsync().ConfigureAwait(false);
                if ((valueFlag & 1) == 1)
                {
                    return null;
                }
            }

            return await ReadNonNullableValueAsync(stream, reader).ConfigureAwait(false);
        }

        /// <summary>
        ///     Reads a non-nullable value from the stream.
        /// </summary>
        /// <param name="stream">The GraphBinary data to parse.</param>
        /// <param name="reader">A <see cref="GraphBinaryReader"/> that can be used to read nested values.</param>
        /// <returns>The read value.</returns>
        protected abstract Task<T> ReadNonNullableValueAsync(Stream stream, GraphBinaryReader reader);

        /// <inheritdoc />
        public override string TypeName => _type.TypeName;
    }
}