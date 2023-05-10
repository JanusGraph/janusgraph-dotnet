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

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Gremlin.Net.Structure.IO.GraphBinary;
using JanusGraph.Net.Geoshapes;

namespace JanusGraph.Net.IO.GraphBinary.Types
{
    internal class GeoshapeSerializer : JanusGraphTypeSerializer
    {
        private readonly Dictionary<Type, GeoshapeTypeSerializer> _serializerByType =
            new Dictionary<Type, GeoshapeTypeSerializer>
            {
                { typeof(Point), new PointSerializer() }
            };

        private readonly Dictionary<int, GeoshapeTypeSerializer> _serializerByGeoshapeTypeCode =
            new Dictionary<int, GeoshapeTypeSerializer>
            {
                { GeoshapeConstants.GeoshapePointTypeCode, new PointSerializer() }
            };

        public GeoshapeSerializer() : base(GraphBinaryType.Geoshape)
        {
        }

        protected override async Task WriteNonNullableValueAsync(object value, Stream stream, GraphBinaryWriter writer,
            CancellationToken cancellationToken = default)
        {
            await stream.WriteByteAsync(GeoshapeConstants.GeoshapeFormatVersion, cancellationToken)
                .ConfigureAwait(false);

            if (!_serializerByType.TryGetValue(value.GetType(), out var serializer))
            {
                throw new IOException($"Geoshape type {value.GetType()} not supported");
            }

            await serializer.WriteNonNullableValueAsync(value, stream, writer, cancellationToken).ConfigureAwait(false);
        }

        protected override async Task<object> ReadNonNullableValueAsync(Stream stream, GraphBinaryReader reader,
            CancellationToken cancellationToken = default)
        {
            var formatVersion = await stream.ReadByteAsync(cancellationToken).ConfigureAwait(false);
            if (formatVersion != GeoshapeConstants.GeoshapeFormatVersion)
            {
                throw new IOException($"Geoshape format {formatVersion} not supported");
            }

            var geoshapeTypeCode = await stream.ReadIntAsync(cancellationToken).ConfigureAwait(false);
            if (!_serializerByGeoshapeTypeCode.TryGetValue(geoshapeTypeCode, out var serializer))
            {
                throw new IOException($"Geoshape type code {geoshapeTypeCode} not supported");
            }

            return await serializer.ReadNonNullableGeoshapeValueAsync(stream, reader, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}