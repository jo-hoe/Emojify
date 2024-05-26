using EmojiDataStructureCreator.Serialization;
using EmojifyCore.Logic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace EmojiDataStructureCreator
{
    /// <summary>
    /// Program which downloads an emoji libary and converts it into a json file which
    /// is used in this solution to build a emoji data structure.
    /// 
    /// The GitHub used for this purpose is https://github.com/github/gemoji
    /// </summary>
    public class Creator
    {
        private const string EmojiURL = @"https://raw.githubusercontent.com/github/gemoji/master/db/emoji.json";
        private const string OutputDumpFolder = @"EmojifyCore\Resources";
        private const string FileName = @"EmojiSource.json";
        private const string FilePath = @"..\..\..\..\" + OutputDumpFolder + @"\";

        static async Task Main(string[] args)
        {
            Console.WriteLine("Getting old file");
            string oldFile = System.IO.File.ReadAllText(FilePath + FileName);
            SortedDictionary<string, HashSet<string>> oldEmojiItems = JsonConvert.DeserializeObject<SortedDictionary<string, HashSet<string>>>(oldFile);

            Console.WriteLine("Getting data from " + EmojiURL);
            string requestResult = await GetResponseBody(EmojiURL);

            Console.WriteLine("Parsing data");
            List<InputEmojiItem> input = ParseInput(requestResult);

            Console.WriteLine("Creating data structure");
            // create list based on likelyhood that emoji fits
            SortedDictionary<string, HashSet<string>> recentEmojiItems = ToEmojiWords(input);

            Console.WriteLine("Merge old with new items");
            var merge = new SortedDictionary<string, HashSet<string>>(oldEmojiItems);
            foreach (KeyValuePair<string, HashSet<string>> entry in recentEmojiItems)
            {
                if (!merge.ContainsKey(entry.Key))
                {
                    merge.Add(entry.Key, entry.Value);
                }
            }

            Console.WriteLine("Writing data structure to file");
            string formattedOutput = JsonConvert.SerializeObject(merge, Formatting.Indented);
            System.IO.File.WriteAllText(FilePath + FileName, formattedOutput);

            Console.WriteLine("Done");
        }

        private static async Task<string> GetResponseBody(string url)
        {
            HttpClient client = new HttpClient();
            string requestResult = await client.GetStringAsync(url);
            return requestResult;
        }

        public static SortedDictionary<string, HashSet<string>> ToEmojiWords(List<InputEmojiItem> input)
        {
            var result = new Dictionary<string, HashSet<string>>();

            foreach (InputEmojiItem item in input)
            {
                AddItem(result, item.Emoji, item.Aliases.ToArray());
                AddItem(result, item.Emoji, item.Tags.ToArray());
            }

            return new SortedDictionary<string, HashSet<string>>(result);
        }

        private static void AddItem(Dictionary<string, HashSet<string>> result, string emoji, params string[] items)
        {
            if(result == null) { 
                result = new Dictionary<string, HashSet<string>>();
            }

            // add tags
            foreach (string item in items)
            {
                var key = item.Replace("_", " ");

                if (!result.ContainsKey(key))
                {
                    result.Add(key, new HashSet<string>());
                }

                result[key].Add(emoji);
            }
        }

        public static List<InputEmojiItem> ParseInput(string inputString)
        {
            return JsonConvert.DeserializeObject<List<InputEmojiItem>>(inputString);
        }
    }
}
