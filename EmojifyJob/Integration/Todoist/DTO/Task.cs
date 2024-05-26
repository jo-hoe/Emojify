using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmojifyFunctions.Integration.Todoist.DTO
{
    public sealed class Task
    {
        [JsonProperty(PropertyName = "id")]
        public long? Id { get; set; }

        [JsonProperty(PropertyName = "content")]
        public string Content { get; set; }
    }
}
