# JanusGraph.Net

JanusGraph.Net extends Apache TinkerPop™'s [Gremlin.Net][gremlin.net] with
support for [JanusGraph][janusgraph]-specific types.

[![GitHub Workflow Status][actions-badge]][actions-url]
[![Codacy Badge][codacy-badge]][codacy-url]
[![Nuget][nuget-badge]][nuget-url]

## Usage

To connect to JanusGraph Server, a `GremlinClient` instance needs to be
created and configured with a message serializer that adds support for
JanusGraph specific types.

This can be done like this for GraphSON 3:

```cs
var client = new GremlinClient(new GremlinServer("localhost", 8182),
    new JanusGraphGraphSONMessageSerializer());
```

or like this for GraphBinary (see
[Serialization Formats](#serialization-formats) for more information about
GraphSON 3 and GraphBinary):

```cs
var client = new GremlinClient(new GremlinServer("localhost", 8182),
    new GraphBinaryMessageSerializer(JanusGraphTypeSerializerRegistry.Instance));
```

Note that the client should be disposed on shut down to release resources and
to close open connections with `client.Dispose()`.
The client can then be used to configure a `GraphTraversalSource`:

```cs
// Add this static using to be able to call `Traversal()` "anonymously"
using static Gremlin.Net.Process.Traversal.AnonymousTraversalSource;

var g = Traversal().WithRemote(new DriverRemoteConnection(client));
// Reuse 'g' across the application
```

The `GraphTraversalSource` `g` can now be used to spawn Gremlin traversals:

```cs
var herculesAge = g.V().Has("demigod", "name", "hercules").Values<int>("age")
    .Next();
Console.WriteLine($"Hercules is {herculesAge} years old.");
```

The traversal can also be executed asynchronously by using `Promise()` which is
recommended as the underlying driver in Gremlin.Net also works
asynchronously:

```cs
var herculesAge = await g.V().Has("demigod", "name", "hercules")
    .Values<int>("age")
    .Promise(t => t.Next());
```

Refer to the chapter [Gremlin Query Language][gremlin-chapter] in the
JanusGraph docs for an introduction to Gremlin and pointers to further
resources.
The main syntactical difference for Gremlin.Net is that it follows .NET naming
conventions, e.g., method names use PascalCase instead of camelCase.

### Text Predicates

The `Text` class provides methods for
[full-text and string searches][text-predicates]:

```cs
await g.V().Has("demigod", "name", Text.TextPrefix("herc"))
    .Promise(t => t.ToList());
```

The other text predicates can be used the same way.

### Geoshapes

The `Geoshape` class in the `JanusGraph.Net.Geoshapes` namespace can be used to
construct [Geoshapes][geoshapes]:

```cs
await g.V().Has("demigod", "name", "hercules").OutE("battled")
    .Has("place", Geoshape.Point(38.1f, 23.7f)).Count().Promise(t => t.Next());
```

Only the `Point` Geoshape is supported right now.

## Version Compatibility

The lowest supported JanusGraph version is 0.3.0.
The following table shows the supported JanusGraph versions for each version
of JanusGraph.Net:

| JanusGraph.Net | JanusGraph             |
| -------------- | ---------------------- |
| 0.1.z          | 0.3.z                  |
| 0.2.z          | 0.4.z, 0.5.z           |
| 0.3.z          | 0.4.z, 0.5.z, 0.6.z    |
| 0.4.z          | (0.4.z, 0.5.z,) 0.6.z  |
| 1.0.z          | 0.6.z, 1.0.z           |

While it should also be possible to use JanusGraph.Net with other versions of
JanusGraph than mentioned here, compatibility is not tested and some
functionality (like added Gremlin steps) will not work as it is not supported
yet in case of an older JanusGraph version or was removed in a newer JanusGraph
version.

### JanusGraph.Net 0.4

JanusGraph.Net 0.4 still supports older versions of JanusGraph, but the
`janusGraphPredicates` flag needs to be set to `false` in order to be able to
use JanusGraph's Text predicates.

### JanusGraph.Net 1.0

Since JanusGraph 1.0.0 is not officially released yet, we also only have prerelease versions of JanusGraph.Net.
Note that serialization of Geoshapes via GraphBinary is only compatible with JanusGraph 1.0.0 and higher.

## Serialization Formats

JanusGraph.Net supports GraphSON 3 as well as GraphBinary.
Note that support for GraphBinary was only added in JanusGraph 0.6.0. So, the
server needs to be at least on that version.

Not all of the JanusGraph-specific types are already supported by both formats:

| Format      | RelationIdentifier | Text predicates | Geoshapes | Geo predicates |
| ----------- | ------------------ | --------------- | --------- | -------------- |
| GraphSON3   | x                  | x               | `Point`   | -              |
| GraphBinary | x                  | x               | `Point`*  | -              |

\* Since version 1.0.0 of JanusGraph.Net.
JanusGraph also needs to be on version 1.0.0 or higher.

## Community

JanusGraph.Net uses the same communication channels as JanusGraph in general.
So, please refer to the
[_Community_ section in JanusGraph's main repository][janusgraph-community]
for more information about these various channels.

Please use GitHub issues only to report bugs or request features.

## Contributing

Please see
[`CONTRIBUTING.md` in JanusGraph's main repository][janusgraph-contributing]
for more information, including CLAs and best practices for working with
GitHub.

## License

JanusGraph.Net code is provided under the [Apache 2.0 license](APACHE-2.0.txt)
and documentation is provided under the [CC-BY-4.0 license](CC-BY-4.0.txt). For
details about this dual-license structure, please see
[`LICENSE.txt`](LICENSE.txt).

[codacy-badge]: https://api.codacy.com/project/badge/Grade/eb69004e41f64f03be82228e6faaedd1
[codacy-url]: https://app.codacy.com/project/JanusGraph/janusgraph-dotnet/dashboard
[nuget-badge]: https://img.shields.io/nuget/v/JanusGraph.NET
[nuget-url]: https://www.nuget.org/packages/JanusGraph.NET/
[actions-badge]:
https://img.shields.io/github/actions/workflow/status/JanusGraph/janusgraph-dotnet/dotnet.yml?branch=master
[actions-url]: https://github.com/JanusGraph/janusgraph-dotnet/actions
[janusgraph]: https://janusgraph.org/
[gremlin.net]: https://tinkerpop.apache.org/docs/current/reference/#gremlin-DotNet
[gremlin-chapter]: https://docs.janusgraph.org/getting-started/gremlin/
[text-predicates]: https://docs.janusgraph.org/interactions/search-predicates/#text-predicate
[geoshapes]: https://docs.janusgraph.org/interactions/search-predicates/#geoshape-data-type
[janusgraph-community]: https://github.com/JanusGraph/janusgraph#community
[janusgraph-contributing]: https://github.com/JanusGraph/janusgraph/blob/master/CONTRIBUTING.md
