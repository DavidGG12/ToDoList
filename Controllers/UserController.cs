using SQLFactory;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Helpers;
using ToDoList.Models;
using ToDoList.Models.Request;

namespace ToDoList.Controllers
{
    public class UserController : ControllerBase
    {
        protected static IConfiguration _config;
        protected static DataService dt;
        protected static JWT jwt;

        protected static string conn;
        public UserController(IConfiguration config)
        {
            _config = config;
            dt = new DataService();
            jwt = new JWT(_config["AppSettings:tokenKey"].ToString());
            conn = _config.GetConnectionString("Cnn");
        }

        [HttpPost("register")]
        public ActionResult register(rq_PostRegister_Model rgstrUser)
        {
            var infoUser = new rq_PostLogin_Model()
            {
                email = rgstrUser.EmailUser,
                password = rgstrUser.PassUser
            };
            var token = jwt.generateToken(infoUser);
            var parameters = new Dictionary<string, object>()
            {
                { "@NUser", rgstrUser.NUser },
                { "@LNPUser", rgstrUser.LNPUser },
                { "@LNMUser", rgstrUser.LNMUser },
                { "@EmailUser", rgstrUser.EmailUser },
                { "@PassUser", rgstrUser.PassUser },
                { "@Token", token }
            };
            var execute = dt.GetData<rslt_Execute_SP_Model>(conn, "sp_TDA_InUser", parameters);

            if (string.Equals(execute.Result, "REGISTRADO"))
                return Ok(token);
            else
                return BadRequest(execute.Result);
        }

        [HttpPost("login")]
        public ActionResult login(rq_PostLogin_Model loginUser)
        {
            var token = jwt.generateToken(loginUser);
            var parameters = new Dictionary<string, object>()
            {
                { "@emailUser", loginUser.email },
                { "@pass", loginUser.password },
                { "@token", token }
            };
            var execute = dt.GetData<rslt_Execute_SP_Model>(conn, "sp_TDA_SelUser", parameters);

            switch(execute.Result)
            {
                case "ENCONTRADO":
                    return Ok(token);

                case "NO EXISTE":
                    return NotFound("USUARIO NO ENCONTRADO");

                default:
                    return BadRequest(execute.Result);
            }
        }
    }
}
