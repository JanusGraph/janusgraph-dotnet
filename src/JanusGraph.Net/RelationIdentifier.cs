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
using System.Text;
using JanusGraph.Net.Utils;

namespace JanusGraph.Net
{
    /// <summary>
    ///     Identifies an edge.
    /// </summary>
    public class RelationIdentifier : IEquatable<RelationIdentifier>
    {
        private const char ToStringDelimiter = '-';

        /// <summary>
        ///     Initializes a new instance of the <see cref="RelationIdentifier" /> class.
        /// </summary>
        /// <param name="stringRepresentation">The underlying relation id.</param>
        public RelationIdentifier(string stringRepresentation)
        {
            StringRepresentation = stringRepresentation;

            var elements = stringRepresentation.Split(ToStringDelimiter);
            if (elements.Length != 3 && elements.Length != 4)
                throw new ArgumentException($"Not a valid relation identifier: {stringRepresentation}");
            OutVertexId = LongEncoding.Decode(elements[1]);
            TypeId = LongEncoding.Decode(elements[2]);
            RelationId = LongEncoding.Decode(elements[0]);
            if (elements.Length == 4)
            {
                InVertexId = LongEncoding.Decode(elements[3]);
            }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="RelationIdentifier" /> class.
        /// </summary>
        /// <param name="outVertexId"></param>
        /// <param name="typeId"></param>
        /// <param name="relationId"></param>
        /// <param name="inVertexId"></param>
        public RelationIdentifier(long outVertexId, long typeId, long relationId, long inVertexId)
        {
            OutVertexId = outVertexId;
            TypeId = typeId;
            RelationId = relationId;
            InVertexId = inVertexId;

            var sb = new StringBuilder();
            sb.Append(LongEncoding.Encode(relationId)).Append(ToStringDelimiter)
                .Append(LongEncoding.Encode(outVertexId)).Append(ToStringDelimiter).Append(LongEncoding.Encode(typeId));
            if (inVertexId != 0)
            {
                sb.Append(ToStringDelimiter).Append(LongEncoding.Encode(inVertexId));
            }

            StringRepresentation = sb.ToString();
        }

        /// <summary>
        ///     Gets the string representation of this <see cref="RelationIdentifier"/>.
        /// </summary>
        public string StringRepresentation { get; }

        /// <summary>
        ///     Gets the id of the outgoing vertex.
        /// </summary>
        public long OutVertexId { get; }

        /// <summary>
        ///     Gets the JanusGraph internal type id.
        /// </summary>
        public long TypeId { get; }

        /// <summary>
        ///     Gets the JanusGraph internal relation id.
        /// </summary>
        public long RelationId { get; }

        /// <summary>
        ///     Gets the id of the incoming vertex.
        /// </summary>
        public long InVertexId { get; }

        /// <inheritdoc />
        public bool Equals(RelationIdentifier other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(StringRepresentation, other.StringRepresentation);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((RelationIdentifier)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return StringRepresentation != null ? StringRepresentation.GetHashCode() : 0;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return StringRepresentation;
        }
    }
}