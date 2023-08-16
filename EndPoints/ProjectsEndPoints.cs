using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using TaskManagerApp.Models;

public class ProjectsEndpoints
{
    public static void ConfigureEndpoints(WebApplication app)
    {
        app.MapGet("/projects/{id}", async ([FromServices] IMongoDatabase database, string id) =>
        {
            var projectsCollection = database.GetCollection<Project>("projects");
            var project = await projectsCollection.Find(p => p.Id == id).FirstOrDefaultAsync();

            if (project == null)
            {
                return Results.NotFound($"Project with ID {id} not found.");
            }

            return Results.Ok(project);
        });

        app.MapPost("/projects", async ([FromServices] IMongoDatabase database, Project project) =>
        {
            var projectsCollection = database.GetCollection<Project>("projects");
            await projectsCollection.InsertOneAsync(project);
            return Results.Created($"/projects/{project.Id}", project);
        });

        app.MapPut("/projects/{id}", async ([FromServices] IMongoDatabase database, string id, Project updatedProject) =>
        {
            var projectsCollection = database.GetCollection<Project>("projects");
            var existingProject = await projectsCollection.Find(p => p.Id == id).FirstOrDefaultAsync();

            if (existingProject == null)
            {
                return Results.NotFound($"Project with ID {id} not found.");
            }

            updatedProject.Id = id;
            await projectsCollection.ReplaceOneAsync(p => p.Id == id, updatedProject);
            return Results.NoContent();
        });

        app.MapDelete("/projects/{id}", async ([FromServices] IMongoDatabase database, string id) =>
        {
            var projectsCollection = database.GetCollection<Project>("projects");
            var result = await projectsCollection.DeleteOneAsync(p => p.Id == id);

            if (result.DeletedCount == 0)
            {
                return Results.NotFound($"Project with ID {id} not found.");
            }

            return Results.NoContent();
        });
    }
}
