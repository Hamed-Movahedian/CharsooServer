using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using CharsooWebAPI.Models;
using Newtonsoft.Json.Linq;

namespace CharsooWebAPI.Controllers
{
    [RoutePrefix("api/Puzzles")]

    public class PuzzlesController : ApiController
    {
        private charsoog_DBEntities db = new charsoog_DBEntities();

        #region GET

        // GET: api/Puzzles
        public IQueryable<Puzzle> GetPuzzles()
        {
            return db.Puzzles;
        }

        [Route("Updates")]
        public List<Puzzle> GetPuzzlesUpdates(DateTime recent)
        {
            recent = recent.AddMilliseconds(999);

            var puzzleUpdates = db.Puzzles.Where(c => c.LastUpdate > recent).ToList();

            return puzzleUpdates;
        }

        // GET: api/Puzzles/5
        [ResponseType(typeof(Puzzle))]
        public IHttpActionResult GetPuzzle(int id)
        {
            Puzzle puzzle = db.Puzzles.Find(id);
            if (puzzle == null)
            {
                return NotFound();
            }

            return Ok(puzzle);
        }

        #endregion

        #region POST

        #region Update

        // PUT: api/Puzzles/5
        [ResponseType(typeof(void)), HttpPost, Route("Update/{id}")]
        public async Task<IHttpActionResult> UpdatePuzzle(int id, Puzzle puzzle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != puzzle.ID)
            {
                return BadRequest();
            }
            puzzle.LastUpdate = DateTime.Now;

            db.Entry(puzzle).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PuzzleExists(id))
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

        private static string _token = "587ebf53ba61daa4124a9ccc2e780b64d0aecc73";
        private static string _title = "بروز رسانی جدول";
        private static string _message = "جدول ها بروزرسانی شد";
        private static string _pushUrl = "http://api.pushe.co/v2/messaging/notifications/";

        [ResponseType(typeof(void)), HttpGet, Route("Send")]
        public async Task<IHttpActionResult> SendNotification()
        {
            HttpClient client = new HttpClient();

            var content = JObject.FromObject(new
            {
                app_ids = new string[] { "com.Matarsak.charsoo" },
                /*                filters=JObject.FromObject(new
                                {
                                   device_id=new string[]
                                   {
                                       "pid_db03-3d22-31"
                                   } 
                                }),*/
                data = JObject.FromObject(new
                {
                    title = _title,
                    content = _message,
                    image = "http://charsoogame.ir/Img/Notif_Icon.jpg"
                })
            });
            var stringContent = new StringContent(content.ToString(), Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", _token);
            var response = await client.PostAsync(_pushUrl, stringContent);

            return StatusCode(HttpStatusCode.NoContent);
        }

        #endregion

        #region Create

        // POST: api/Puzzles
        [ResponseType(typeof(Puzzle)), HttpPost, Route("Create")]
        public IHttpActionResult CreatePuzzle(Puzzle puzzle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            puzzle.LastUpdate = DateTime.Now;
            db.Puzzles.Add(puzzle);

            db.SaveChanges();

            return Ok(puzzle);
        }

        #endregion

        #region Report

        // POST: api/Puzzles
        #region ConnectToAccount

        [ResponseType(typeof(string)), HttpPost, Route("Report")]
        public IHttpActionResult Report(string puzzlePlayer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (puzzlePlayer.Split('/').Length < 2)
                return Ok("Fail");


            return Ok("Success");
        }


        #endregion

        #endregion

        #region Delete

        // DELETE: api/Puzzles/5
        [ResponseType(typeof(Puzzle)), HttpPost, Route("Delete/{id}")]
        public IHttpActionResult DeletePuzzle(int id)
        {
            Puzzle puzzle = db.Puzzles.Find(id);
            if (puzzle == null)
            {
                return NotFound();
            }

            puzzle.LastUpdate = DateTime.Now;
            puzzle.CategoryID = null;

            db.Entry(puzzle).State = EntityState.Modified;
            db.SaveChanges();

            return Ok(puzzle);
        }

        #endregion



        #region Replace

        // PUT: api/Puzzle/
        [ResponseType(typeof(string)), HttpPost, Route("Replace")]
        public IHttpActionResult ReplacePuzzle(int id, [FromBody] Puzzle puzzle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != puzzle.ID)
            {
                return BadRequest();
            }
            puzzle.LastUpdate = DateTime.Now;
            db.Entry(puzzle).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PuzzleExists(id))
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



        #endregion

        #region Tools

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PuzzleExists(int id)
        {
            return db.Puzzles.Count(e => e.ID == id) > 0;
        }

        #endregion

    }
}