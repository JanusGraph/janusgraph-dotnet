﻿#region License

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
using JanusGraph.Net.Geoshapes;
using Xunit;
using static Gremlin.Net.Process.Traversal.AnonymousTraversalSource;

namespace JanusGraph.Net.IntegrationTest
{
    [Collection("JanusGraph Server collection")]
    public class DocTraversalsTests : IDisposable
    {
        private readonly RemoteConnectionFactory _connectionFactory;

        public DocTraversalsTests(JanusGraphServerFixture fixture)
        {
            _connectionFactory = new RemoteConnectionFactory(fixture.Host, fixture.Port);
        }

        [Fact]
        public void GremlinNetGettingStartedTest()
        {
            var g = Traversal().WithRemote(_connectionFactory.CreateRemoteConnection());

            var herculesAge = g.V().Has("name", "hercules").Values<int>("age").Next();

            Assert.Equal(30, herculesAge);
        }

        [Fact]
        public void ReceivingEdgesTest()
        {
            var g = Traversal().WithRemote(_connectionFactory.CreateRemoteConnection());

            var edges = g.V().Has("name", "hercules").OutE("battled").ToList();

            Assert.Equal(3, edges.Count);
        }

        [Fact]
        public void TextContainsPredicateTest()
        {
            var g = Traversal().WithRemote(_connectionFactory.CreateRemoteConnection());

            var reasons = g.E().Has("reason", Text.TextContains("loves")).ToList();

            Assert.Equal(2, reasons.Count);
        }

        [Fact]
        public void GeoTypesPointsReceivedTest()
        {
            var g = Traversal().WithRemote(_connectionFactory.CreateRemoteConnection());

            var firstBattlePlace = g.V().Has("name", "hercules").OutE("battled").Order().By("time")
                .Values<Point>("place").Next();

            Assert.Equal(38.1f, firstBattlePlace.Latitude, 3);
            Assert.Equal(23.7f, firstBattlePlace.Longitude, 3);
        }

        [Fact]
        public void GeoTypesPointAsArgumentTest()
        {
            var g = Traversal().WithRemote(_connectionFactory.CreateRemoteConnection());

            g.V().Has("name", "hercules").OutE("battled").Has("place", Geoshape.Point(38.1f, 23.7f)).Next();
        }

        public void Dispose()
        {
            _connectionFactory?.Dispose();
        }
    }
}