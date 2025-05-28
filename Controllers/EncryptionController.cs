using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Helpers;
using ToDoList.Models.Reponse;

namespace ToDoList.Controllers
{
    [ApiController]
    [Route("API/Encryption")]
    public class EncryptionController : ControllerBase
    {
        [Authorize]
        [HttpGet("encryption")]
        public ActionResult encryption(string word)
        {
            var rp = new rp_GetLogin_Model();
            Cryptho cry = new Cryptho();
            
            rp.status = "Success";
            rp.message = cry.Encrypt(word);

            return Ok(rp);
        }

        [Authorize]
        [HttpGet("decryption")]
        public ActionResult decryption(string word)
        {
            var rp = new rp_GetLogin_Model();
            Cryptho cry = new Cryptho();

            rp.status = "Success";
            rp.message = cry.Decrypt(word);
            
            return Ok(rp);
        }
    }
}
