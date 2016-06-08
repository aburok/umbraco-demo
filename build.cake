var target = Argument("target", "Default");

Task("Default")
    .IsDependentOn("CopyDatabase")
    .Does(() =>
    {
        Information("Hello World!");
    });

var databaseTemplate = ".\\UmbracoDemo\\App_Data\\Umbraco_Empty_Base.sdf";
var database = ".\\UmbracoDemo\\App_Data\\Umbraco.sdf";

Task("CopyDatabase")
    .Does(()=>{
        CopyFile(databaseTemplate, database);
    });



RunTarget(target);
