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
using System.Collections.Generic;
using Gremlin.Net.Driver;
using Gremlin.Net.Driver.Remote;
using Gremlin.Net.Process.Remote;
using JanusGraph.Net.IO.GraphSON;

namespace JanusGraph.Net.IntegrationTest
{
    public class RemoteConnectionFactory : IDisposable
    {
        private readonly IList<DriverRemoteConnection> _connections = new List<DriverRemoteConnection>();
        private readonly string _host;
        private readonly int _port;
        private readonly IMessageSerializer _serializer;

        public RemoteConnectionFactory(string host, int port)
            : this(host, port, new JanusGraphGraphSONMessageSerializer())
        {
        }

        public RemoteConnectionFactory(string host, int port, IMessageSerializer serializer)
        {
            _host = host;
            _port = port;
            _serializer = serializer;
        }

        public IRemoteConnection CreateRemoteConnection()
        {
            var client = new GremlinClient(new GremlinServer(_host, _port), _serializer);
            var c = new DriverRemoteConnection(client);
            _connections.Add(c);
            return c;
        }

        public void Dispose()
        {
            foreach (var connection in _connections) connection.Dispose();
        }
    }
}