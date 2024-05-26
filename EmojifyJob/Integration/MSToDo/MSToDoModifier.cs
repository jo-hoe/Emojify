using EmojifyCore.Logic;
using EmojifyCore.Logic.EmojifyStrategy;
using Newtonsoft.Json;
using System;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Text;

namespace EmojifyFunctions.Integration.MSToDo
{
    public class MSDoToModifier
    {
        private const string MSDoToBaseURL = @"https://outlook.office.com/api/v2.0/me/tasks";

        private readonly string clientId, clientSecret;
        private readonly List<string> listIds;
        private readonly IEmojifyStrategy emojifyStrategy;
        private readonly IEmojiContainer emojiContainer;
        private readonly ILogger log;

        public MSDoToModifier(IEmojiContainer emojiContainer, IEmojifyStrategy emojifyStrategy, ILogger log,
            string clientId, 
            string clientSecret, 
            params string[] listIds)
        {
            this.clientId = clientId;
            this.clientSecret = clientSecret;
            this.listIds = new List<string>(listIds);
            this.emojifyStrategy = emojifyStrategy;
            this.emojiContainer = emojiContainer;
            this.log = log;
        }

        async public void Emojify()
        {
            List<DTO.Task> existingTasks = new List<DTO.Task>();
            foreach (string listId in listIds)
            {
                existingTasks.AddRange(await GetTasks(listId));
            }

            List<DTO.Task> tasksToUpdate = new List<DTO.Task>();
            foreach (DTO.Task task in existingTasks)
            {
                if (!emojiContainer.ContainsEmoji(task.Subject))
                {
                    var emojifyedTitle = emojifyStrategy.Emojify(task.Subject);
                    if (!task.Subject.Equals(emojifyedTitle))
                    {
                        tasksToUpdate.Add(new DTO.Task
                        {
                            Subject = emojifyedTitle,
                            Id = task.Id,
                        });
                    }
                }
            }

            log.LogInformation("Number of To Do items to update is {0}", tasksToUpdate.Count);
            foreach (DTO.Task task in tasksToUpdate)
            {
                AdaptTask(task);
            }
        }

        async private Task<List<DTO.Task>> GetTasks(string listId)
        {
            HttpClient client = new HttpClient();
            using (var request = CreateRequest())
            {
                request.Method = HttpMethod.Patch;
                request.RequestUri = new Uri(MSDoToBaseURL + "('"+ listId + "')");
                var response = await client.SendAsync(request);
                if (!response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    return new List<DTO.Task>();
                }

                var responseString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<DTO.Task>>(responseString);
            }
        }

        async private void AdaptTask(DTO.Task task)
        {
            HttpClient client = new HttpClient();
            using (var request = CreateRequest())
            {
                request.Method = HttpMethod.Patch;
                request.RequestUri = new Uri(MSDoToBaseURL + "tasks/" + task.Id);
                request.Content = new StringContent(JsonConvert.SerializeObject(task), Encoding.UTF8, "application/json");
                var response = await client.SendAsync(request);
                if (!response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    var message = await response.Content.ReadAsStringAsync();
                    log.LogError("Request failed: {0} - {1}", response.StatusCode, message);
                }
            }
        }

        private HttpRequestMessage CreateRequest()
        {
            var request = new HttpRequestMessage();
            request.Headers.Add("X-Client-ID", clientId);
            request.Headers.Add("X-Access-Token", clientSecret);
            return request;
        }
    }
}
