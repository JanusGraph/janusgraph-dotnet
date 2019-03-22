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
using System.Threading.Tasks;
using Gremlin.Net.Structure;
using JanusGraph.Net.Geoshapes;
using Xunit;

namespace JanusGraph.Net.IntegrationTest.IO.GraphSON
{
    [Collection("JanusGraph Server collection")]
    public class GeoshapeDeserializerTests : IDisposable
    {
        private readonly RemoteConnectionFactory _connectionFactory;

        public GeoshapeDeserializerTests(JanusGraphServerFixture fixture)
        {
            _connectionFactory = new RemoteConnectionFactory(fixture.Host, fixture.Port);
        }

        [Fact]
        public async Task TraversalWithPointPropertyValue_PointReturned_ExpectedPoint()
        {
            var g = new Graph().Traversal().WithRemote(_connectionFactory.CreateRemoteConnection());

            var place = await g.V().Has("demigod", "name", "hercules").OutE("battled").Has("time", 1)
                .Values<Point>("place").Promise(t => t.Next());

            var expectedPlace = Geoshape.Point(38.1f, 23.7f);
            Assert.Equal(expectedPlace.Latitude, place.Latitude, 3);
            Assert.Equal(expectedPlace.Longitude, place.Longitude, 3);
        }

        public void Dispose()
        {
            _connectionFactory?.Dispose();
        }
    }
}