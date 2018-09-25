using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using CharsooWebAPI.Models;

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