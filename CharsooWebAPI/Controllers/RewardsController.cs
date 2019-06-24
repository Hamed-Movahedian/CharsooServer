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
    public class RewardsController : ApiController
    {
        private charsoog_DBEntities db = new charsoog_DBEntities();

        #region DefaultMethods

        // GET: api/Rewards
        public IQueryable<Reward> GetRewards()
        {
            return db.Rewards;
        }

        // GET: api/Rewards/5
        [ResponseType(typeof(Reward))]
        public IHttpActionResult GetReward(int id)
        {
            Reward reward = db.Rewards.Find(id);
            if (reward == null)
            {
                return NotFound();
            }

            return Ok(reward);
        }

        // PUT: api/Rewards/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutReward(int id, Reward reward)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != reward.RewardID)
            {
                return BadRequest();
            }

            db.Entry(reward).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RewardExists(id))
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

        // POST: api/Rewards
        [ResponseType(typeof(Reward))]
        public IHttpActionResult PostReward(Reward reward)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Rewards.Add(reward);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (RewardExists(reward.RewardID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = reward.RewardID }, reward);
        }

        // DELETE: api/Rewards/5
        [ResponseType(typeof(Reward))]
        public IHttpActionResult DeleteReward(int id)
        {
            Reward reward = db.Rewards.Find(id);
            if (reward == null)
            {
                return NotFound();
            }

            db.Rewards.Remove(reward);
            db.SaveChanges();

            return Ok(reward);
        }


        #endregion


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RewardExists(int id)
        {
            return db.Rewards.Count(e => e.RewardID == id) > 0;
        }
    }
}