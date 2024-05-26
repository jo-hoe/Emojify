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

namespace EmojifyFunctions.Integration.Todoist
{
    public class TodoistModifier
    {
        private const string TodoistBaseURL = @"https://api.todoist.com/rest/v1/";

        private readonly string token;
        private readonly IEmojifyStrategy emojifyStrategy;
        private readonly IEmojiContainer emojiContainer;
        private readonly ILogger log;

        public TodoistModifier(IEmojifyStrategy emojifyStrategy, IEmojiContainer emojiContainer, ILogger log, string token)
        {
            this.token = token;
            this.emojifyStrategy = emojifyStrategy;
            this.emojiContainer = emojiContainer;
            this.log = log;
        }

        async public void Emojify()
        {
            List<DTO.Task> existingTasks = await GetTasks();
            List<DTO.Task> tasksToUpdate = new List<DTO.Task>();
            foreach (DTO.Task task in existingTasks)
            {
                if (!emojiContainer.ContainsEmoji(task.Content))
                {
                    var emojifyedTitle = emojifyStrategy.Emojify(task.Content, 3);
                    if (!task.Content.Equals(emojifyedTitle))
                    {
                        tasksToUpdate.Add(new DTO.Task
                        {
                            Id = task.Id,
                            Content = emojifyedTitle
                        });
                    }
                }
            }

            log.LogInformation("Number of Todoist items to update is {0}", tasksToUpdate.Count);
            foreach (DTO.Task task in tasksToUpdate)
            {
                AdaptTask(task);
            }
        }

        async private Task<List<DTO.Task>> GetTasks()
        {
            HttpClient client = new HttpClient();
            using (var request = CreateRequest())
            {
                request.Method = HttpMethod.Get;
                request.RequestUri = new Uri(TodoistBaseURL + "tasks");
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
                request.Method = HttpMethod.Post;
                request.Headers.Add("X-Request-Id", System.Guid.NewGuid().ToString());
                request.RequestUri = new Uri(TodoistBaseURL + "tasks/" + task.Id);
                string jsonContent = JsonConvert.SerializeObject(task);

                // setting body in this way instead of using
                // request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                // since the line above sets the Content-Type to "application/json; charset=utf-8"
                // and if charset is set, the API returns an error
                HttpContent httpBody = new StringContent(jsonContent);
                httpBody.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                request.Content = httpBody;

                var response = await client.SendAsync(request);
                if (!response.StatusCode.Equals(HttpStatusCode.NoContent))
                {
                    var message = await response.Content.ReadAsStringAsync();
                    log.LogError("Request failed: {0} - {1}", response.StatusCode, message);
                }
            }
        }

        private HttpRequestMessage CreateRequest()
        {
            var request = new HttpRequestMessage();
            request.Headers.Add("Authorization", "Bearer " + token);
            return request;
        }
    }
}
