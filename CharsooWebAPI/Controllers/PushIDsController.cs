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
    [RoutePrefix("api/PushIDs")]
    public class PushIDsController : ApiController
    {
        private charsoog_DBEntities db = new charsoog_DBEntities();

        // GET: api/PushIDs
        public IQueryable<PushID> GetPushIDs()
        {
            return db.PushIDs;
        }

        // GET: api/PushIDs/5
        [ResponseType(typeof(PushID))]
        public IHttpActionResult GetPushID(string id)
        {
            PushID pushID = db.PushIDs.Find(id);
            if (pushID == null)
            {
                return NotFound();
            }

            return Ok(pushID);
        }

        // PUT: api/PushIDs/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPushID(string id, PushID pushID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pushID.PID)
            {
                return BadRequest();
            }

            db.Entry(pushID).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PushIDExists(pushID.PlayerID,id))
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

        // POST: api/PushIDs
        [ResponseType(typeof(string)), HttpPost, Route("Update")]
        public IHttpActionResult AddPushID(int playerID, [FromBody] string pusheID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PushID o = db.PushIDs.FirstOrDefault(p=>p.PID==pusheID);

            if (o!=null)
            {
                if (o.PlayerID == playerID)
                    return Ok("Success");

                o.PlayerID = playerID;

                db.Entry(o).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PushIDExists(playerID, pusheID))
                        return Ok("Fail");

                    throw;
                }

                return Ok("Success");
            }

            PushID pushID = new PushID()
            {
                PlayerID = playerID,
                PID = pusheID,
                PlayerInfo = db.PlayerInfoes.FirstOrDefault(pi => pi.PlayerID == playerID)
            };

            db.PushIDs.Add(pushID);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (PushIDExists(playerID, pusheID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Ok("Success");
        }

        // DELETE: api/PushIDs/5
        [ResponseType(typeof(PushID))]
        public IHttpActionResult DeletePushID(string id)
        {
            PushID pushID = db.PushIDs.Find(id);
            if (pushID == null)
            {
                return NotFound();
            }

            db.PushIDs.Remove(pushID);
            db.SaveChanges();

            return Ok(pushID);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PushIDExists(int pid, string id)
        {
            return db.PushIDs.Count(e => e.PID == id && e.PlayerID == pid) > 0;
        }

    }
}