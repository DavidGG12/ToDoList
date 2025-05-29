using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SQLFactory;
using ToDoList.Helpers;
using ToDoList.Helpers.Attributes;
using ToDoList.Models;
using ToDoList.Models.Reponse;
using ToDoList.Models.Request;

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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult registerTask([FromBody]rq_PostTask_Model rq)
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
                    return Ok(new { resultado.idTasks, resultado.TitleTasks, resultado.DescriptionTasks });

                return BadRequest(new { mensaje = "No se ha podido registrar la tarea." });
            }
            catch (SqlException exSQL)
            {
                return BadRequest(new { mensaje = exSQL.Message });
            }
        }

        [Authorize]
        [AcceptedIdTask(true)]
        [HttpPut("updateTask")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult updateTask([FromBody]rq_PutTask_Model rq)
        {
            try
            {
                var header = HttpContext.Request.Headers;
                header.TryGetValue("idTask", out var idTask);

                if (string.IsNullOrEmpty(idTask.ToString().Trim()))
                    return UnprocessableEntity(new { mensaje = "Falta ID de la tarea" });

                var parameters = new Dictionary<string, object>
                {
                    { "@idTask", idTask.ToString() },
                    { "@title", rq.title },
                    { "@description", rq.description }
                };
                var resultado = dt.GetData<TblAPI_TD_Tasks_Model>(conn, "sp_TDA_UpTask", parameters);
                if (resultado != null)
                    return Ok(new { resultado.idTasks, resultado.TitleTasks, resultado.DescriptionTasks });

                return BadRequest(new { mensaje = "No se ha podido registrar la tarea." });
            }
            catch (SqlException exSQL)
            {
                return BadRequest(new { mensaje = exSQL.Message });
            }
        }

        [Authorize]
        [AcceptedIdTask(true)]
        [HttpDelete("deleteTask")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult deleteTask()
        {

            return default;
        }
    }
}
