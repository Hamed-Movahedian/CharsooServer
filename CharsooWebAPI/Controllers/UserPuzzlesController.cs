using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
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
        [ResponseType(typeof(string)), HttpPost, Route("GetInviteData")]
        public IHttpActionResult GetInviteData(int puzzleID, int senderID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var userPuzzle = _db.UserPuzzles.FirstOrDefault(up => up.ID == puzzleID);
            var senderInfo = _db.PlayerInfoes.FirstOrDefault(pi => pi.PlayerID == senderID);

            if (userPuzzle == null || senderInfo == null)
                return InternalServerError();

            var creatorInfo = _db.PlayerInfoes.FirstOrDefault(pi => pi.PlayerID == userPuzzle.CreatorID);

            var result = new JObject()
            {
                ["Clue"] = userPuzzle.Clue,
                ["Creator"] = creatorInfo == null ? "Unknown" : creatorInfo.Name,
                ["CreatorID"] = creatorInfo?.PlayerID.ToString() ?? "",
                ["Content"] = userPuzzle.Content,
                ["Sender"] = senderInfo.Name
            };

            return Ok(result.ToString());
        }

        [ResponseType(typeof(string)), HttpPost, Route("RegisterFeedback")]
        public IHttpActionResult RegisterFeedback(int puzzleID, int playerID, float star)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var userPuzzle = _db.UserPuzzles.FirstOrDefault(up => up.ID == puzzleID);
            var senderInfo = _db.PlayerInfoes.FirstOrDefault(pi => pi.PlayerID == playerID);

            if (userPuzzle == null || senderInfo == null)
                return InternalServerError();



            return Ok("Success");
        }

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
                    existingRecord.LastUpdate = DateTime.Now;

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
                    p =>
                    {
                        var rates = _db.PuzzleRates.Where(pr => pr.PuzzleID == p.ID);

                        int? count = null;
                        if (rates.Any()) count = rates.Count();

                        outData.UpdatedPuzzles.Add(new OutData.PuzzleUpdate
                        {
                            ServerID = p.ID,
                            CategoryName = p.CategoryID.HasValue ?
                            (p.CategoryID == 2050 ? "-" : _db.Categories.FirstOrDefault(c => c.ID == p.CategoryID.Value)?.Name) : "",
                            Rate = rates.Any() ? (int?)Math.Round(rates.Average(pr => pr.Rate)) : null,
                            PlayCount = count,
                            Content = p.Content,
                            Clue = p.Clue,
                            ID = p.ClientID
                        });
                    });

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

        [ResponseType(typeof(List<UserPuzzle>))]
        public IHttpActionResult GetNewPuzzles()
        {
            List<UserPuzzle> newPuzzles = new List<UserPuzzle>();
            newPuzzles.AddRange(_db.UserPuzzles.Where(p => p.CategoryID == null));
            return Ok(newPuzzles);
        }

        [ResponseType(typeof(string)), HttpPost, Route("SetCategory")]
        public IHttpActionResult SetCategory(int puzzleID, int category)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            UserPuzzle userPuzzle = _db.UserPuzzles.FirstOrDefault(p => p.ID == puzzleID);
            if (userPuzzle == null) return BadRequest();
            userPuzzle.CategoryID = category;
            userPuzzle.LastUpdate = DateTime.Now;

            _db.Entry(userPuzzle).State = EntityState.Modified;

            try
            {
                _db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return Ok("Failed");
            }

            return Ok("Success");
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
