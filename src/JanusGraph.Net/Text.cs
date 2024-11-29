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

using Gremlin.Net.Process.Traversal;

namespace JanusGraph.Net
{
    /// <summary>
    ///     Provides text search predicates.
    /// </summary>
    public static class Text
    {
        /// <summary>
        ///     Is true if (at least) one word inside the text string matches the query string.
        /// </summary>
        /// <param name="query">The query to search.</param>
        /// <returns>The text predicate.</returns>
        public static P TextContains(string query) => new JanusGraphP("textContains", query);

        /// <summary>
        ///     Is true if no words inside the text string match the query string.
        /// </summary>
        /// <param name="query">The query to search.</param>
        /// <returns>The text predicate.</returns>
        public static P TextNotContains(string query) => new JanusGraphP("textNotContains", query);

        /// <summary>
        ///     Is true if (at least) one word inside the text string begins with the query string.
        /// </summary>
        /// <param name="query">The query to search.</param>
        /// <returns>The text predicate.</returns>
        public static P TextContainsPrefix(string query) => new JanusGraphP("textContainsPrefix", query);

        /// <summary>
        ///     Is true if no words inside the text string begin with the query string.
        /// </summary>
        /// <param name="query">The query to search.</param>
        /// <returns>The text predicate.</returns>
        public static P TextNotContainsPrefix(string query) => new JanusGraphP("textNotContainsPrefix", query);

        /// <summary>
        ///     Is true if (at least) one word inside the text string matches the given regular expression.
        /// </summary>
        /// <param name="regex">The regular expression.</param>
        /// <returns>The text predicate.</returns>
        public static P TextContainsRegex(string regex) => new JanusGraphP("textContainsRegex", regex);

        /// <summary>
        ///     Is true if no words inside the text string match the given regular expression.
        /// </summary>
        /// <param name="regex">The regular expression.</param>
        /// <returns>The text predicate.</returns>
        public static P TextNotContainsRegex(string regex) => new JanusGraphP("textNotContainsRegex", regex);

        /// <summary>
        ///     Is true if (at least) one word inside the text string is similar to the query String (based on
        ///     Levenshtein edit distance).
        /// </summary>
        /// <param name="query">The query to search.</param>
        /// <returns>The text predicate.</returns>
        public static P TextContainsFuzzy(string query) => new JanusGraphP("textContainsFuzzy", query);

        /// <summary>
        ///     Is true if no words inside the text string are similar to the query string (based on Levenshtein edit
        ///     distance).
        /// </summary>
        /// <param name="query">The query to search.</param>
        /// <returns>The text predicate.</returns>
        public static P TextNotContainsFuzzy(string query) => new JanusGraphP("textNotContainsFuzzy", query);

        /// <summary>
        ///     Is true if the text string does contain the sequence of words in the query string.
        /// </summary>
        /// <param name="query">The query to search.</param>
        /// <returns>The text predicate.</returns>
        public static P TextContainsPhrase(string query) => new JanusGraphP("textContainsPhrase", query);

        /// <summary>
        ///     Is true if the text string does not contain the sequence of words in the query string.
        /// </summary>
        /// <param name="query">The query to search.</param>
        /// <returns>The text predicate.</returns>
        public static P TextNotContainsPhrase(string query) => new JanusGraphP("textNotContainsPhrase", query);

        /// <summary>
        ///     Is true if the string value starts with the given query string.
        /// </summary>
        /// <param name="query">The query to search.</param>
        /// <returns>The text predicate.</returns>
        public static P TextPrefix(string query) => new JanusGraphP("textPrefix", query);

        /// <summary>
        ///     Is true if the string value does not start with the given query string.
        /// </summary>
        /// <param name="query">The query to search.</param>
        /// <returns>The text predicate.</returns>
        public static P TextNotPrefix(string query) => new JanusGraphP("textNotPrefix", query);

        /// <summary>
        ///     Is true if the string value matches the given regular expression in its entirety.
        /// </summary>
        /// <param name="regex">The regular expression.</param>
        /// <returns>The text predicate.</returns>
        public static P TextRegex(string regex) => new JanusGraphP("textRegex", regex);

        /// <summary>
        ///     Is true if the string value does not match the given regular expression in its entirety.
        /// </summary>
        /// <param name="regex">The regular expression.</param>
        /// <returns>The text predicate.</returns>
        public static P TextNotRegex(string regex) => new JanusGraphP("textNotRegex", regex);

        /// <summary>
        ///     Is true if the string value is similar to the given query string (based on Levenshtein edit distance).
        /// </summary>
        /// <param name="query">The query to search.</param>
        /// <returns>The text predicate.</returns>
        public static P TextFuzzy(string query) => new JanusGraphP("textFuzzy", query);

        /// <summary>
        ///     Is true if the string value is not similar to the given query string (based on Levenshtein edit
        ///     distance).
        /// </summary>
        /// <param name="query">The query to search.</param>
        /// <returns>The text predicate.</returns>
        public static P TextNotFuzzy(string query) => new JanusGraphP("textNotFuzzy", query);
    }
}