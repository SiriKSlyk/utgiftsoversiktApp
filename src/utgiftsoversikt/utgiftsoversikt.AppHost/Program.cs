

var builder = DistributedApplication.CreateBuilder(args);

var db = builder.ExecutionContext.IsRunMode ?
        builder.AddConnectionString("cosmos"):
        builder.AddAzureCosmosDB("cosmos").AddDatabase("db");//.RunAsEmulator();
//var db = builder.AddAzureCosmosDB("cosmos").AddDatabase("db");

var backend = builder.AddProject<Projects.utgiftsoversikt>("utgiftsoversikt")
    .WithReference(db)
    .WithExternalHttpEndpoints();



builder.AddNpmApp("frontend", "../../../../frontend").WithReference(backend)
    .WithEnvironment("BROWSER", "none") // Disable opening browser on npm start
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints();


builder.Build().Run();
