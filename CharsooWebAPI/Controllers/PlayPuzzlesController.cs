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
    [RoutePrefix("api/PlayPuzzles")]
    public class PlayPuzzlesController : ApiController
    {
        private charsoog_DBEntities db = new charsoog_DBEntities();

        [Route("GetPlayerHistory"), HttpPost, ResponseType(typeof(List<PlayPuzzle>))]
        public IHttpActionResult GetPlayerHistory(int playerID)
        {
            var playPuzzles = db
                .PlayPuzzles
                .Where(p => p.PlayerID == playerID).ToList();

            return Ok(playPuzzles);
        }

        [Route("AddHistory"), HttpPost, ResponseType(typeof(string))]
        public IHttpActionResult AddHistory(List<PlayPuzzle> history)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var addList = new List<PlayPuzzle>();

            foreach (var playPuzzle in history)
            {
                if(db.PlayPuzzles.Find(playPuzzle.PlayerID,playPuzzle.PuzzleID,playPuzzle.Time)==null)
                    addList.Add(playPuzzle);
            }

            db.PlayPuzzles.AddRange(addList);

            try
            {
                db.SaveChanges();
            }
            catch (Exception)
            {
                return Ok("Fail");
            }

            return Ok("Success");
        }

        #region Tools
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PlayPuzzleExists(int id)
        {
            return db.PlayPuzzles.Count(e => e.PlayerID == id) > 0;
        } 
        #endregion
    }
}