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
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Gremlin.Net.Structure.IO.GraphBinary;

namespace JanusGraph.Net.IO.GraphBinary.Types
{
    internal class RelationIdentifierSerializer : JanusGraphTypeSerializer
    {
        private static readonly byte LongMarker = 0;
        private static readonly byte StringMarker = 1;

        public RelationIdentifierSerializer() : base(GraphBinaryType.RelationIdentifier)
        {
        }

        protected override async Task WriteNonNullableValueInternalAsync(object value, Stream stream,
            GraphBinaryWriter writer, CancellationToken cancellationToken = default)
        {
            var relationIdentifier = (RelationIdentifier)value;

            if (relationIdentifier.OutVertexId is long outVLongId)
            {
                await stream.WriteByteAsync(LongMarker, cancellationToken).ConfigureAwait(false);
                await stream.WriteLongAsync(outVLongId, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                await stream.WriteByteAsync(StringMarker, cancellationToken).ConfigureAwait(false);
                await WriteStringAsync((string)relationIdentifier.OutVertexId, stream, cancellationToken)
                    .ConfigureAwait(false);
            }
            await stream.WriteLongAsync(relationIdentifier.TypeId, cancellationToken).ConfigureAwait(false);
            await stream.WriteLongAsync(relationIdentifier.RelationId, cancellationToken).ConfigureAwait(false);

            if (relationIdentifier.InVertexId == null)
            {
                // properties don't have in-vertex
                await stream.WriteByteAsync(LongMarker, cancellationToken).ConfigureAwait(false);
                await stream.WriteLongAsync(0, cancellationToken).ConfigureAwait(false);
            }
            else if (relationIdentifier.InVertexId is long inVLongId)
            {
                await stream.WriteByteAsync(LongMarker, cancellationToken).ConfigureAwait(false);
                await stream.WriteLongAsync(inVLongId, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                await stream.WriteByteAsync(StringMarker, cancellationToken).ConfigureAwait(false);
                await WriteStringAsync((string)relationIdentifier.InVertexId, stream, cancellationToken)
                    .ConfigureAwait(false);
            }
        }

        private static async Task WriteStringAsync(string value, Stream stream, CancellationToken cancellationToken)
        {
            var arr = new byte[value.Length];
            for (var i = 0; i < value.Length; i++)
            {
                int c = value[i];
                var b = (byte)c;
                if (i + 1 == value.Length)
                {
                    b |= 0x80; // end marker
                }

                arr[i] = b;
            }

            await stream.WriteAsync(arr, cancellationToken).ConfigureAwait(false);
        }

        public override async Task<object> ReadNonNullableValueAsync(Stream stream,
            GraphBinaryReader reader, CancellationToken cancellationToken = default)
        {
            var outVertexIdMarker = await stream.ReadByteAsync(cancellationToken).ConfigureAwait(false);
            object outVertexId;
            if (outVertexIdMarker == StringMarker)
            {
                outVertexId = await ReadStringAsync(stream, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                outVertexId = await stream.ReadLongAsync(cancellationToken).ConfigureAwait(false);
            }

            var typeId = await stream.ReadLongAsync(cancellationToken).ConfigureAwait(false);
            var relationId = await stream.ReadLongAsync(cancellationToken).ConfigureAwait(false);

            var inVertexIdMarker = await stream.ReadByteAsync(cancellationToken).ConfigureAwait(false);
            object inVertexId;
            if (inVertexIdMarker == StringMarker)
            {
                inVertexId = await ReadStringAsync(stream, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                inVertexId = await stream.ReadLongAsync(cancellationToken).ConfigureAwait(false);
                if (inVertexId.Equals(0L))
                {
                    inVertexId = null;
                }
            }
            return new RelationIdentifier(outVertexId, typeId, relationId, inVertexId);
        }

        private static async Task<string> ReadStringAsync(Stream stream, CancellationToken cancellationToken)
        {
            var sb = new StringBuilder();
            while (true)
            {
                var c = 0xFF & await stream.ReadByteAsync(cancellationToken).ConfigureAwait(false);
                sb.Append((char)(c & 0x7F));
                if ((c & 0x80) > 0) break;
            }

            return sb.ToString();
        }
    }
}