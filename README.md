# JanusGraph.Net

JanusGraph.Net is the .NET driver of [JanusGraph][JanusGraph]. It extends
Apache TinkerPop™'s [Gremlin.Net][Gremlin.Net] as its core dependency
with additional support for JanusGraph-specific types.

## Documentation

JanusGraph.Net includes a `JanusGraphClientBuilder` that can be used to build
a `IGremlinClient` pre-configured for JanusGraph. This client can then be used
to configure a `GraphTraversalSource`:

```cs
var client = JanusGraphClientBuilder
    .BuildClientForServer(new GremlinServer("localhost", 8182))
    .Create();
var g = new Graph().Traversal().WithRemote(new DriverRemoteConnection(client));
```

The `Text` class provides methods for
[full-text and string searches][text-predicates].

The `Geoshape` class in the `JanusGraph.Net.Geoshapes` namespace can be used to
construct [Geoshapes][geoshapes].

## Community

JanusGraph.Net uses the same communication channels as JanusGraph in general.
So, please refer to the
[_Community_ section in JanusGraph's main repository][JanusGraph-community]
for more information about these various channels.

Please use GitHub issues only to report bugs or request features.

## Contributing

Please see
[`CONTRIBUTING.md` in JanusGraph's main repository][JanusGraph-contributing]
for more information, including CLAs and best practices for working with
GitHub.

## License

JanusGraph.Net code is provided under the [Apache 2.0 license](APACHE-2.0.txt)
and documentation is provided under the [CC-BY-4.0 license](CC-BY-4.0.txt). For
details about this dual-license structure, please see
[`LICENSE.txt`](LICENSE.txt).

[JanusGraph]: http://janusgraph.org/
[Gremlin.Net]: http://tinkerpop.apache.org/docs/current/reference/#gremlin-DotNet
[text-predicates]: https://docs.janusgraph.org/latest/search-predicates.html#_text_predicate
[geoshapes]: https://docs.janusgraph.org/latest/search-predicates.html#geoshape
[JanusGraph-community]: https://github.com/JanusGraph/janusgraph#community
[JanusGraph-contributing]: https://github.com/JanusGraph/janusgraph/blob/master/CONTRIBUTING.md