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
        private charsoog_DBEntities db = new charsoog_DBEntities();

        [ResponseType(typeof(string)), HttpPost, Route("GetCommands")]
        public IHttpActionResult GetCommands(int playerID, DateTime clientLastCmdTime)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // increse client last command time
            clientLastCmdTime = clientLastCmdTime.AddMilliseconds(999);

            // Create command array
            JArray commands = new JArray();

            AddNewCategoriesCommand(clientLastCmdTime, commands);

            AddNewPuzzlesCommand(clientLastCmdTime, commands);

            string content = commands.ToString(Formatting.None);

            return Ok(content);
        }

        private void AddNewPuzzlesCommand(DateTime clientLastCmdTime, JArray commands)
        {
            List<Puzzle> newPuzzles =
                db.Puzzles
                    .Where(c => c.LastUpdate > clientLastCmdTime)
                    .ToList();

            if (newPuzzles.Count <= 0) return;

            commands.Add(new JObject
            {
                ["Command"] = "AddPuzzles",
                ["Data"] = new JArray(newPuzzles.Select(JObject.FromObject))
            });
        }

        private void AddNewCategoriesCommand(DateTime clientLastCmdTime, JArray commands)
        {
            List<Category> newCategories =
                db.Categories
                    .Where(c => c.LastUpdate > clientLastCmdTime)
                    .ToList();

            if (newCategories.Count <= 0) return;

            commands.Add(new JObject
            {
                ["Command"] = "AddCategories",
                ["Data"] = new JArray(newCategories.Select(JObject.FromObject))
            });
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

        #endregion
    }


}
