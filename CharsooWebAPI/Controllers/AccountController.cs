﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using CharsooWebAPI.Models;
using CharsooWebAPI.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CharsooWebAPI.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        #region SendSms

        [FollowMachine("Send SMS", "Repetitive Number,Not Register,Invalid Phone Number,No Sms Service,Success")]
        [ResponseType(typeof(string)), HttpPost, Route("SendSms")]
        public IHttpActionResult SendSms(string phoneNumber, string code, bool forRegister)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Search for players by phone number
            var players = _db.PlayerInfoes
                .Where(pi => pi.Telephone == phoneNumber)
                .ToList();

            // phone number not registered !!!!
            if (forRegister)
            {
                if (players.Count > 0)
                    return Ok("Repetitive Number");
            }
            else if (players.Count == 0)
                return Ok("Not Register");

            var result = SmsService.CallVerifyService(phoneNumber, "CharsooVerify", code);


            // Send sms and get result ( OK or InvalidPhoneNumber of NoSmsService )
            //var result = SmsService.CallSmsService(phoneNumber, $"سلام دوست عزیز به چارسو خوش آمدید\nکد فعال سازی شما : {code}");

            return Ok(result);
        }



        #endregion

        #region RegisterPhoneNumber

        [ResponseType(typeof(PlayerInfo)), HttpPost, Route("RegisterPhoneNumber")]
        public IHttpActionResult RegisterPhoneNumber(int playerID, string phoneNumber)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Search for player by ID
            var player = _db.PlayerInfoes
                .FirstOrDefault(pi => pi.PlayerID == playerID);

            // Player not found !!!!
            if (player == null)
                return NotFound();

            player.Telephone = phoneNumber;

            return Ok(player);
        }



        #endregion

        #region ConnectToAccount

        [ResponseType(typeof(AccountInfo)), HttpPost, Route("ConnectToAccount")]
        public IHttpActionResult ConnectToAccount(string phoneNumber)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var accountInfo = new AccountInfo();

            // ****************** Get player info

            // Search for players by phone number
            var players = _db.PlayerInfoes
                .Where(pi => pi.Telephone == phoneNumber)
                .ToList();

            // phone number not registered !!!!
            if (players.Count == 0)
                return NotFound();

            accountInfo.PlayerInfo = players[0];

            // ********************* get user puzzles
            accountInfo.UserPuzzles = _db
                .UserPuzzles
                .Where(p => p.CreatorID == accountInfo.PlayerInfo.PlayerID)
                .ToList()
                .Select(p =>
                {
                    var rates = _db.PuzzleRates.Where(pr => pr.PuzzleID == p.ID);

                    int? count = null;
                    if (rates.Any()) count = rates.Count();

                    return new ClientUserPuzzle
                    {

                        ServerID = p.ID,
                        CategoryName = p.CategoryID.HasValue ?
                            (p.CategoryID == 2050 ? "-" : _db.Categories.FirstOrDefault(c => c.ID == p.CategoryID.Value)?.Name) : "",
                        Rate = rates.Any() ? (int?)Math.Round(rates.Average(pr => pr.Rate)) : null,
                        PlayCount = count,
                        Content = p.Content,
                        Clue = p.Clue,
                        ID = p.ClientID
                    };
                })
                .ToList();


            // ********************* get purchases
            accountInfo.Purchaseses = _db
                .Purchases
                .Where(p => p.PlayerID == accountInfo.PlayerInfo.PlayerID).ToList();

            // ********************* Get play puzzles
            accountInfo.PlayPuzzleses = _db
                .PlayPuzzles
                .Where(p => p.PlayerID == accountInfo.PlayerInfo.PlayerID).ToList();

            return Ok(accountInfo);
        }


        #endregion

        #region Database Access

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

        #region Account info classes
        public class AccountInfo
        {
            public PlayerInfo PlayerInfo;
            public List<ClientUserPuzzle> UserPuzzles;
            public List<PlayPuzzle> PlayPuzzleses;
            public List<Purchase> Purchaseses;
        }
        public class ClientUserPuzzle
        {
            public int ID { get; set; }
            public int? ServerID { get; set; }
            public string Clue { get; set; }
            public string Content { get; set; }
            public int? Rate { get; set; }
            public int? PlayCount { get; set; }
            public string CategoryName { get; set; }
        }
        #endregion
    }

}
