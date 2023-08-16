using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using TaskManagerApp.Models;

public class UserEndPoints
{
    public static void ConfigureEndpoints(WebApplication app)
    {
        app.MapGet("/users", async ([FromServices] IMongoDatabase database) =>
        {
            var usersCollection = database.GetCollection<User>("users");
            var users = await usersCollection.Find(_ => true).ToListAsync();
            return Results.Ok(users);
        });

        app.MapGet("/users/{id}", async ([FromServices] IMongoDatabase database, string id) =>
        {
            var usersCollection = database.GetCollection<User>("users");
            var user = await usersCollection.Find(u => u.Id == id).FirstOrDefaultAsync();

            if (user == null)
            {
                return Results.NotFound($"User with ID {id} not found.");
            }

            return Results.Ok(user);
        });

        app.MapPost("/users", async ([FromServices] IMongoDatabase database, User user) =>
        {
            var usersCollection = database.GetCollection<User>("users");
            await usersCollection.InsertOneAsync(user);
            return Results.Created($"/users/{user.Id}", user);
        });

        app.MapPut("/users/{id}", async ([FromServices] IMongoDatabase database, string id, User updatedUser) =>
        {
            var usersCollection = database.GetCollection<User>("users");
            var existingUser = await usersCollection.Find(u => u.Id == id).FirstOrDefaultAsync();

            if (existingUser == null)
            {
                return Results.NotFound($"User with ID {id} not found.");
            }

            updatedUser.Id = id;
            await usersCollection.ReplaceOneAsync(u => u.Id == id, updatedUser);
            return Results.NoContent();
        });

        app.MapDelete("/users/{id}", async ([FromServices] IMongoDatabase database, string id) =>
        {
            var usersCollection = database.GetCollection<User>("users");
            var result = await usersCollection.DeleteOneAsync(u => u.Id == id);

            if (result.DeletedCount == 0)
            {
                return Results.NotFound($"User with ID {id} not found.");
            }

            return Results.NoContent();
        });
    }
}
