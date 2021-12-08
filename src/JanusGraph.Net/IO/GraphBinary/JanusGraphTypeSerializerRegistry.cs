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
using Gremlin.Net.Structure.IO.GraphBinary.Types;
using JanusGraph.Net.IO.GraphBinary.Types;

namespace JanusGraph.Net.IO.GraphBinary
{
    /// <summary>
    ///     Provides GraphBinary serializers for different types, including JanusGraph specific types.
    /// </summary>
    public static class JanusGraphTypeSerializerRegistry
    {
        /// <summary>
        ///     Provides a default <see cref="TypeSerializerRegistry" /> instance with JanusGraph types already registered.
        /// </summary>
        public static readonly TypeSerializerRegistry Instance = Build().Create();

        private static Builder Build() => new Builder();

        /// <summary>
        ///     Builds a <see cref="TypeSerializerRegistry" /> with serializers for JanusGraph types already registered.
        /// </summary>
        public class Builder
        {
            private readonly TypeSerializerRegistry.Builder _builder = TypeSerializerRegistry.Build();

            internal Builder()
            {
                _builder.AddCustomType(typeof(RelationIdentifier), new RelationIdentifierSerializer());
                _builder.AddCustomType(typeof(JanusGraphP), new JanusGraphPSerializer());
            }

            /// <summary>
            ///     Adds a serializer for a custom type.
            /// </summary>
            /// <param name="type">The custom type supported by the serializer.</param>
            /// <param name="serializer">The serializer for the custom type.</param>
            public Builder AddCustomType(Type type, CustomTypeSerializer serializer)
            {
                _builder.AddCustomType(type, serializer);
                return this;
            }

            /// <summary>
            ///     Creates the <see cref="TypeSerializerRegistry"/>.
            /// </summary>
            public TypeSerializerRegistry Create() => _builder.Create();
        }
    }
}