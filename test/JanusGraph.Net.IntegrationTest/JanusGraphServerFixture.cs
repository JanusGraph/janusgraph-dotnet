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
using System.Net.WebSockets;
using System.Threading.Tasks;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Gremlin.Net.Driver;
using Gremlin.Net.Driver.Remote;
using JanusGraph.Net.IO.GraphSON;
using Microsoft.Extensions.Configuration;
using Xunit;
using static Gremlin.Net.Process.Traversal.AnonymousTraversalSource;

namespace JanusGraph.Net.IntegrationTest
{
    public class JanusGraphServerFixture : IAsyncLifetime
    {
        private readonly IContainer _container;
        private const ushort JanusGraphServerPort = 8182;

        public JanusGraphServerFixture()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var dockerImage = config["dockerImage"];
            _container = new ContainerBuilder()
                .WithImage(dockerImage)
                .WithName("janusgraph")
                .WithPortBinding(JanusGraphServerPort, true)
                .WithBindMount($"{AppContext.BaseDirectory}/load_data.groovy",
                    "/docker-entrypoint-initdb.d/load_data.groovy", AccessMode.ReadOnly)
                .WithWaitStrategy(Wait.ForUnixContainer().UntilOperationIsSucceeded(IsServerReady, 1000))
                .Build();
        }

        private bool IsServerReady()
        {
            try
            {
                using var client = new GremlinClient(new GremlinServer(Host, Port),
                    new JanusGraphGraphSONMessageSerializer());
                var g = Traversal().WithRemote(new DriverRemoteConnection(client));
                return g.V().Has("name", "hercules").HasNext();
            }
            catch (AggregateException e) when (e.InnerException is WebSocketException)
            {
                return false;
            }
        }

        public string Host => _container.Hostname;
        public int Port => _container.GetMappedPublicPort(JanusGraphServerPort);

        public Task InitializeAsync()
        {
            return _container.StartAsync();
        }

        public Task DisposeAsync()
        {
            return _container.StopAsync();
        }
    }
}