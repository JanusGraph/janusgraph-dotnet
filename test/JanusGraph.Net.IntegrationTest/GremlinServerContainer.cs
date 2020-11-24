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
using Docker.DotNet;
using Gremlin.Net.Driver;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using TestContainers.Container.Abstractions;
using TestContainers.Container.Abstractions.Images;

namespace JanusGraph.Net.IntegrationTest
{
    public class GremlinServerContainer : GenericContainer
    {
        public static readonly int GremlinServerPort = 8182;

        public string Host => GetDockerHostIpAddress();

        public int Port => GetMappedPort(GremlinServerPort);

        public string ServerStartedCheckTraversal { get; set; } = "1+1==2";

        /// <inheritdoc />
        public GremlinServerContainer(string dockerImageName, IDockerClient dockerClient, ILoggerFactory loggerFactory)
            : base(dockerImageName, dockerClient, loggerFactory)
        {
        }

        /// <inheritdoc />
        [ActivatorUtilitiesConstructor]
        public GremlinServerContainer(IImage dockerImage, IDockerClient dockerClient, ILoggerFactory loggerFactory)
            : base(dockerImage, dockerClient, loggerFactory)
        {
        }

        protected override async Task ContainerStarted()
        {
            var result = await Policy.TimeoutAsync(TimeSpan.FromMinutes(2))
                .WrapAsync(Policy
                    .Handle<WebSocketException>()
                    .Or<InvalidOperationException>()
                    .WaitAndRetryForeverAsync(iteration => TimeSpan.FromSeconds(2)))
                .ExecuteAndCaptureAsync(async () =>
                {
                    var serverStarted = await IsServerStartedAsync();

                    if (!serverStarted)
                    {
                        throw new InvalidOperationException("Server not fully started yet");
                    }
                });

            if (result.Outcome == OutcomeType.Failure)
                throw new Exception(result.FinalException.Message);
        }

        private async Task<bool> IsServerStartedAsync()
        {
            using var client = new GremlinClient(new GremlinServer(Host, Port));
            return await client.SubmitWithSingleResultAsync<bool>(ServerStartedCheckTraversal);
        }
    }
}