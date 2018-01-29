using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using pickuphockey2.Models;

namespace pickuphockey2.Controllers
{
    [Route("api/[controller]")]
    public class ActivityLogController : Controller
    {
        private readonly ActivityLogContext _context;

        public ActivityLogController(ActivityLogContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets activities
        /// </summary>
        /// <remarks>List of activities</remarks>
        /// <response code="200">If there are activities</response>
        /// <response code="204">If there are no activities</response>
        /// <response code="404">If the session is not found</response>
        [HttpGet("[action]")]
        [ProducesResponseType(typeof(IEnumerable<ActivityLog>), 200)]
        [ProducesResponseType(204)]
        public IActionResult Activities(int SessionId)
        {
            // TODO Add code if session not found return NotFound()

            var ret = _context.ActivityLogs.Where(q => q.SessionId == SessionId).ToList();
            if (ret.Count() > 0)
            {
                return new ObjectResult(ret);
            }

            return NoContent();
        }
    }
}
