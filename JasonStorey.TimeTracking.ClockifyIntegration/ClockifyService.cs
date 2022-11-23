using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clockify.Net;
using Clockify.Net.Models.Clients;
using Clockify.Net.Models.Projects;
using Clockify.Net.Models.Tags;
using Clockify.Net.Models.TimeEntries;
using static JasonStorey.TimeTracking.ClockifyIntegration.ModelConverter;

namespace JasonStorey.TimeTracking.ClockifyIntegration
{
    public class ClockifyService
    {
        readonly ILogger _logger;
        readonly ClockifyClient _client;
        public ClockifyService(string key,ILogger logger)
        {
            _logger = logger;
            _client = new ClockifyClient(key);
        }

        public async Task<User> GetUser()
        {
            var user = await _client.GetCurrentUserAsync();
            return ToUser(user.Data);
        }

        public async Task<TimeEntry> GetCurrentEntry()
        {
            var current = await GetTimeEntries();
            return current.FirstOrDefault(x => x.IsRunning);
        }

        public async Task<TimeEntry> StopCurrentTask()
        {
            try
            {
                var current = await GetCurrentEntry();
                if (current == null) return null;
                
                var result = await _client.UpdateTimeEntryAsync(current.Client.WorkspaceId, current.Id,
                    new UpdateTimeEntryRequest
                    {
                        Start = current.Started,
                        Description        = current.Name,
                        End = DateTimeOffset.Now,
                        Billable = false
                    });

                return ToTimeEntry(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        public async Task<TimeEntry> StartTask(string name)
        {
            try
            {
                var user = await GetUser();
                var result = await _client.CreateTimeEntryAsync(user.WorkspaceId, new TimeEntryRequest
                {
                    Start = DateTimeOffset.Now,
                    Billable = false,
                    Description = name
                });
                return ToTimeEntry(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }
        

        public async Task<IEnumerable<TimeEntry>> GetTimeEntries()
        {
            var user = await GetUser();
            var results = await _client.FindAllTimeEntriesForUserAsync(user.WorkspaceId, user.Id);
            return results.Data.Select(ToTimeEntry);
        }

        public async Task<IEnumerable<Tag>> GetTagsAsync()
        {
            var workspace = await GetCurrentWorkspace();
            var result =  await _client.FindAllTagsOnWorkspaceAsync(workspace.Id);
            return result.Data.Select(ToTag);
        }

        public async Task<Client> AddClient(string client)
        {
            var en = await GetCurrentWorkspace();
            var clientResult = await _client.CreateClientAsync(en.Id,new ClientRequest
            {
Name = client
            });

            return ToClient(clientResult.Data);
        }

        public async Task<Tag> AddTag(string tag)
        {
            var en = await GetCurrentWorkspace();
            var result = await _client.CreateTagAsync(en.Id, new TagRequest
            { 
                Name = tag
            });
            return ToTag(result.Data);
        }
        
        public async Task AddProject(Client client,string project,string description,string color)
        {
            var en = await GetCurrentWorkspace();
            await _client.CreateProjectAsync(en.Id, new ProjectRequest
            {
                Name = project,
                ClientId = client.Id,
                Color = color,
                Note = description
            });
        }
        
        
        public async Task<Workspace> GetCurrentWorkspace()
        {
            var user = await GetUser();
            var workspaces = await GetWorkspaces();
            var current = workspaces.FirstOrDefault(x => x.Id == user.WorkspaceId);
            return current;
        }

        public async Task<IEnumerable<Workspace>> GetWorkspaces()
        {
            var ws = await _client.GetWorkspacesAsync();
            return ws.Data.Select(ToWorkspace);
        }
        

        
    }
}