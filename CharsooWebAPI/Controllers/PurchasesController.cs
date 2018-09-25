﻿using System;
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
    [RoutePrefix("api/Purchases")]
    public class PurchasesController : ApiController
    {
        private charsoog_DBEntities db = new charsoog_DBEntities();

        [Route("GetPurchase"),HttpPost,ResponseType(typeof(List<Purchase>))]
        public IHttpActionResult GetPurchase(int playerID,DateTime lastUpdate)
        {
            lastUpdate = lastUpdate.AddMilliseconds(999);

            var purchases = db
                .Purchases
                .Where(p => p.LastUpdate > lastUpdate && p.PlayerID==playerID).ToList();

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

        private bool PurchaseExists(int id)
        {
            return db.Purchases.Count(e => e.PlayerID == id) > 0;
        } 
        #endregion
    }
}