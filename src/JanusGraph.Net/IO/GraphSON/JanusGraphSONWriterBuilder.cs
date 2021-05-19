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
using System.Collections.Generic;
using Gremlin.Net.Structure.IO.GraphSON;
using JanusGraph.Net.Geoshapes;

namespace JanusGraph.Net.IO.GraphSON
{
    /// <summary>
    ///     Creates a <see cref="GraphSONWriter" /> with the default JanusGraph serializers and allows adding of custom
    ///     serializers.
    /// </summary>
    public class JanusGraphSONWriterBuilder
    {
        private readonly Dictionary<Type, IGraphSONSerializer> _serializerByType =
            new Dictionary<Type, IGraphSONSerializer>
            {
                {typeof(Point), new PointSerializer()},
                {typeof(RelationIdentifier), new RelationIdentifierSerializer()}
            };

        private JanusGraphSONWriterBuilder()
        {
        }

        /// <summary>
        ///     Initializes a <see cref="JanusGraphSONWriterBuilder" />.
        /// </summary>
        public static JanusGraphSONWriterBuilder Build()
        {
            return new JanusGraphSONWriterBuilder();
        }

        /// <summary>
        ///     Registers a custom GraphSON serializer for the given type.
        /// </summary>
        /// <param name="type">The type the serializer should be registered for.</param>
        /// <param name="serializer">The serializer to register.</param>
        public JanusGraphSONWriterBuilder RegisterSerializer(Type type, IGraphSONSerializer serializer)
        {
            _serializerByType[type] = serializer;
            return this;
        }

        /// <summary>
        ///     Creates a <see cref="GraphSONWriter" /> with the registered serializers as well as the default JanusGraph
        ///     serializers.
        /// </summary>
        public GraphSON3Writer Create()
        {
            return new GraphSON3Writer(_serializerByType);
        }
    }
}