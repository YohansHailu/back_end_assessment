using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json");
var configuration = builder.Configuration;

// MongoDB Configuration
var mongoClient = new MongoClient(configuration.GetConnectionString("MongoDB"));
var database = mongoClient.GetDatabase(configuration.GetValue<string>("MongoDBSettings:DatabaseName"));
builder.Services.AddSingleton<IMongoDatabase>(database);

var app = builder.Build();
TaskManagerAppEndPoints.ConfigureEndpoints(app);
//ProjectManagerAppEndPoints.ConfigureEndpoints(app);
//UserManagerAppEndPoints.ConfigureEndpoints(app);
app.Run();
