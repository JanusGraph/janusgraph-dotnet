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
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Gremlin.Net.Driver;
using Polly;
using TestContainers.Core.Containers;

namespace JanusGraph.Net.IntegrationTest
{
    public class GremlinServerContainer : Container
    {
        public string Host => GetDockerHostIpAddress();

        public int Port
        {
            get
            {
                var portBindings = PortBindings?.SingleOrDefault(p => p.ExposedPort == ExposedPorts.First());
                return portBindings?.PortBinding ?? ExposedPorts.First();
            }
        }

        protected override async Task WaitUntilContainerStarted()
        {
            await base.WaitUntilContainerStarted();

            using (var client = new GremlinClient(new GremlinServer(Host, Port)))
            {
                var result = await Policy.TimeoutAsync(TimeSpan.FromMinutes(2))
                    .WrapAsync(Policy.Handle<WebSocketException>()
                        .WaitAndRetryForeverAsync(iteration => TimeSpan.FromSeconds(2)))
                    .ExecuteAndCaptureAsync(() => client.SubmitAsync("1+1"));
                if (result.Outcome == OutcomeType.Failure)
                    throw new Exception(result.FinalException.Message);
            }
        }
    }
}