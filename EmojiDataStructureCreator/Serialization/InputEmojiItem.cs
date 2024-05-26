using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmojiDataStructureCreator.Serialization
{
    public class InputEmojiItem
    {
        [JsonProperty(PropertyName = "emoji")]
        public string Emoji { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "category")]
        public string Category { get; set; }

        [JsonProperty(PropertyName = "aliases")]
        public List<string> Aliases { get; set; } = new List<string>();

        [JsonProperty(PropertyName = "tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonProperty(PropertyName = "unicode_version")]
        public string UnicodeVersion { get; set; }

        [JsonProperty(PropertyName = "ios_version")]
        public string IOSVersion { get; set; }

        [JsonProperty(PropertyName = "skin_tones")]
        public bool SkinTones { get; set; }

    }
}
