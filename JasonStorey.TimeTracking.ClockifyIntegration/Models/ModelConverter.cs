using System;
using Clockify.Net.Models.Clients;
using Clockify.Net.Models.Tags;
using Clockify.Net.Models.TimeEntries;
using Clockify.Net.Models.Users;
using Clockify.Net.Models.Workspaces;

namespace JasonStorey.TimeTracking.ClockifyIntegration
{
    public class ModelConverter
    {
        public static User ToUser(CurrentUserDto user) =>
            new User
            {
                Name = user.Name,
                Email = user.Email,
                Id = user.Id,
                WorkspaceId = user.ActiveWorkspace
            };

        public static Workspace ToWorkspace(WorkspaceDto workspace) =>
            new Workspace
            {
                Id = workspace.Id,
                Name = workspace.Name,
                Image = workspace.ImageUrl
            };

        public static Client ToClient(ClientDto client) =>
            new Client
            {
                Id = client.Id,
                Name = client.Name,
                WorkspaceId = client.WorkspaceId
            };

        public static Tag ToTag(TagDto tag) =>
            new Tag
            {
                Id = tag.Id,
                Name = tag.Name,
                WorkspaceId = tag.WorkspaceId
            };

        public static TimeEntry ToTimeEntry(TimeEntryDtoImpl entry) =>
            new TimeEntry
            {
                Id = entry.Id,
                Name = entry.Description,
                Client = new Client
                {
                    Id = entry.ClientId,
                    Name = entry.ClientName,
                    WorkspaceId = entry.WorkspaceId,
                },
                IsRunning = !entry.TimeInterval.End.HasValue,
                Started = entry.TimeInterval.Start.Value,
                Ended = entry.TimeInterval.End
            };
    }

    public class TimeEntry
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsRunning { get; set; }
        public DateTimeOffset Started { get; set; }
        public DateTimeOffset? Ended { get; set; }
        public TimeSpan Duration { get; set; }
        public Client Client { get; set; }
    }
    
    public class Tag
    {
        public string Id { get; set; }
        public string WorkspaceId { get; set; }
        public string Name { get; set; }
    }
    
}