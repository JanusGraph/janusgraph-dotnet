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
using System.Threading.Tasks;
using Xunit;
using static Gremlin.Net.Process.Traversal.AnonymousTraversalSource;

namespace JanusGraph.Net.IntegrationTest
{
    [Collection("JanusGraph Server collection")]
    public abstract class TextTests : IDisposable
    {
        protected abstract RemoteConnectionFactory ConnectionFactory { get; }

        [Theory]
        [InlineData("loves", 2)]
        [InlineData("shouldNotBeFound", 0)]
        public async Task TextContainsgivenSearchText_ExpectedCountOfElements(string searchText, int expectedCount)
        {
            var g = Traversal().WithRemote(ConnectionFactory.CreateRemoteConnection());

            var count = await g.E().Has("reason", Text.TextContains(searchText)).Count().Promise(t => t.Next());

            Assert.Equal(expectedCount, count);
        }

        [Theory]
        [InlineData("wave", 1)]
        [InlineData("f", 2)]
        [InlineData("shouldNotBeFound", 0)]
        public async Task TextContainsPrefixgivenSearchText_ExpectedCountOfElements(string searchText, int expectedCount)
        {
            var g = Traversal().WithRemote(ConnectionFactory.CreateRemoteConnection());

            var count = await g.E().Has("reason", Text.TextContainsPrefix(searchText)).Count().Promise(t => t.Next());

            Assert.Equal(expectedCount, count);
        }

        [Theory]
        [InlineData(".*ave.*", 1)]
        [InlineData("f.{3,4}", 2)]
        [InlineData("shouldNotBeFound", 0)]
        public async Task TextContainsRegexgivenRegex_ExpectedCountOfElements(string regex, int expectedCount)
        {
            var g = Traversal().WithRemote(ConnectionFactory.CreateRemoteConnection());

            var count = await g.E().Has("reason", Text.TextContainsRegex(regex)).Count().Promise(t => t.Next());

            Assert.Equal(expectedCount, count);
        }

        [Theory]
        [InlineData("waxes", 1)]
        [InlineData("shouldNotBeFound", 0)]
        public async Task TextContainsFuzzygivenSearchText_ExpectedCountOfElements(string searchText, int expectedCount)
        {
            var g = Traversal().WithRemote(ConnectionFactory.CreateRemoteConnection());

            var count = await g.E().Has("reason", Text.TextContainsFuzzy(searchText)).Count().Promise(t => t.Next());

            Assert.Equal(expectedCount, count);
        }

        [Theory]
        [InlineData("herc", 1)]
        [InlineData("s", 3)]
        [InlineData("shouldNotBeFound", 0)]
        public async Task TextPrefixgivenSearchText_ExpectedCountOfElements(string searchText, int expectedCount)
        {
            var g = Traversal().WithRemote(ConnectionFactory.CreateRemoteConnection());

            var count = await g.V().Has("name", Text.TextPrefix(searchText)).Count().Promise(t => t.Next());

            Assert.Equal(expectedCount, count);
        }

        [Theory]
        [InlineData(".*rcule.*", 1)]
        [InlineData("s.{2}", 2)]
        [InlineData("shouldNotBeFound", 0)]
        public async Task TextRegexgivenRegex_ExpectedCountOfElements(string regex, int expectedCount)
        {
            var g = Traversal().WithRemote(ConnectionFactory.CreateRemoteConnection());

            var count = await g.V().Has("name", Text.TextRegex(regex)).Count().Promise(t => t.Next());

            Assert.Equal(expectedCount, count);
        }

        [Theory]
        [InlineData("herculex", 1)]
        [InlineData("ska", 2)]
        [InlineData("shouldNotBeFound", 0)]
        public async Task TextFuzzygivenSearchText_ExpectedCountOfElements(string searchText, int expectedCount)
        {
            var g = Traversal().WithRemote(ConnectionFactory.CreateRemoteConnection());

            var count = await g.V().Has("name", Text.TextFuzzy(searchText)).Count().Promise(t => t.Next());

            Assert.Equal(expectedCount, count);
        }

        public void Dispose()
        {
            ConnectionFactory?.Dispose();
        }
    }
}