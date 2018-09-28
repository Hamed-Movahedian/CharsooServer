using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
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
    [RoutePrefix("api/UserPuzzles")]
    public class UserPuzzlesController : ApiController
    {

        [ResponseType(typeof(OutData)), HttpPost, Route("Sync")]
        public IHttpActionResult Sync(InData inData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var outData = new OutData();

            outData.LastUpdate = DateTime.Now;

            #region Register new puzzles

            foreach (var cPuzzle in inData.NewPuzzles)
            {
                // Check if new puzzle already exist?
                UserPuzzle existingRecord;

                try
                {
                    existingRecord = _db.UserPuzzles
                        .FirstOrDefault(p => p.CreatorID == inData.PlayerID && p.ClientID == cPuzzle.ID);
                }
                catch (Exception e)
                {
                    return InternalServerError(e);
                }

                if (existingRecord != null)
                {
                    // new puzzle already exist
                    existingRecord.Clue = cPuzzle.Clue;
                    existingRecord.Content = cPuzzle.Content;
                    existingRecord.LastUpdate= DateTime.Now;

                    _db.Entry(existingRecord).State = EntityState.Modified;

                    _db.SaveChanges();
                }
                else
                {
                    // new puzzle not exist and must be added
                    var sPuzzle = new UserPuzzle
                    {
                        ClientID = cPuzzle.ID,
                        CreatorID = inData.PlayerID,
                        Clue = cPuzzle.Clue,
                        Content = cPuzzle.Content,
                        LastUpdate = DateTime.Now
                    };
                    _db.UserPuzzles.Add(sPuzzle);

                    _db.SaveChanges();
                }
            }


            #endregion

            #region Get Recent Updates

            outData.UpdatedPuzzles = new List<OutData.PuzzleUpdate>();

            var userPuzzles = _db.UserPuzzles.ToList();

            var filteredPuzzles = userPuzzles.Where(p => p.CreatorID == inData.PlayerID && p.LastUpdate > inData.LastUpdate)
                .ToList();

            filteredPuzzles.ForEach(
                    p => outData.UpdatedPuzzles.Add(new OutData.PuzzleUpdate
                    {
                        ServerID = p.ID,
                        CategoryName = p.Category?.Name,
                        Rate = p.Rate,
                        PlayCount = p.PlayCount,
                        Content = p.Content,
                        Clue = p.Clue,
                        ID = p.ClientID
                    }));

            #endregion

            return Ok(outData);
        }

        #region Get Userpuzzle

        [ResponseType(typeof(UserPuzzle))]
        public IHttpActionResult GetPlayerInfo(int id)
        {
            UserPuzzle onlinePuzzle = _db.UserPuzzles.Find(id);
            if (onlinePuzzle == null)
            {
                return NotFound();
            }

            return Ok(onlinePuzzle);
        }

        

        #endregion

        #region InData & OutData classes

        public class OutData
        {
            public DateTime LastUpdate { get; set; }
            public List<PuzzleUpdate> UpdatedPuzzles { get; set; }

            public class PuzzleUpdate
            {
                public int ServerID { get; set; }
                public string CategoryName { get; set; }
                public int? Rate { get; set; }
                public int? PlayCount { get; set; }
                public string Content { get; set; }
                public string Clue { get; set; }
                public int ID { get; set; }
            }
        }

        public class InData
        {
            public List<NewPuzzle> NewPuzzles { get; set; }
            public int PlayerID { get; set; }
            public DateTime LastUpdate { get; set; }

            public class NewPuzzle
            {
                public string Clue { get; set; }
                public string Content { get; set; }
                public int ID { get; set; }
            }
        }
        

        #endregion

        #region DB

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
