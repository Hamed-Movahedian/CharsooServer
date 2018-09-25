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
            var purchases = db
                .PlayPuzzles
                .Where(p => p.PlayerID == playerID).ToList();

            return Ok(purchases);
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