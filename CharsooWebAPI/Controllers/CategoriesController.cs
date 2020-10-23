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
    [RoutePrefix("api/Categories")]
    public class CategoriesController : ApiController
    {
        private charsoog_DBEntities db = new charsoog_DBEntities();

        #region GET

        // GET: api/Categories
        [ResponseType(typeof(List<Category>))]
        public IHttpActionResult GetCategories()
        {
            var categories = db.Categories.ToList();
            return Ok(categories);
        }

        // GET: api/Categories
        [Route("Updates")]
        public List<Category> GetCategoryUpdates(DateTime recent)
        {
            recent = recent.AddMilliseconds(999);
            var categoryUpdates = db.Categories.Where(c => c.LastUpdate > recent).ToList();

            return categoryUpdates;
        }


        // GET: api/Categories/5
        [ResponseType(typeof(Category))]
        public IHttpActionResult GetCategory(int id)
        {
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        #endregion

        #region POST

        #region Update

        // POST: api/Categories/5
        [ResponseType(typeof(void)), HttpPost, Route("Update/{id}")]
        public IHttpActionResult UpdateCategory(int id, Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != category.ID)
            {
                return BadRequest();
            }

            category.LastUpdate = DateTime.Now;

            db.Entry(category).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
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

        // POST: api/Categories
        [ResponseType(typeof(Category)), HttpPost, Route("Create")]
        public IHttpActionResult CreateCategory(Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Categories.Add(category);
            db.SaveChanges();

            //return CreatedAtRoute("DefaultApi", new { id = category.ID }, category);
            return Ok(category);
        }

        #endregion

        #region Delete

        // POST: api/Categories/5
        [ResponseType(typeof(Category)), HttpPost, Route("Delete/{id}")]
        public IHttpActionResult DeleteCategory(int id)
        {
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            db.Categories.Remove(category);
            db.SaveChanges();

            return Ok(category);
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

        private bool CategoryExists(int id)
        {
            return db.Categories.Count(e => e.ID == id) > 0;
        }

        #endregion


    }
}