# Building JanusGraph.Net

JanusGraph.Net uses [Cake](https://cakebuild.net/) for build automation to get
deterministic builds across platforms.

## Requirements

[.NET Core SDK 2.1](https://www.microsoft.com/net/download) is needed to build
and test the project. Docker needs to be running in order to execute the
integration tests as they automatically start a JanusGraph Docker container.

## Build Scripts

The repository contains two build scripts, one for Linux (`build.sh`) and one
for Windows (`build.ps1`). These scripts bootstrap and execute the Cake
script (`build.cake`) which contains different tasks for building and testing.

When these scripts are executed without any arguments, then the library will be
built and tested.

Other tasks can be executed by specifying them as targets:

```sh
./build.sh --target Pack
```

or on Windows:

```sh
./build.ps1 -target Pack
```

The shown `Pack` task packs the library into a NuGet package after building and
testing it.