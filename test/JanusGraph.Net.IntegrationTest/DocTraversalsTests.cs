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
using Gremlin.Net.Structure.IO.GraphBinary;
using JanusGraph.Net.Geoshapes;
using JanusGraph.Net.IO.GraphBinary;
using JanusGraph.Net.IO.GraphSON;
using Xunit;
using static Gremlin.Net.Process.Traversal.AnonymousTraversalSource;

namespace JanusGraph.Net.IntegrationTest
{
    [Collection("JanusGraph Server collection")]
    public class DocTraversalsTests : IDisposable
    {
        protected virtual RemoteConnectionFactory ConnectionFactory { get; }

        public DocTraversalsTests(JanusGraphServerFixture fixture)
        {
            ConnectionFactory = new RemoteConnectionFactory(fixture.Host, fixture.Port);
        }

        [Fact(Skip = "No server running under localhost")]
        public void CreateGremlinClientWithGraphSONTest()
        {
            var client = new GremlinClient(new GremlinServer("localhost", 8182),
                new JanusGraphGraphSONMessageSerializer());
        }

        [Fact(Skip = "No server running under localhost")]
        public void CreateGremlinClientWithGraphBinaryTest()
        {
            var client = new GremlinClient(new GremlinServer("localhost", 8182),
                new GraphBinaryMessageSerializer(JanusGraphTypeSerializerRegistry.Instance));
        }

        [Fact]
        public void GremlinNetGettingStartedTest()
        {
            var g = Traversal().WithRemote(ConnectionFactory.CreateRemoteConnection());

            var herculesAge = g.V().Has("name", "hercules").Values<int>("age").Next();

            Assert.Equal(30, herculesAge);
        }

        [Fact]
        public void ReceivingEdgesTest()
        {
            var g = Traversal().WithRemote(ConnectionFactory.CreateRemoteConnection());

            var edges = g.V().Has("name", "hercules").OutE("battled").ToList();

            Assert.Equal(3, edges.Count);
        }

        [Fact]
        public virtual void TextContainsPredicateTest()
        {
            var g = Traversal().WithRemote(ConnectionFactory.CreateRemoteConnection());

            var reasons = g.E().Has("reason", Text.TextContains("loves")).ToList();

            Assert.Equal(2, reasons.Count);
        }

        [Fact]
        public virtual void GeoTypesPointsReceivedTest()
        {
            var g = Traversal().WithRemote(ConnectionFactory.CreateRemoteConnection());

            var firstBattlePlace = g.V().Has("name", "hercules").OutE("battled").Order().By("time")
                .Values<Point>("place").Next();

            Assert.Equal(38.1f, firstBattlePlace.Latitude, 3);
            Assert.Equal(23.7f, firstBattlePlace.Longitude, 3);
        }

        [Fact]
        public virtual void GeoTypesPointAsArgumentTest()
        {
            var g = Traversal().WithRemote(ConnectionFactory.CreateRemoteConnection());

            g.V().Has("name", "hercules").OutE("battled").Has("place", Geoshape.Point(38.1f, 23.7f)).Next();
        }

        public void Dispose()
        {
            ConnectionFactory?.Dispose();
        }
    }
}