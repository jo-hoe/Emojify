using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmojifyFunctions.Integration.MSToDo.DTO
{
    public sealed class Task
    {
        [JsonProperty(PropertyName = "Id")]
        public long Id { get; set; }

        [JsonProperty(PropertyName = "Subject")]
        public string Subject { get; set; }
    }
}
