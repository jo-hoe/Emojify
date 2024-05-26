using EmojifyCore.Logic;
using EmojifyFunctions;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.IO;

[assembly: FunctionsStartup(typeof(Startup))]
namespace EmojifyFunctions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var executionContextOptions = builder.Services.BuildServiceProvider().GetService<IOptions<ExecutionContextOptions>>().Value;
            var currentDirectory = executionContextOptions.AppDirectory;
            var path = Path.GetFullPath(Path.Combine(currentDirectory, EmojiContainer.DefaultInputFilePath));
            builder.Services.AddSingleton<IEmojiContainer>(x => new EmojiContainer(path));
        }
    }
}