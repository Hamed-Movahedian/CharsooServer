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
    [RoutePrefix("api/PuzzleRates")]
    public class PuzzleRatesController : ApiController
    {
        private charsoog_DBEntities _db = new charsoog_DBEntities();


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


            PuzzleRate pr = _db.PuzzleRates.Where(e => e.PlayerID == playerID).FirstOrDefault(e => e.PuzzleID == puzzleID);

            if (pr != null)
            {
                if (pr.Rate == (int)star) return Ok("Success");
                _db.Entry(pr).State = EntityState.Modified;
            }
            else
                _db.PuzzleRates.Add(new PuzzleRate
                {
                    PlayerID = playerID,
                    PuzzleID = puzzleID,
                    Rate = (int)star
                }
                );

            userPuzzle.LastUpdate = DateTime.Now;
            _db.Entry(userPuzzle).State = EntityState.Modified;


            try
            {
                _db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PuzzleRateExists(playerID, puzzleID))
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

        private bool PuzzleRateExists(int playerId, int puzzleId)
        {
            return _db.PuzzleRates.Count(e => e.PlayerID == playerId && e.PuzzleID == puzzleId) > 0;
        }



        /*

                // GET: api/PuzzleRates
                public IQueryable<PuzzleRate> GetPuzzleRates()
                {
                    return db.PuzzleRates;
                }

                // GET: api/PuzzleRates/5
                [ResponseType(typeof(PuzzleRate))]
                public IHttpActionResult GetPuzzleRate(int id)
                {
                    PuzzleRate puzzleRate = db.PuzzleRates.Find(id);
                    if (puzzleRate == null)
                    {
                        return NotFound();
                    }

                    return Ok(puzzleRate);
                }

                // PUT: api/PuzzleRates/5
                [ResponseType(typeof(void))]
                public IHttpActionResult PutPuzzleRate(int id, PuzzleRate puzzleRate)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    if (id != puzzleRate.PuzzleID)
                    {
                        return BadRequest();
                    }

                    db.Entry(puzzleRate).State = EntityState.Modified;

                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!PuzzleRateExists(id))
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

                // POST: api/PuzzleRates
                [ResponseType(typeof(PuzzleRate))]
                public IHttpActionResult PostPuzzleRate(PuzzleRate puzzleRate)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    db.PuzzleRates.Add(puzzleRate);

                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbUpdateException)
                    {
                        if (PuzzleRateExists(puzzleRate.PuzzleID))
                        {
                            return Conflict();
                        }
                        else
                        {
                            throw;
                        }
                    }

                    return CreatedAtRoute("DefaultApi", new { id = puzzleRate.PuzzleID }, puzzleRate);
                }

                // DELETE: api/PuzzleRates/5
                [ResponseType(typeof(PuzzleRate))]
                public IHttpActionResult DeletePuzzleRate(int id)
                {
                    PuzzleRate puzzleRate = db.PuzzleRates.Find(id);
                    if (puzzleRate == null)
                    {
                        return NotFound();
                    }

                    db.PuzzleRates.Remove(puzzleRate);
                    db.SaveChanges();

                    return Ok(puzzleRate);
                }

                protected override void Dispose(bool disposing)
                {
                    if (disposing)
                    {
                        db.Dispose();
                    }
                    base.Dispose(disposing);
                }


        */
    }
}