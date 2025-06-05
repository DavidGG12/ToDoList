using Newtonsoft.Json;
using System.Text.Json;

namespace ToDoList.Models.Reponse
{
    public class rp_GetLogin_Model
    {
        public string status { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string TokenUser { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string message { get; set; }
    }
}
