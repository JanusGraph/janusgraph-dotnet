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

## Deployment

We use continuous deployment via Travis to push NuGet packages to nuget.org.
To create a new release, you only have to create a [git tag][git-tag] for the
release:

```sh
git tag -a v0.1.0 -m "JanusGraph.Net 0.1.0 release"
```

and then push this tag:

```sh
git push origin v0.1.0
```

This will trigger a deployment via Travis after the usual build has completed
successfully.
The version number used for the tag should correspond to the version in the
`.csproj` file as that version is used for the NuGet package.

[cake]: https://cakebuild.net/
[dotnet-sdk]: https://www.microsoft.com/net/download
[docker]: https://www.docker.com/
[git-tag]: https://git-scm.com/book/en/v2/Git-Basics-Tagging
