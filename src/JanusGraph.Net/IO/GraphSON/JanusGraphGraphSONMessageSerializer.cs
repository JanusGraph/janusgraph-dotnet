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

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Gremlin.Net.Driver;
using Gremlin.Net.Driver.Messages;
using Gremlin.Net.Structure.IO.GraphSON;

namespace JanusGraph.Net.IO.GraphSON
{
    /// <summary>
    ///     Serializes data to and from JanusGraph Server in GraphSON3 format.
    /// </summary>
    public class JanusGraphGraphSONMessageSerializer : IMessageSerializer
    {
        private readonly GraphSON3MessageSerializer _serializer;

        /// <summary>
        ///     Initializes a new instance of the <see cref="JanusGraphGraphSONMessageSerializer" /> class.
        /// </summary>
        /// <param name="graphSONReader">The <see cref="GraphSON3Reader"/> used to deserialize from GraphSON.</param>
        /// <param name="graphSONWriter">The <see cref="GraphSON3Writer"/> used to serialize to GraphSON.</param>
        public JanusGraphGraphSONMessageSerializer(GraphSON3Reader graphSONReader, GraphSON3Writer graphSONWriter)
        {
            _serializer = new GraphSON3MessageSerializer(graphSONReader, graphSONWriter);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="JanusGraphGraphSONMessageSerializer" /> class.
        /// </summary>
        public JanusGraphGraphSONMessageSerializer()
            : this(JanusGraphSONReaderBuilder.Build().Create(), JanusGraphSONWriterBuilder.Build().Create())
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="JanusGraphGraphSONMessageSerializer" /> class.
        /// </summary>
        /// <param name="janusGraphPredicates">
        ///     This value activates support for JanusGraph predicate serialization added in
        ///     JanusGraph 0.6.0. It should be set to true for JanusGraph Server versions >= 0.6.0 and to false for versions before
        ///     0.6.0.
        /// </param>
        public JanusGraphGraphSONMessageSerializer(bool janusGraphPredicates)
            : this(JanusGraphSONReaderBuilder.Build().Create(),
                JanusGraphSONWriterBuilder.Build(janusGraphPredicates).Create())
        {
        }

        /// <inheritdoc />
        public async Task<byte[]> SerializeMessageAsync(RequestMessage requestMessage,
            CancellationToken cancellationToken = default)
        {
            return await _serializer.SerializeMessageAsync(requestMessage, cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<ResponseMessage<List<object>>> DeserializeMessageAsync(byte[] message,
            CancellationToken cancellationToken = default)
        {
            return await _serializer.DeserializeMessageAsync(message, cancellationToken).ConfigureAwait(false);
        }
    }
}