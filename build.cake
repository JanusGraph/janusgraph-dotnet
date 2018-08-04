var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var artifactsDir = Argument("artifactsDir", "artifacts");
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