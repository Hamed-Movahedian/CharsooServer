﻿using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Http;
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
        [ResponseType(typeof(string)), HttpPost, Route("Update")]
        public IHttpActionResult UpdatePlayerInfo(int id, [FromBody] PlayerInfo playerInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != playerInfo.PlayerID)
            {
                return BadRequest();
            }
            playerInfo.Dirty = false;
            db.Entry(playerInfo).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlayerInfoExists(id))
                {
                    return Ok("Fail");
                }
                else
                {
                    throw;
                }
            }

            return Ok("Success");
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