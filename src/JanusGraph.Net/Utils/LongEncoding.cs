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
    internal static class LongEncoding
    {
        private const string BaseSymbols = "0123456789abcdefghijklmnopqrstuvwxyz";
        private static readonly int NrSymbols = BaseSymbols.Length;

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

        public static string Encode(long num)
        {
            var sb = new StringBuilder();
            while (num != 0)
            {
                sb.Append(BaseSymbols[(int)num % NrSymbols]);
                num /= NrSymbols;
            }

            var chars = sb.ToString().ToCharArray();
            Array.Reverse(chars);
            return new string(chars);
        }
    }
}