using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using TelegramBot.Services;

namespace TelegramBot.Controllers
{
    [ApiController]
    [Route("api/phone/confirm")]
    public class PhoneConfirmController:ControllerBase
    {
        private PhoneConfirmationService confirmationServiceservice;
        public PhoneConfirmController(PhoneConfirmationService _confirmationService)
        {
            confirmationServiceservice= _confirmationService;
        }
        [HttpGet]
        public int Get(string phoneNumber)
        {
            return confirmationServiceservice.CreateConfirmationCode(phoneNumber);
        }
    }
}
