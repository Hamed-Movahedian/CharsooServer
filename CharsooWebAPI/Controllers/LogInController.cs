using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using CharsooWebAPI.Models;

namespace CharsooWebAPI.Controllers
{
    [RoutePrefix("api/Login")]
    public class LogInController : ApiController
    {
        private charsoog_DBEntities db = new charsoog_DBEntities();

        // GET: api/LogIn
        public IQueryable<LogIn> GetLogIns()
        {
            return db.LogIns;
        }

        // GET: api/LogIn/5
        [ResponseType(typeof(LogIn))]
        public IHttpActionResult GetLogIn(int id)
        {
            LogIn logIn = db.LogIns.Find(id);
            if (logIn == null)
            {
                return NotFound();
            }

            return Ok(logIn);
        }

        [ResponseType(typeof(PlayerInfo)),HttpGet,Route("RestorePlayerInfo")]
        public IHttpActionResult RestorePlayerInfoByDeviceID(string deviceId)
        {
            if (deviceId == null)
                return BadRequest("DeviceID is null");

            var logIn = db.LogIns.FirstOrDefault(l => l.DeviceID == deviceId);

            if (logIn == null)
                return NotFound();

            int id = logIn.PlayerID;

            PlayerInfo playerInfo = db.PlayerInfoes.Find(id);

            return Ok(playerInfo);
        }

        // PUT: api/LogIn/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutLogIn(int id, LogIn logIn)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != logIn.PlayerID)
            {
                return BadRequest();
            }

            db.Entry(logIn).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LogInExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/LogIn
        [ResponseType(typeof(int)), HttpPost, Route("AddRange")]
        public IHttpActionResult AddLogins(List<LogIn> logIns)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (LogIn logIn in logIns)
            {
                db.LogIns.Add(logIn);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    if (LogInExists(logIn))
                    {
                        return Conflict();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return Ok(logIns.Count);
        }

        // DELETE: api/LogIn/5
        [ResponseType(typeof(LogIn))]
        public IHttpActionResult DeleteLogIn(int id)
        {
            LogIn logIn = db.LogIns.Find(id);
            if (logIn == null)
            {
                return NotFound();
            }

            db.LogIns.Remove(logIn);
            db.SaveChanges();

            return Ok(logIn);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LogInExists(LogIn logIn)
        {
            return db.LogIns.Count(e =>
                        e.PlayerID == logIn.PlayerID &&
                        e.DeviceID == logIn.DeviceID &&
                        e.LoginTime == logIn.LoginTime) > 0;
        }
        private bool LogInExists(int id)
        {
            return db.LogIns.Count(e =>
                        e.PlayerID == id) > 0;
        }
    }
}