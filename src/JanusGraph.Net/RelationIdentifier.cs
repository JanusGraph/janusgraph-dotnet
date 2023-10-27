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

            if (elements[1][0] == LongEncoding.StringEncodingMarker)
            {
                OutVertexId = elements[1].Substring(1);
            }
            else
            {
                OutVertexId = LongEncoding.Decode(elements[1]);
            }
            TypeId = LongEncoding.Decode(elements[2]);
            RelationId = LongEncoding.Decode(elements[0]);
            if (elements.Length == 4)
            {
                if (elements[3][0] == LongEncoding.StringEncodingMarker)
                {
                    InVertexId = elements[3].Substring(1);
                }
                else
                {
                    InVertexId = LongEncoding.Decode(elements[3]);
                }
            }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="RelationIdentifier" /> class.
        /// </summary>
        /// <param name="outVertexId">The id of the outgoing vertex.</param>
        /// <param name="typeId">The JanusGraph internal type id.</param>
        /// <param name="relationId">The JanusGraph internal relation id.</param>
        /// <param name="inVertexId">The id of the incoming vertex.</param>
        public RelationIdentifier(object outVertexId, long typeId, long relationId, object? inVertexId)
        {
            OutVertexId = outVertexId;
            TypeId = typeId;
            RelationId = relationId;
            InVertexId = inVertexId;

            var sb = new StringBuilder();
            sb.Append(LongEncoding.Encode(relationId)).Append(ToStringDelimiter);
            if (outVertexId is long outVLongId)
            {
                sb.Append(LongEncoding.Encode(outVLongId));
            }
            else
            {
                sb.Append(LongEncoding.StringEncodingMarker).Append(outVertexId);
            }

            sb.Append(ToStringDelimiter).Append(LongEncoding.Encode(typeId));

            if (inVertexId != null)
            {
                sb.Append(ToStringDelimiter);
                if (inVertexId is long inVLongId)
                {
                    sb.Append(LongEncoding.Encode(inVLongId));
                }
                else
                {
                    sb.Append(LongEncoding.StringEncodingMarker).Append(inVertexId);
                }
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
        public object OutVertexId { get; }

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
        public object? InVertexId { get; }

        /// <inheritdoc />
        public bool Equals(RelationIdentifier? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(StringRepresentation, other.StringRepresentation);
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((RelationIdentifier)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return StringRepresentation.GetHashCode();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return StringRepresentation;
        }
    }
}