using System;
using System.Collections;
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
    [RoutePrefix("api/UserPuzzles")]
    public class UserPuzzlesController : ApiController
    {

        [ResponseType(typeof(OutData)), HttpPost, Route("Sync")]
        public IHttpActionResult GetRecentCommands(InData inData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var outData = new OutData
            {
                LastUpdate = DateTime.Now,
                NewPuzzles = AddNewPuzzles(inData),
                UpdatedPuzzles = GetUpdatedPuzzles(inData)
            };

            return Ok(outData);
        }

        private List<OutData.PuzzleUpdate> GetUpdatedPuzzles(InData inData)
        {
            var results = new List<OutData.PuzzleUpdate>();

            _db.Puzzles
                .Where(p => p.CreatorID == inData.PlayerID && p.LastUpdate > inData.LastUpdate)
                .ToList()
                .ForEach(
                    p => results.Add(new OutData.PuzzleUpdate
                    {
                        ServerID = p.ID,
                        CategoryName = p.Category.Name,
                        Rate = p.Rate,
                        PlayCount = p.PlayCount
                    }));

            return results;
        }

        private List<OutData.NewPuzzle> AddNewPuzzles(InData inData)
        {
            var newPuzzles = new List<OutData.NewPuzzle>();

            foreach (var cPuzzle in inData.NewPuzzles)
            {
                var sPuzzle = new Puzzle()
                {
                    Clue = cPuzzle.Clue,
                    Content = cPuzzle.Content,
                    CreatorID = inData.PlayerID,
                    LastUpdate = DateTime.Now
                };
                sPuzzle = _db.Puzzles.Add(sPuzzle);
                newPuzzles.Add(new OutData.NewPuzzle { ID = cPuzzle.ID, ServerID = sPuzzle.ID });
            }

            return newPuzzles;
        }

        public class OutData
        {
            public DateTime LastUpdate { get; set; }
            public List<NewPuzzle> NewPuzzles { get; set; }
            public List<PuzzleUpdate> UpdatedPuzzles { get; set; }

            public class PuzzleUpdate
            {
                public int ServerID { get; set; }
                public string CategoryName { get; set; }
                public int? Rate { get; set; }
                public int? PlayCount { get; set; }
            }

            public class NewPuzzle
            {
                public int ServerID { get; set; }
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
