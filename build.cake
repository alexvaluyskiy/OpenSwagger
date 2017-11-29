#addin "Cake.Bower"

// Usings
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

// Arguments
var target = Argument<string>("target", "Default");
var source = Argument<string>("source", null);
var apiKey = Argument<string>("apikey", null);
var skipClean = Argument<bool>("skipclean", false);
var skipTests = Argument<bool>("skiptests", false);

// Variables
var configuration = IsRunningOnWindows() ? "Release" : "MonoRelease";
var csProjectFiles = GetFiles("./src/**/*.csproj");

// Directories
var nuget = Directory(".nuget");
var output = Directory("build");
var outputBinaries = output + Directory("binaries");
var outputBinariesNetstandard = outputBinaries + Directory("netstandard2.0");
var outputPackages = output + Directory("packages");
var outputNuGet = output + Directory("nuget");

///////////////////////////////////////////////////////////////

Task("Clean")
  .Does(() =>
{
  // Clean artifact directories.
  CleanDirectories(new DirectoryPath[] {
    output, outputBinaries, outputPackages, outputNuGet, outputBinariesNetstandard
  });

  if(!skipClean) {
    // Clean output directories.
    CleanDirectories("./**/bin/" + configuration);
    CleanDirectories("./**/obj/" + configuration);
  }
});

Task("Restore")
  .Description("Restores dependencies")
  .Does(() =>
{
  Bower.Install(s => s.UseWorkingDirectory("./src/OpenSwagger.SwaggerUI"));

  DotNetCoreRestore("./OpenSwagger.sln", new DotNetCoreRestoreSettings
  {
    NoCache = true,
    Runtime = "win-x64"
  });
});

Task("Compile")
  .Description("Builds the solution")
  .IsDependentOn("Clean")
  .IsDependentOn("Restore")
  .Does(() =>
{
  DotNetCoreBuild("./OpenSwagger.sln", new DotNetCoreBuildSettings
  {
    Configuration = configuration
  });
});

Task("Test")
  .Description("Executes xUnit tests")
  .WithCriteria(!skipTests)
  .IsDependentOn("Compile")
  .Does(() =>
{
  var projects = GetFiles("./test/**/*.csproj");

  foreach(var project in projects)
  {
    DotNetCoreTest(project.FullPath, new DotNetCoreTestSettings
    {
      Configuration = configuration
    });
  }
});

Task("Package-NuGet")
  .Description("Generates NuGet packages for each project that contains a nuspec")
  .Does(() =>
{
  var settings = new DotNetCorePackSettings {
    Configuration = configuration,
    OutputDirectory = outputNuGet,
    IncludeSymbols = true,
    IncludeSource = true
  };

  foreach(var project in csProjectFiles)
  {
    DotNetCorePack(project.GetDirectory().FullPath, settings);
  }

});

Task("Default")
  .IsDependentOn("Test"); 

///////////////////////////////////////////////////////////////

RunTarget(target);