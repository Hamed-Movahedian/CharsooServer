using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using CharsooWebAPI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CharsooWebAPI.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private readonly charsoog_DBEntities _db = new charsoog_DBEntities();

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
                return Ok("NotRegister");

            // check phone first char for TEST
            if (phoneNumber[0] == '0')
                return Ok("NoSmsService");

            return Ok("OK");
        }

        #endregion

        #region ConnectToAccount

        [ResponseType(typeof(string)), HttpPost, Route("ConnectToAccount")]
        public IHttpActionResult ConnectToAccount(string phoneNumber, string code)
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
                return Ok("NotRegister");

            // check phone first char for TEST
            if (phoneNumber[0] == '0')
                return Ok("NoSmsService");

            return Ok("OK");
        }


        #endregion

        #region Tools

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
