#region License

/*
 * Copyright 2023 JanusGraph.Net Authors
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
    internal abstract class GeoshapeTypeSerializer
    {
        private readonly int _geoshapeTypeCode;

        protected GeoshapeTypeSerializer(int geoshapeTypeCode)
        {
            _geoshapeTypeCode = geoshapeTypeCode;
        }

        public async Task WriteNonNullableValueAsync(object geoshape, Stream stream,
            GraphBinaryWriter writer, CancellationToken cancellationToken = default)
        {
            await stream.WriteIntAsync(_geoshapeTypeCode, cancellationToken)
                .ConfigureAwait(false);

            await WriteNonNullableGeoshapeValueAsync(geoshape, stream, writer, cancellationToken).ConfigureAwait(false);
        }

        protected abstract Task WriteNonNullableGeoshapeValueAsync(object geoshape, Stream stream,
            GraphBinaryWriter writer, CancellationToken cancellationToken);

        public abstract Task<object> ReadNonNullableGeoshapeValueAsync(Stream stream, GraphBinaryReader reader,
            CancellationToken cancellationToken);
    }
}