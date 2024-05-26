using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using EmojifyFunctions;
using EmojifyCore.Logic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Text;
using EmojifyCore.Logic.EmojifyStrategy;

namespace EmojifyJob
{
    public class Emojify
    {
        private readonly IEmojiContainer emojiContainer;

        public Emojify(IEmojiContainer emojiContainer)
        {
            this.emojiContainer = emojiContainer;
        }

        [FunctionName("Emojify")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] 
            HttpRequestMessage request)
        {
            string requestBody = await request.Content.ReadAsStringAsync().ConfigureAwait(true);
            var requestBodyLines = Regex.Split(requestBody, "\r\n|\r|\n");

            StringBuilder stringBuilder = new StringBuilder();
            AfterSentenceStrategy strategy = new AfterSentenceStrategy(emojiContainer);
            foreach (var line in requestBodyLines)
            {
                stringBuilder.Append(strategy.Emojify(line));
            }

            return new OkObjectResult(stringBuilder.ToString());
        }
    }
}
