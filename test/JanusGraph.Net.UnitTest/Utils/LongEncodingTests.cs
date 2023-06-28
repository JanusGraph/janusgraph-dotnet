#region License

/*
 * Copyright 2023 JanusGraph.Net Authors
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

using JanusGraph.Net.Utils;
using Xunit;

namespace JanusGraph.Net.UnitTest.Utils;

public class LongEncodingTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(1234567890)]
    [InlineData(int.MaxValue)]
    [InlineData((long)int.MaxValue + 1)]
    [InlineData(long.MaxValue)]
    public void EncodeAndDecode_ValidValue_SameValue(long toEncode)
    {
        var encoded = LongEncoding.Encode(toEncode);
        var decoded = LongEncoding.Decode(encoded);

        Assert.Equal(toEncode, decoded);
    }
}