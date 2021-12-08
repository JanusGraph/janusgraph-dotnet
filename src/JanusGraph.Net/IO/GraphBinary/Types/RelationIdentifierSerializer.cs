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

namespace JanusGraph.Net.IO.GraphBinary.Types
{
    internal class RelationIdentifierSerializer : JanusGraphTypeSerializer<RelationIdentifier>
    {
        public RelationIdentifierSerializer() : base(GraphBinaryType.RelationIdentifier)
        {
        }

        protected override async Task WriteNonNullableValueAsync(RelationIdentifier value, Stream stream,
            GraphBinaryWriter writer)
        {
            await stream.WriteLongAsync(value.OutVertexId).ConfigureAwait(false);
            await stream.WriteLongAsync(value.TypeId).ConfigureAwait(false);
            await stream.WriteLongAsync(value.RelationId).ConfigureAwait(false);
            await stream.WriteLongAsync(value.InVertexId).ConfigureAwait(false);
        }

        protected override async Task<RelationIdentifier> ReadNonNullableValueAsync(Stream stream,
            GraphBinaryReader reader)
        {
            var outVertexId = await stream.ReadLongAsync().ConfigureAwait(false);
            var typeId = await stream.ReadLongAsync().ConfigureAwait(false);
            var relationId = await stream.ReadLongAsync().ConfigureAwait(false);
            var inVertexId = await stream.ReadLongAsync().ConfigureAwait(false);
            return new RelationIdentifier(outVertexId, typeId, relationId, inVertexId);
        }
    }
}