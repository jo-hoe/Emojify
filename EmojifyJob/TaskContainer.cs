using EmojifyCore.Logic;
using EmojifyCore.Logic.EmojifyStrategy;
using EmojifyFunctions.Integration.Todoist;
using EmojifyFunctions.Integration.MSToDo;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmojifyFunctions
{
    public class TaskContainer
    {
        private readonly IEmojiContainer emojiContainer;
        private readonly ILogger logger;

        public TaskContainer(IEmojiContainer emojiContainer, ILogger logger)
        {
            this.emojiContainer = emojiContainer;
            this.logger = logger;
        }

        public void Emojify()
        {
            //EmojifyMSToDo();
            EmojifyTodoist();
        }

        private void EmojifyMSToDo()
        {
            var afterTextStrategy = new AfterTextStrategy(emojiContainer);
            var msDoToModifier = new MSDoToModifier(emojiContainer, afterTextStrategy, logger,
                Environment.GetEnvironmentVariable("Wunderlist:ClientId"),
                Environment.GetEnvironmentVariable("Wunderlist:ClientSecret"),
                Environment.GetEnvironmentVariable("Wunderlist:ListIds").Split(";"));

            logger.LogInformation("Starting to update Wunderlist");
            msDoToModifier.Emojify();
        }

        private void EmojifyTodoist()
        {
            var strategy = new BeforeTextStrategy(emojiContainer);
            var todoistModifier = new TodoistModifier(strategy, emojiContainer, logger,
                Environment.GetEnvironmentVariable("Todoist:Token"));

            logger.LogInformation("Starting to update Todoist");
            todoistModifier.Emojify();
        }
    }
}
