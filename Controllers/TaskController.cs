using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ToDoList.Helpers;
using ToDoList.Models;
using ToDoList.Models.Reponse;
using ToDoList.Models.Request;
using SQLFactory;

namespace ToDoList.Controllers
{
    public class TaskController : ControllerBase
    {
        protected static IConfiguration _config;
        protected static DataService dt;
        protected static string conn;

        public TaskController(IConfiguration config)
        {
            _config = config;
            dt = new DataService();
            conn = _config.GetConnectionString("Cnn");
        }

        [Authorize]
        [HttpPost("registerTask")]
        public ActionResult registerTask(rq_PostTask_Model rq)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "@idUser", rq.idUsuario },
                    { "@titleTask", rq.title },
                    { "@descriptionTask", rq.description }
                };
                var resultado = dt.GetData<TblAPI_TD_Tasks_Model>(conn, "sp_TDA_InTask", parameters);
                var rp = new rp_PostTask_Model();

                if (resultado != null)
                    return Ok(new { resultado.idTask, resultado.TitleTasks, resultado.DescriptionTasks });

                return BadRequest("No se ha podido registrar la tarea.");
            }
            catch (SqlException exSQL)
            {
                return BadRequest(exSQL.Message);
            }
        }

        [Authorize]
        [HttpPut("updateTask")]
        public ActionResult updateTask()
        {
            try
            {
                
            }
            catch (SqlException sqlError)
            {

            }
        }
    }
}
