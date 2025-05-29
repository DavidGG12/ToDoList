using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SQLFactory;
using System.Runtime.CompilerServices;
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
        [HttpGet("getTasks")]
        public ActionResult getTasks([FromQuery] string idTask = "")
        {
            try
            {
                var header = HttpContext.Request.Headers;
                header.TryGetValue("idUser", out var idUser);

                var parameters = new Dictionary<string, object>()
                {
                    { "@idUser", idUser },
                    { "@idTask", idTask.Trim() }
                };
                var resultado = dt.GetData<TblAPI_TD_Tasks_Model>(conn, "sp_TDA_SelTask", parameters);

                if (resultado != null)
                    return Ok(resultado);
                return NoContent();
            }
            catch (SqlException exSQL)
            {
                return BadRequest(new { mensaje = exSQL.Message });
            }
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

                return BadRequest(new { mensaje = "No se ha podido actualizar la tarea." });
            }
            catch (SqlException exSQL)
            {
                return BadRequest(new { mensaje = exSQL.Message });
            }
        }

        [Authorize]
        [AcceptedIdTask(true)]
        [AcceptedIdUser(true)]
        [HttpDelete("deleteTask")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult deleteTask()
        {
            try
            {
                var header = HttpContext.Request.Headers;
                header.TryGetValue("idTask", out var idTask);
                header.TryGetValue("idUser", out var idUser);

                if (string.IsNullOrEmpty(idTask.ToString().Trim()))
                    return UnprocessableEntity(new { mensaje = "No se ingresó el ID de la tarea." });
                if (string.IsNullOrEmpty(idUser.ToString().Trim()))
                    return UnprocessableEntity(new { mensaje = "No se ingresó el ID del usuario." });

                var parameters = new Dictionary<string, object>()
                {
                    { "@idTask", idTask.ToString() },
                    { "@idUser", idUser.ToString() }
                };
                var resultado = dt.GetData<rslt_Execute_SP_Model>(conn, "sp_TDA_DelTask", parameters);
                if(resultado != null && string.Equals(resultado.Result, "ELIMINADO"))
                    return NoContent();

                return BadRequest(new { mensaje = "No se ha podido eliminar la tarea" });
            }
            catch (SqlException exSQL)
            {
                return BadRequest(new { mensaje = exSQL.Message });
            }
        }
    }
}
