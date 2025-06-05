using System.Text.Json.Serialization;

namespace ToDoList.Models.Request
{
    public class rq_PostTask_Model
    {
        public string idUsuario {  get; set; }
        public string title { get; set; }
        public string description { get; set; }
    }
}
