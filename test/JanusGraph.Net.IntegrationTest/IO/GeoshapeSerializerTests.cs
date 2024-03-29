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
using JanusGraph.Net.Geoshapes;
using Xunit;
using static Gremlin.Net.Process.Traversal.AnonymousTraversalSource;

namespace JanusGraph.Net.IntegrationTest.IO
{
    [Collection("JanusGraph Server collection")]
    public abstract class GeoshapeSerializerTests : IDisposable
    {
        protected abstract RemoteConnectionFactory ConnectionFactory { get; }

        [Fact]
        public async Task TraversalWithPointHasFilter_ExistingPoint_ElementFound()
        {
            var g = Traversal().WithRemote(ConnectionFactory.CreateRemoteConnection());

            var count = await g.V().Has("demigod", "name", "hercules").OutE("battled")
                .Has("place", Geoshape.Point(38.1f, 23.7f)).Count().Promise(t => t.Next());

            Assert.Equal(1, count);
        }

        public void Dispose()
        {
            ConnectionFactory?.Dispose();
        }
    }
}