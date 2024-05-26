using EmojifyCore.Logic;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace EmojifyFunctions
{
    public class EmojifyJob
    {
        private readonly IEmojiContainer emojiContainer;

        public EmojifyJob(IEmojiContainer emojiContainer)
        {
            this.emojiContainer = emojiContainer;
        }

        [FunctionName("EmojifyJob")]
        public void Run([TimerTrigger("0 0,30 7-23 * * *")]TimerInfo myTimer, ILogger log)
        {
            new TaskContainer(emojiContainer, log).Emojify();
        }
    }
}
