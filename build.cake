//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var buildDir = Directory("./UmbracoDemo/bin");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

var databaseTemplate = ".\\UmbracoDemo\\App_Data\\Umbraco_Empty_Base.sdf";
var database = ".\\UmbracoDemo\\App_Data\\Umbraco.sdf";

Task("CopyDatabase")
    .Does(()=>
{
    CopyFile(databaseTemplate, database);
});


Task("Clean")
    .Does(() =>
{
    CleanDirectory(buildDir);
});

var solutionFile = "./UmbracoDemo.sln";

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    NuGetRestore(solutionFile);
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .IsDependentOn("CopyDatabase")
    .Does(() =>
{
    if(IsRunningOnWindows())
    {
      // Use MSBuild
      MSBuild(solutionFile, settings =>
        settings.SetConfiguration(configuration));
    }
    else
    {
      // Use XBuild
      XBuild(solutionFile, settings =>
        settings.SetConfiguration(configuration));
    }
});

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    NUnit3("./src/**/bin/" + configuration + "/*.Tests.dll", new NUnit3Settings {
        NoResults = true
        });
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Build");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
