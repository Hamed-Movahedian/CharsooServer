using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Description;
using CharsooWebAPI.Models;

namespace CharsooWebAPI.Controllers
{
    [RoutePrefix("api/PlayerInfo")]

    public class PlayerInfoController : ApiController
    {
        private charsoog_DBEntities db = new charsoog_DBEntities();

        #region Get

        // GET: api/PlayerInfo
        public IQueryable<PlayerInfo> GetPlayerInfoes()
        {
            return db.PlayerInfoes;
        }

        // GET: api/PlayerInfo/5
        [ResponseType(typeof(PlayerInfo))]
        public IHttpActionResult GetPlayerInfo(int id)
        {
            PlayerInfo playerInfo = db.PlayerInfoes.Find(id);
            if (playerInfo == null)
            {
                return NotFound();
            }

            return Ok(playerInfo);
        }
        #endregion

        #region Update

        // PUT: api/PlayerInfo/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPlayerInfo(int id, PlayerInfo playerInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != playerInfo.PlayerID)
            {
                return BadRequest();
            }

            db.Entry(playerInfo).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlayerInfoExists(id))
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
        #endregion

        #region Create

        // POST: api/PlayerInfo/Create
        [ResponseType(typeof(PlayerInfo)), HttpPost, Route("Create")]
        public IHttpActionResult CreatePlayerInfo(PlayerInfo playerInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (playerInfo == null)
                return BadRequest("PlayerInfo is null!!!");

            db.PlayerInfoes.Add(playerInfo);
            db.SaveChanges();

            int count = db.PlayerInfoes.Count();

            return Ok(playerInfo); //CreatedAtRoute("DefaultApi", new { id = playerInfo.PlayerID }, playerInfo);
        }

        #endregion

        #region Delete

        // DELETE: api/PlayerInfo/5
        [ResponseType(typeof(PlayerInfo))]
        public IHttpActionResult DeletePlayerInfo(int id)
        {
            PlayerInfo playerInfo = db.PlayerInfoes.Find(id);
            if (playerInfo == null)
            {
                return NotFound();
            }

            db.PlayerInfoes.Remove(playerInfo);
            db.SaveChanges();

            return Ok(playerInfo);
        }

        #endregion

        #region Auxilary

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PlayerInfoExists(int id)
        {
            return db.PlayerInfoes.Count(e => e.PlayerID == id) > 0;
        }

        #endregion

    }
}