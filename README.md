# JanusGraph.Net

JanusGraph.Net is the .NET driver of [JanusGraph](http://janusgraph.org/). It is based on Apache TinkerPop™'s [Gremlin.Net](http://tinkerpop.apache.org/docs/current/reference/#gremlin-DotNet).

## Documentation

JanusGraph.Net includes a `JanusGraphClientBuilder` that can be used to build a `IGremlinClient` pre-configured for JanusGraph. This client can then be used to configure a `GraphTraversalSource`:

```cs
var client = JanusGraphClientBuilder.BuildClientForServer(new GremlinServer("localhost", 8182)).Create();
var g = new Graph().Traversal().WithRemote(new DriverRemoteConnection(client));
```

The `Text` class provides methods for [full-text and string searches](https://docs.janusgraph.org/latest/search-predicates.html#_text_predicate).

The `Geoshape` class in the `JanusGraph.Net.Geoshapes` namespace can be used to construct [Geoshapes](https://docs.janusgraph.org/latest/search-predicates.html#geoshape).

## Contributing

Please see [`CONTRIBUTING.md` in JanusGraph's main repository](https://github.com/JanusGraph/janusgraph/blob/master/CONTRIBUTING.md) for more information, including
CLAs and best practices for working with GitHub.
