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
using Gremlin.Net.Structure.IO.GraphBinary;
using JanusGraph.Net.IO.GraphBinary;
using Xunit;

namespace JanusGraph.Net.IntegrationTest
{
    [Collection("JanusGraph Server collection")]
    public class GraphBinaryDocTraversalsTests : DocTraversalsTests
    {
        protected override RemoteConnectionFactory ConnectionFactory { get; }

        public GraphBinaryDocTraversalsTests(JanusGraphServerFixture fixture) : base(fixture)
        {
            ConnectionFactory = new RemoteConnectionFactory(fixture.Host, fixture.Port,
                new GraphBinaryMessageSerializer(JanusGraphTypeSerializerRegistry.Instance));
        }

        [Fact(Skip = "Geoshapes not supported yet for GraphBinary")]
        public override void GeoTypesPointsReceivedTest()
        {
            throw new NotImplementedException("GraphBinary support for Geo types not implemented yet.");
        }

        [Fact(Skip = "Geoshapes not supported yet for GraphBinary")]
        public override void GeoTypesPointAsArgumentTest()
        {
            throw new NotImplementedException("GraphBinary support for Geo types not implemented yet.");
        }
    }
}