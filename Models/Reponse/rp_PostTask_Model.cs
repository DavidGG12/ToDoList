using System.Text.Json.Serialization;
using ToDoList.Models.Request;

namespace ToDoList.Models.Reponse
{
    public class rp_PostTask_Model
    {
        public string id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string message { get; set; }
    }
}
