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

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var artifactsDir = Argument("artifactsDir", "artifacts");
var apiKey = Argument("apiKey", "");
var nugetSource = Argument("nugetSource", "https://api.nuget.org/v3/index.json");
var solutionPath = SolutionPath().FullPath;

Task("Clean")
    .Does(() => {
        CleanDirectory(artifactsDir);
        DotNetCoreClean(solutionPath);
    });

Task("Build")
    .IsDependentOn("Clean")
    .Does(() => {
        DotNetCoreBuild(solutionPath, new DotNetCoreBuildSettings { Configuration = configuration });
    });

Task("Test")
    .IsDependentOn("Build")
    .DoesForEach(() => TestProjects(), (testProject) => {
        DotNetCoreTest(testProject.FullPath);
    })
    .DeferOnError();

Task("Pack")
    .IsDependentOn("Test")
    .DoesForEach(() => SourceProjects(), (srcProject) => {
        var settings = new DotNetCorePackSettings
        {
            OutputDirectory = artifactsDir,
            Configuration = configuration,
            NoBuild = true
        };
        DotNetCorePack(srcProject.FullPath, settings);
    });

Task("Push")
    .DoesForEach(() => NuGetPackages(), (nugetPackage) => {
        var settings = new DotNetCoreNuGetPushSettings
        {
            ApiKey = apiKey,
            Source = nugetSource
        };
        DotNetCoreNuGetPush(nugetPackage.FullPath, settings);
    });

Task("Default")
    .IsDependentOn("Test");

RunTarget(target);

FilePath SolutionPath()
{
    return GetFiles("*.sln").First();
}

FilePathCollection TestProjects()
{
	return GetFiles("./test/**/*Test.csproj");
}

FilePathCollection SourceProjects()
{
	return GetFiles("./src/**/*.csproj");
}

FilePathCollection NuGetPackages()
{
    return GetFiles($"{artifactsDir}/*.nupkg");
}
