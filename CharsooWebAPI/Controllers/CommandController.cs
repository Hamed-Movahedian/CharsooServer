using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using CharsooWebAPI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CharsooWebAPI.Controllers
{
    [RoutePrefix("api/Commands")]
    public class CommandController : ApiController
    {
        private readonly charsoog_DBEntities _db = new charsoog_DBEntities();

        #region GetRecentCommands

        [ResponseType(typeof(string)), HttpPost, Route("GetCommands")]
        public IHttpActionResult GetRecentCommands(int playerID, DateTime clientLastCmdTime)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // increase client last command time
            clientLastCmdTime = clientLastCmdTime.AddMilliseconds(999);

            // Create command array
            JArray commands = new JArray();

            AddNewCategoriesCommand(clientLastCmdTime, commands);

            AddNewPuzzlesCommand(clientLastCmdTime, commands);

            string content = commands.ToString(Formatting.None);

            return Ok(content);
        }

        #endregion

        #region AddNewPuzzlesCommand

        private void AddNewPuzzlesCommand(DateTime clientLastCmdTime, JArray commands)
        {
            List<Puzzle> newPuzzles =
                _db.Puzzles
                    .Where(c => c.LastUpdate > clientLastCmdTime)
                    .ToList();

            if (newPuzzles.Count <= 0) return;

            commands.Add(new JObject
            {
                ["Command"] = "AddPuzzles",
                ["Data"] = new JArray(newPuzzles.Select(JObject.FromObject))
            });
        }


        #endregion

        #region AddNewCategoriesCommand

        private void AddNewCategoriesCommand(DateTime clientLastCmdTime, JArray commands)
        {
            List<Category> newCategories =
                _db.Categories
                    .Where(c => c.LastUpdate > clientLastCmdTime)
                    .ToList();

            if (newCategories.Count <= 0) return;

            commands.Add(new JObject
            {
                ["Command"] = "AddCategories",
                ["Data"] = new JArray(newCategories.Select(JObject.FromObject))
            });
        }

        #endregion

        #region ConnectToAccount

        [ResponseType(typeof(string)), HttpPost, Route("ConnectToAccount")]
        public IHttpActionResult ConnectToAccount(string phoneNumber, DateTime lastCommandTime)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // increase client last command time
            lastCommandTime = lastCommandTime.AddMilliseconds(999);

            // Create command array
            JArray commands = new JArray();

            // Get players with this phoneNumber
            var players = _db.PlayerInfoes
                 .Where(pi => pi.Telephone == phoneNumber)
                 .ToList();

            // if no player exist => FAIL
            if (players.Count == 0)
                return Ok("Fail");

            // Add update player info command
            commands.Add(new JObject
            {
                ["Command"] = "UpdatePlayerInfo",
                ["Data"] = JObject.FromObject(players[0])
            });
            
            string content = commands.ToString(Formatting.None);

            return Ok(content);
        }


        #endregion

        #region Tools

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
