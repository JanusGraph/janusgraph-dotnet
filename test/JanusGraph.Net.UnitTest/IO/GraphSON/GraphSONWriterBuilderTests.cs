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

using System.Collections.Generic;
using System.Text.Json;
using Gremlin.Net.Structure.IO.GraphSON;
using JanusGraph.Net.Geoshapes;
using JanusGraph.Net.IO.GraphSON;
using Xunit;

namespace JanusGraph.Net.UnitTest.IO.GraphSON
{
    public class GraphSONWriterBuilderTests
    {
        [Theory]
        [InlineData(1, "test:Type")]
        [InlineData("string", "test:OtherType")]
        public void RegisterSerializer_CustomSerializerForGivenType_SerializerRegistered(object dataToSerialize,
            string graphSonTypeToUse)
        {
            var customSerializer = SerializerFake.Register(graphSonTypeToUse, dataToSerialize);

            var writer = JanusGraphSONWriterBuilder.Build()
                .RegisterSerializer(dataToSerialize.GetType(), customSerializer).Create();

            Assert.Equal(customSerializer.DeserializationResult, writer.WriteObject(dataToSerialize));
        }

        [Fact]
        public void RegisterSerializer_CustomSerializerForKnownJanusGraphType_SerializerRegistered()
        {
            var customSerializer = SerializerFake.Register("janusgraph:Geoshape", Geoshape.Point(1, 2));

            var writer = JanusGraphSONWriterBuilder.Build().RegisterSerializer(typeof(Point), customSerializer)
                .Create();

            Assert.Equal(customSerializer.DeserializationResult, writer.WriteObject(Geoshape.Point(1, 2)));
        }

        private class SerializerFake : IGraphSONSerializer
        {
            public string DeserializationResult => JsonSerializer.Serialize(_typedValue);
            private readonly Dictionary<string, dynamic> _typedValue;

            private SerializerFake(string graphSonType, object valueToReturn)
            {
                _typedValue = new Dictionary<string, dynamic> { { graphSonType, valueToReturn } };
            }

            public static SerializerFake Register(string graphSonType, object valueToReturn)
            {
                return new(graphSonType, valueToReturn);
            }

            public Dictionary<string, dynamic> Dictify(dynamic objectData, GraphSONWriter writer)
            {
                return _typedValue;
            }
        }
    }
}