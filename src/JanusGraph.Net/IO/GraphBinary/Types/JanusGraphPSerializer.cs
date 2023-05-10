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

namespace JanusGraph.Net.IO.GraphBinary.Types
{
    internal class JanusGraphPSerializer : JanusGraphTypeSerializer
    {
        public JanusGraphPSerializer() : base(GraphBinaryType.JanusGraphP)
        {
        }

        protected override async Task WriteNonNullableValueAsync(object value, Stream stream,
            GraphBinaryWriter writer, CancellationToken cancellationToken = default)
        {
            var p = (JanusGraphP)value;
            await writer.WriteValueAsync(p.OperatorName, stream, false, cancellationToken).ConfigureAwait(false);
            await writer.WriteAsync(p.Value, stream, cancellationToken).ConfigureAwait(false);
        }

        protected override async Task<object> ReadNonNullableValueAsync(Stream stream, GraphBinaryReader reader,
            CancellationToken cancellationToken = default)
        {
            var operatorName = (string)await reader.ReadValueAsync<string>(stream, false, cancellationToken)
                .ConfigureAwait(false);
            var value = await reader.ReadAsync(stream, cancellationToken).ConfigureAwait(false);
            return new JanusGraphP(operatorName, value);
        }
    }
}