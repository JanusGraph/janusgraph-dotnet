# Building JanusGraph.Net

JanusGraph.Net uses [Cake][cake] for build automation to get
deterministic builds across platforms.

## Requirements

* [.NET Core SDK (version >= 2.1.400)][dotnet-sdk] is needed to build and test the project.
* [Docker][docker] needs to be running in order to execute the integration tests as they automatically start a JanusGraph Docker container.

## Cake Build Script

To execute the Cake build script, the Cake .NET CLI tool needs to be installed:

```sh
dotnet tool install -g Cake.Tool
```

Afterwards, the library can be built by executing:

```sh
dotnet cake
```

By default, the script builds and tests the library.
Other tasks can be executed by specifying them as targets:

```sh
dotnet cake --target=Pack
```

The shown `Pack` task packs the library into a NuGet package after building and
testing it.

[cake]: https://cakebuild.net/
[dotnet-sdk]: https://www.microsoft.com/net/download
[docker]: https://www.docker.com/