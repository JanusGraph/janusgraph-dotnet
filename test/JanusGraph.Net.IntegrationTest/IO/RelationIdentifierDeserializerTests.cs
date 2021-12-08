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

using System;
using System.Threading.Tasks;
using Xunit;
using static Gremlin.Net.Process.Traversal.AnonymousTraversalSource;

namespace JanusGraph.Net.IntegrationTest.IO
{
    [Collection("JanusGraph Server collection")]
    public abstract class RelationIdentifierDeserializerTests : IDisposable
    {
        protected abstract RemoteConnectionFactory ConnectionFactory { get; }

        [Fact]
        public async Task TraversalWithEdgeId_RelationIdentifierReturned_ValidRelationIdentifier()
        {
            var g = Traversal().WithRemote(ConnectionFactory.CreateRemoteConnection());

            var relationIdentifier =
                await g.V().Has("demigod", "name", "hercules").OutE("father").Id().Promise(t => t.Next());

            Assert.IsType<RelationIdentifier>(relationIdentifier);
        }

        [Fact]
        public async Task TraversalWithEdge_EdgeReturned_EdgeWithIdOfTypeRelationIdentifier()
        {
            var g = Traversal().WithRemote(ConnectionFactory.CreateRemoteConnection());

            var edge = await g.V().Has("demigod", "name", "hercules").OutE("father").Promise(t => t.Next());

            Assert.IsType<RelationIdentifier>(edge.Id);
        }

        public void Dispose()
        {
            ConnectionFactory?.Dispose();
        }
    }
}