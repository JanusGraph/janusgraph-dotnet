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

namespace JanusGraph.Net.IO.GraphBinary.Types
{
    /// <summary>
    ///     Represents a JanusGraph GraphBinary type with its type information needed for serialization.
    /// </summary>
    public class GraphBinaryType
    {
        internal static readonly GraphBinaryType Geoshape =
            new GraphBinaryType(0x1000, "janusgraph.Geoshape");

        internal static readonly GraphBinaryType RelationIdentifier =
            new GraphBinaryType(0x1001, "janusgraph.RelationIdentifier");

        internal static readonly GraphBinaryType JanusGraphP =
            new GraphBinaryType(0x1002, "janusgraph.P");

        /// <summary>
        ///     Initializes a new instance of the <see cref="GraphBinaryType" /> class.
        /// </summary>
        /// <param name="typeId">The JanusGraph internal id of the type.</param>
        /// <param name="typeName">The name of the type.</param>
        public GraphBinaryType(int typeId, string typeName)
        {
            TypeId = typeId;
            TypeName = typeName;
        }

        /// <summary>
        ///     Gets the JanusGraph internal id of the type.
        /// </summary>
        public int TypeId { get; }

        /// <summary>
        ///     Gets the name of the type.
        /// </summary>
        public string TypeName { get; }
    }
}