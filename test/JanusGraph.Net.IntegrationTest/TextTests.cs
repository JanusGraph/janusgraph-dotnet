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
        public async Task TextContainsGivenSearchText_ExpectedCountOfElements(string searchText, int expectedCount)
        {
            var g = Traversal().WithRemote(ConnectionFactory.CreateRemoteConnection());

            var count = await g.E().Has("reason", Text.TextContains(searchText)).Count().Promise(t => t.Next());

            Assert.Equal(expectedCount, count);
        }

        [Theory]
        [InlineData("loves", 1)]
        [InlineData("shouldNotBeFound", 3)]
        public async Task TextNotContainsGivenSearchText_ExpectedCountOfElements(string searchText, int expectedCount)
        {
            var g = Traversal().WithRemote(ConnectionFactory.CreateRemoteConnection());

            var count = await g.E().Has("reason", Text.TextNotContains(searchText)).Count().Promise(t => t.Next());

            Assert.Equal(expectedCount, count);
        }

        [Theory]
        [InlineData("wave", 1)]
        [InlineData("f", 2)]
        [InlineData("shouldNotBeFound", 0)]
        public async Task TextContainsPrefixGivenSearchText_ExpectedCountOfElements(string searchText,
            int expectedCount)
        {
            var g = Traversal().WithRemote(ConnectionFactory.CreateRemoteConnection());

            var count = await g.E().Has("reason", Text.TextContainsPrefix(searchText)).Count().Promise(t => t.Next());

            Assert.Equal(expectedCount, count);
        }

        [Theory]
        [InlineData("wave", 2)]
        [InlineData("f", 1)]
        [InlineData("shouldNotBeFound", 3)]
        public async Task TextNotContainsPrefixGivenSearchText_ExpectedCountOfElements(string searchText,
            int expectedCount)
        {
            var g = Traversal().WithRemote(ConnectionFactory.CreateRemoteConnection());

            var count = await g.E().Has("reason", Text.TextNotContainsPrefix(searchText)).Count()
                .Promise(t => t.Next());

            Assert.Equal(expectedCount, count);
        }

        [Theory]
        [InlineData(".*ave.*", 1)]
        [InlineData("f.{3,4}", 2)]
        [InlineData("shouldNotBeFound", 0)]
        public async Task TextContainsRegexGivenRegex_ExpectedCountOfElements(string regex, int expectedCount)
        {
            var g = Traversal().WithRemote(ConnectionFactory.CreateRemoteConnection());

            var count = await g.E().Has("reason", Text.TextContainsRegex(regex)).Count().Promise(t => t.Next());

            Assert.Equal(expectedCount, count);
        }

        [Theory]
        [InlineData(".*ave.*", 2)]
        [InlineData("f.{3,4}", 1)]
        [InlineData("shouldNotBeFound", 3)]
        public async Task TextNotContainsRegexGivenRegex_ExpectedCountOfElements(string regex, int expectedCount)
        {
            var g = Traversal().WithRemote(ConnectionFactory.CreateRemoteConnection());

            var count = await g.E().Has("reason", Text.TextNotContainsRegex(regex)).Count().Promise(t => t.Next());

            Assert.Equal(expectedCount, count);
        }

        [Theory]
        [InlineData("waxes", 1)]
        [InlineData("shouldNotBeFound", 0)]
        public async Task TextContainsFuzzyGivenSearchText_ExpectedCountOfElements(string searchText, int expectedCount)
        {
            var g = Traversal().WithRemote(ConnectionFactory.CreateRemoteConnection());

            var count = await g.E().Has("reason", Text.TextContainsFuzzy(searchText)).Count().Promise(t => t.Next());

            Assert.Equal(expectedCount, count);
        }

        [Theory]
        [InlineData("waxes", 2)]
        [InlineData("shouldNotBeFound", 3)]
        public async Task TextNotContainsFuzzyGivenSearchText_ExpectedCountOfElements(string searchText,
            int expectedCount)
        {
            var g = Traversal().WithRemote(ConnectionFactory.CreateRemoteConnection());

            var count = await g.E().Has("reason", Text.TextNotContainsFuzzy(searchText)).Count().Promise(t => t.Next());

            Assert.Equal(expectedCount, count);
        }

        [Theory]
        [InlineData("fresh breezes", 1)]
        [InlineData("no fear", 1)]
        [InlineData("fear of", 1)]
        [InlineData("shouldNotBeFound", 0)]
        public async Task TextContainsPhraseGivenSearchText_ExpectedCountOfElements(string searchText,
            int expectedCount)
        {
            var g = Traversal().WithRemote(ConnectionFactory.CreateRemoteConnection());

            var count = await g.E().Has("reason", Text.TextContainsPhrase(searchText)).Count().Promise(t => t.Next());

            Assert.Equal(expectedCount, count);
        }

        [Theory]
        [InlineData("fresh breezes", 2)]
        [InlineData("no fear", 2)]
        [InlineData("fear of", 2)]
        [InlineData("shouldNotBeFound", 3)]
        public async Task TextNotContainsPhraseGivenSearchText_ExpectedCountOfElements(string searchText,
            int expectedCount)
        {
            var g = Traversal().WithRemote(ConnectionFactory.CreateRemoteConnection());

            var count = await g.E().Has("reason", Text.TextNotContainsPhrase(searchText)).Count()
                .Promise(t => t.Next());

            Assert.Equal(expectedCount, count);
        }

        [Theory]
        [InlineData("herc", 1)]
        [InlineData("s", 3)]
        [InlineData("shouldNotBeFound", 0)]
        public async Task TextPrefixGivenSearchText_ExpectedCountOfElements(string searchText, int expectedCount)
        {
            var g = Traversal().WithRemote(ConnectionFactory.CreateRemoteConnection());

            var count = await g.V().Has("name", Text.TextPrefix(searchText)).Count().Promise(t => t.Next());

            Assert.Equal(expectedCount, count);
        }

        [Theory]
        [InlineData("herc", 11)]
        [InlineData("s", 9)]
        [InlineData("shouldNotBeFound", 12)]
        public async Task TextNotPrefixGivenSearchText_ExpectedCountOfElements(string searchText, int expectedCount)
        {
            var g = Traversal().WithRemote(ConnectionFactory.CreateRemoteConnection());

            var count = await g.V().Has("name", Text.TextNotPrefix(searchText)).Count().Promise(t => t.Next());

            Assert.Equal(expectedCount, count);
        }

        [Theory]
        [InlineData(".*rcule.*", 1)]
        [InlineData("s.{2}", 2)]
        [InlineData("shouldNotBeFound", 0)]
        public async Task TextRegexGivenRegex_ExpectedCountOfElements(string regex, int expectedCount)
        {
            var g = Traversal().WithRemote(ConnectionFactory.CreateRemoteConnection());

            var count = await g.V().Has("name", Text.TextRegex(regex)).Count().Promise(t => t.Next());

            Assert.Equal(expectedCount, count);
        }

        [Theory]
        [InlineData(".*rcule.*", 11)]
        [InlineData("s.{2}", 10)]
        [InlineData("shouldNotBeFound", 12)]
        public async Task TextNotRegexGivenRegex_ExpectedCountOfElements(string regex, int expectedCount)
        {
            var g = Traversal().WithRemote(ConnectionFactory.CreateRemoteConnection());

            var count = await g.V().Has("name", Text.TextNotRegex(regex)).Count().Promise(t => t.Next());

            Assert.Equal(expectedCount, count);
        }

        [Theory]
        [InlineData("herculex", 1)]
        [InlineData("ska", 2)]
        [InlineData("shouldNotBeFound", 0)]
        public async Task TextFuzzyGivenSearchText_ExpectedCountOfElements(string searchText, int expectedCount)
        {
            var g = Traversal().WithRemote(ConnectionFactory.CreateRemoteConnection());

            var count = await g.V().Has("name", Text.TextFuzzy(searchText)).Count().Promise(t => t.Next());

            Assert.Equal(expectedCount, count);
        }

        [Theory]
        [InlineData("herculex", 11)]
        [InlineData("ska", 10)]
        [InlineData("shouldNotBeFound", 12)]
        public async Task TextNotFuzzyGivenSearchText_ExpectedCountOfElements(string searchText, int expectedCount)
        {
            var g = Traversal().WithRemote(ConnectionFactory.CreateRemoteConnection());

            var count = await g.V().Has("name", Text.TextNotFuzzy(searchText)).Count().Promise(t => t.Next());

            Assert.Equal(expectedCount, count);
        }

        public void Dispose()
        {
            ConnectionFactory?.Dispose();
        }
    }
}