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

using Gremlin.Net.Structure.IO.GraphSON;
using JanusGraph.Net.IO.GraphSON;
using Newtonsoft.Json.Linq;
using Xunit;

namespace JanusGraph.Net.UnitTest.IO.GraphSON
{
    public class GraphSONReaderBuilderTests
    {
        [Theory]
        [InlineData("test:Type", 1)]
        [InlineData("janusgraph:Geoshape", 2)]
        [InlineData("g:Int32", "result")]
        public void RegisterDeserializer_CustomDeserializerForGivenType_DeserializerRegistered(string graphSONType,
            object deserializationResult)
        {
            var customDeserializer = new DeserializerFake {DeserializationResult = deserializationResult};
            var graphSon = "{\"@type\":\"" + graphSONType + "\",\"@value\":0}";

            var reader = GraphSONReaderBuilder.Build().RegisterDeserializer(graphSONType, customDeserializer).Create();

            Assert.Equal(deserializationResult, reader.ToObject(JToken.Parse(graphSon)));
        }

        private class DeserializerFake : IGraphSONDeserializer
        {
            public object DeserializationResult { get; set; }

            public dynamic Objectify(JToken graphsonObject, GraphSONReader reader)
            {
                return DeserializationResult;
            }
        }
    }
}