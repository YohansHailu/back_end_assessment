using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using TaskManagerApp.Models;

public class TaskManagerAppEndPoints
{
    public static void ConfigureEndpoints(WebApplication app)
    {
        app.MapGet("/tasks", async ([FromServices] IMongoDatabase database) =>
        {
            var tasksCollection = database.GetCollection<ProjectTask>("tasks");
            var tasks = await tasksCollection.Find(_ => true).ToListAsync();
            return Results.Ok(tasks);
        });

        app.MapGet("/tasks/{id}", async ([FromServices] IMongoDatabase database, string id) =>
        {
            var tasksCollection = database.GetCollection<ProjectTask>("tasks");
            var task = await tasksCollection.Find(t => t.Id == id).FirstOrDefaultAsync();

            if (task == null)
            {
                return Results.NotFound($"Task with ID {id} not found.");
            }

            return Results.Ok(task);
        });

        app.MapPost("/tasks", async ([FromServices] IMongoDatabase database, ProjectTask task) =>
        {
            var tasksCollection = database.GetCollection<ProjectTask>("tasks");
            await tasksCollection.InsertOneAsync(task);
            return Results.Created($"/tasks/{task.Id}", task);
        });

        app.MapPut("/tasks/{id}", async ([FromServices] IMongoDatabase database, string id, ProjectTask updatedTask) =>
        {
            var tasksCollection = database.GetCollection<ProjectTask>("tasks");
            var existingTask = await tasksCollection.Find(t => t.Id == id).FirstOrDefaultAsync();

            if (existingTask == null)
            {
                return Results.NotFound($"Task with ID {id} not found.");
            }

            updatedTask.Id = id;
            await tasksCollection.ReplaceOneAsync(t => t.Id == id, updatedTask);
            return Results.NoContent();
        });

        app.MapDelete("/tasks/{id}", async ([FromServices] IMongoDatabase database, string id) =>
        {
            var tasksCollection = database.GetCollection<ProjectTask>("tasks");
            var result = await tasksCollection.DeleteOneAsync(t => t.Id == id);

            if (result.DeletedCount == 0)
            {
                return Results.NotFound($"Task with ID {id} not found.");
            }

            return Results.NoContent();
        });
    }
}
