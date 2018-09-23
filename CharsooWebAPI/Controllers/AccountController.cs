using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using CharsooWebAPI.Models;
using CharsooWebAPI.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CharsooWebAPI.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        #region SendSms

        [ResponseType(typeof(string)), HttpPost, Route("SendSms")]
        public IHttpActionResult SendSms(string phoneNumber, string code)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Search for players by phone number
            var players = _db.PlayerInfoes
                .Where(pi => pi.Telephone == phoneNumber)
                .ToList();

            // phone number not registered !!!!
            if (players.Count == 0)
                return Ok("Not Register");

            // Send sms and get result ( OK or InvalidPhoneNumber of NoSmsService )
            var result = SmsService.CallSmsService(
                phoneNumber,
                $"سلام دوست عزیز به چهارسو خوش آمدید\nکد فعال سازی شما : {code}");

            return Ok(result);
        }



        #endregion

        #region ConnectToAccount

        [ResponseType(typeof(PlayerInfo)), HttpPost, Route("ConnectToAccount")]
        public IHttpActionResult ConnectToAccount(string phoneNumber)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Search for players by phone number
            var players = _db.PlayerInfoes
                .Where(pi => pi.Telephone == phoneNumber)
                .ToList();

            // phone number not registered !!!!
            if (players.Count == 0)
                return NotFound();

            return Ok(players[0]);
        }


        #endregion

        #region Database Access

        private readonly charsoog_DBEntities _db = new charsoog_DBEntities();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}
