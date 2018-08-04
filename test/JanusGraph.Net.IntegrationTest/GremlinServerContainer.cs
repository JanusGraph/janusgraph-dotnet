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