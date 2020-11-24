# Building JanusGraph.Net

JanusGraph.Net uses dotnet build for convenient builds across platforms.

## Requirements

* [.NET Core SDK (version >= 2.1.400)][dotnet-sdk] is needed to build and test the project.
* [Docker][docker] needs to be running in order to execute the integration tests as they automatically start a JanusGraph Docker container.

## Build

The library can be tested by executing:

```sh
dotnet test JanusGraph.NET.sln
```

The library can be packed into NuGet package by executing:

```sh
dotnet pack -v minimal -c Release -o ./artifacts JanusGraph.NET.sln
```

## Deployment

We use continuous deployment via GitHub Actions to push NuGet packages to nuget.org.
To create a new release, you only have to create a [git tag][git-tag] for the
release:

```sh
git tag -a v0.1.0 -m "JanusGraph.Net 0.1.0 release"
```

and then push this tag:

```sh
git push origin v0.1.0
```

This will trigger a deployment via Github Actions after the usual build has completed
successfully.
The version number used for the tag should correspond to the version in the
`.csproj` file as that version is used for the NuGet package.

[dotnet-sdk]: https://www.microsoft.com/net/download
[docker]: https://www.docker.com/
[git-tag]: https://git-scm.com/book/en/v2/Git-Basics-Tagging
