using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using TaskManagerApp.Models;

class RelationEndPoints
{

    public static void ConfigureEndpoints(WebApplication app)
    {
        // assing task to users
        app.MapPost("/users/{userId}/add-task/{taskId}", async ([FromServices] IMongoDatabase database, string userId, string taskId) =>
               {
                   var usersCollection = database.GetCollection<User>("users");
                   var tasksCollection = database.GetCollection<ProjectTask>("tasks");

                   var user = await usersCollection.Find(u => u.Id == userId).FirstOrDefaultAsync();
                   var task = await tasksCollection.Find(t => t.Id == taskId).FirstOrDefaultAsync();

                   if (user == null || task == null)
                   {
                       return Results.NotFound("User or Task not found.");
                   }

                   if (user.Tasks == null)
                   {
                       user.Tasks = new List<string>();
                   }

                   user.Tasks.Add(task.Id);

                   await usersCollection.ReplaceOneAsync(u => u.Id == userId, user);

                   return Results.NoContent();
               });

        // Add a user to a project's list of users
        app.MapPost("/projects/{projectId}/add-user/{userId}", async ([FromServices] IMongoDatabase database, string projectId, string userId) =>
        {
            // Retrieve projects and users collections
            var projectsCollection = database.GetCollection<Project>("projects");
            var usersCollection = database.GetCollection<User>("users");

            var project = await projectsCollection.Find(p => p.Id == projectId).FirstOrDefaultAsync();
            var user = await usersCollection.Find(u => u.Id == userId).FirstOrDefaultAsync();

            if (project == null || user == null)
            {
                return Results.NotFound("Project or User not found.");
            }

            if (project.Users == null)
            {
                project.Users = new List<string>();
            }

            project.Users.Add(user.Id);

            await projectsCollection.ReplaceOneAsync(p => p.Id == projectId, project);

            return Results.NoContent();
        });

        // Add a task to a project's list of tasks
        app.MapPost("/projects/{projectId}/add-task/{taskId}", async ([FromServices] IMongoDatabase database, string projectId, string taskId) =>
        {
            // Retrieve projects and tasks collections
            var projectsCollection = database.GetCollection<Project>("projects");
            var tasksCollection = database.GetCollection<ProjectTask>("tasks");

            var project = await projectsCollection.Find(p => p.Id == projectId).FirstOrDefaultAsync();
            var task = await tasksCollection.Find(t => t.Id == taskId).FirstOrDefaultAsync();

            if (project == null || task == null)
            {
                return Results.NotFound("Project or Task not found.");
            }

            if (project.Tasks == null)
            {
                project.Tasks = new List<string>();
            }

            project.Tasks.Add(task.Id);

            await projectsCollection.ReplaceOneAsync(p => p.Id == projectId, project);

            return Results.NoContent();
        });


    }
}
