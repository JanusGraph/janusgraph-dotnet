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
using System.Text;

namespace JanusGraph.Net.Utils
{
    /// <summary>
    ///     Utility class for encoding longs in strings, re-implemented from its Java equivalent.
    /// </summary>
    /// <remarks>
    ///     JanusGraph encodes long IDs in the <see cref="RelationIdentifier"/> as strings.
    /// </remarks>
    public static class LongEncoding
    {
        /// <summary>
        ///     The symbols used for the encoding.
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        public static readonly string BaseSymbols = "0123456789abcdefghijklmnopqrstuvwxyz";
        private static readonly int NrSymbols = BaseSymbols.Length;

        /// <summary>
        /// Encoding used to indicate that an id is a string.
        /// </summary>
        public static readonly char StringEncodingMarker = 'S';

        /// <summary>
        ///     Decodes a string back into a long.
        /// </summary>
        /// <param name="s">The string to decode.</param>
        /// <returns>The decoded long value.</returns>
        /// <exception cref="ArgumentException">
        ///     Thrown if the string contains any invalid characters. Only <see cref="BaseSymbols"/> are allowed.
        /// </exception>
        public static long Decode(string s)
        {
            long num = 0;
            foreach (var ch in s)
            {
                num *= NrSymbols;
                var pos = BaseSymbols.IndexOf(ch);
                if (pos < 0)
                    throw new ArgumentException($"Symbol {ch} not allowed, only these symbols are: {BaseSymbols}");
                num += pos;
            }

            return num;
        }

        /// <summary>
        ///     Encodes a long value as a string.
        /// </summary>
        /// <param name="num">The long value to encode.</param>
        /// <returns>The encoded string value.</returns>
        public static string Encode(long num)
        {
            var sb = new StringBuilder();
            while (num != 0)
            {
                sb.Append(BaseSymbols[(int)(num % NrSymbols)]);
                num /= NrSymbols;
            }

            var chars = sb.ToString().ToCharArray();
            Array.Reverse(chars);
            return new string(chars);
        }
    }
}