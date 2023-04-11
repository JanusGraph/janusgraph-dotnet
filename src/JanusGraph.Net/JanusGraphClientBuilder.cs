#region License

/*
 * Copyright 2018 JanusGraph.Net Authors
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
using Gremlin.Net.Driver;
using Gremlin.Net.Structure.IO.GraphSON;
using JanusGraph.Net.IO.GraphSON;

namespace JanusGraph.Net
{
    /// <summary>
    ///     Allows creating of a <see cref="IGremlinClient" /> pre-configured for JanusGraph.
    /// </summary>
    [Obsolete("We recommend that users directly create a GremlinClient instance instead of using this builder " +
                "because it makes it easier to configure the GremlinClient. You just need to provide it with either " +
                "a JanusGraphGraphSONMessageSerializer for GraphSON 3 or like this for GraphBinary: " +
                "new GraphBinaryMessageSerializer(JanusGraphTypeSerializerRegistry.Instance).")]
    public class JanusGraphClientBuilder
    {
        private readonly GremlinServer _server;
        private readonly JanusGraphSONReaderBuilder _readerBuilder = JanusGraphSONReaderBuilder.Build();
        private readonly JanusGraphSONWriterBuilder _writerBuilder;
        private IMessageSerializer _serializer;
        private ConnectionPoolSettings _connectionPoolSettings;

        private JanusGraphClientBuilder(GremlinServer server)
        {
            _server = server;
            _writerBuilder = JanusGraphSONWriterBuilder.Build();
        }

        /// <summary>
        ///     Initializes a <see cref="JanusGraphClientBuilder" /> for the given <see cref="GremlinServer" />.
        /// </summary>
        /// <param name="server">The <see cref="GremlinServer" /> requests should be sent to.</param>
        public static JanusGraphClientBuilder BuildClientForServer(GremlinServer server)
        {
            return new JanusGraphClientBuilder(server);
        }

        /// <summary>
        ///     Registers a <see cref="IMessageSerializer"/> used to serialize data to and from JanusGraph Server.
        /// </summary>
        /// <param name="serializer">The serializer to use.</param>
        public JanusGraphClientBuilder WithSerializer(IMessageSerializer serializer)
        {
            _serializer = serializer;
            return this;
        }

        /// <summary>
        ///     Registers a custom GraphSON deserializer for the given GraphSON type.
        /// </summary>
        /// <param name="graphSONType">The GraphSON type the deserializer should be registered for.</param>
        /// <param name="deserializer">The deserializer to register.</param>
        [Obsolete("Use WithSerializer() instead")]
        public JanusGraphClientBuilder RegisterDeserializer(string graphSONType, IGraphSONDeserializer deserializer)
        {
            _readerBuilder.RegisterDeserializer(graphSONType, deserializer);
            return this;
        }

        /// <summary>
        ///     Registers a custom GraphSON serializer for the given type.
        /// </summary>
        /// <param name="type">The type the serializer should be registered for.</param>
        /// <param name="serializer">The serializer to register.</param>
        [Obsolete("Use WithSerializer() instead")]
        public JanusGraphClientBuilder RegisterSerializer(Type type, IGraphSONSerializer serializer)
        {
            _writerBuilder.RegisterSerializer(type, serializer);
            return this;
        }

        /// <summary>
        ///     Configures the <see cref="ConnectionPoolSettings"/> for the client.
        /// </summary>
        /// <param name="connectionPoolSettings">The connection pool settings to use.</param>
        public JanusGraphClientBuilder WithConnectionPoolSettings(ConnectionPoolSettings connectionPoolSettings)
        {
            _connectionPoolSettings = connectionPoolSettings;
            return this;
        }

        /// <summary>
        ///     Creates the <see cref="IGremlinClient" /> with the given settings and pre-configured for JanusGraph.
        /// </summary>
        public IGremlinClient Create()
        {
            var serializer = _serializer ??
                new JanusGraphGraphSONMessageSerializer(_readerBuilder.Create(), _writerBuilder.Create());
            return new GremlinClient(_server, serializer, _connectionPoolSettings);
        }
    }
}