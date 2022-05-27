using Microsoft.AspNetCore.Mvc;

namespace TelegramBot.Controllers
{
    [ApiController]
    [Route("api/message/update")]
    public class MessagesController : ControllerBase
    {


        [HttpGet]
        //Get api/
        public string Get()
        {
            return "a";
        }
    }
}