using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TaskManagerApp.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonRequired]
        public string? Email { get; set; }

        [BsonRequired]
        public string? PasswordHash { get; set; }

        public List<string>? Projects { get; set; }
        public List<string>? Tasks { get; set; }
        public List<string>? AssignedTasks { get; set; }
    }

    public class Project
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonRequired]
        public string? Name { get; set; }

        [BsonRequired]
        public string? OwnerId { get; set; }

        public List<string>? CollaboratorIds { get; set; }
        public List<string>? Tasks { get; set; }
        public List<string>? Users { get; set; }
    }

    public class ProjectTask
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonRequired]
        public string? Title { get; set; }

        public string? Description { get; set; }

        [BsonRequired]
        public DateTime DueDate { get; set; }

        [BsonRequired]
        public string? Status { get; set; }

        [BsonRequired]
        public string? ProjectId { get; set; }

        [BsonRequired]
        public string? CreatorId { get; set; }

        public List<string>? AssignedUserIds { get; set; }


        public string? Priority { get; set; }
        public List<string>? Labels { get; set; }
    }

    // public enum TaskStatus
    // {
    //     ToDo,
    //     InProgress,
    //     Done
    // }

    // public enum Priority
    // {
    //     Low,
    //     Medium,
    //     High
    // }
}
