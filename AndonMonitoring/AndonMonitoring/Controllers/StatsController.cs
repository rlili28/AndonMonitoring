using AndonMonitoring.QueryBuilder;
using AndonMonitoring.Services.Interfaces;
using AndonMonitoring.AndonExceptions;

using Microsoft.AspNetCore.Mvc;

namespace AndonMonitoring.Controllers
{
    [ApiController]
    [Route("Stats")]
    public class StatsController : ControllerBase
    {
        private readonly IStatsService service;
        public StatsController(IStatsService statsService)
        {
            service = statsService;
        }

        /// <summary>
        /// Endpoint to get the total number of minutes that an Andon light was in a specific state on a given day.
        /// </summary>
        /// <param name="lightId">id of the Andon light</param>
        /// <param name="stateId">id of the state</param>
        /// <param name="dayValue">Date of the day</param> //TODO: format is supposed to be something parseable by DateTime.TryParse()
        /// <returns>The total number of minutes that the Andon light was in the specified state on the specified day</returns>
        [HttpGet("getDayMinutes")]
        public ActionResult<int> GetAndonStateMinutesByDay(int lightId, int stateId, string dayValue)
        {
            if(lightId < 0 || stateId < 0)
            {
                return BadRequest();
            }

            DateTime day;
            if(!DateTime.TryParse(dayValue, out day)) { return BadRequest(); }

            StatQuery param = new StatQueryBuilder().WithAndon(lightId).WithState(stateId).OnDay(day).Build();
            int minute;
            try
            {
                minute = service.GetAndonStateMinutesByDay(param);
            }
            catch(AndonFormatException e)
            {
                return BadRequest(e.Message);
            }

            return minute;
        }

        /// <summary>
        /// Endpoint to get the total number of minutes that an Andon light was in a specific state on a given month.
        /// </summary>
        /// <param name="lightId">id of the Andon light</param>
        /// <param name="stateId">id of the state</param>
        /// <param name="monthValue">Date of the month</param>
        /// <returns>The total number of minutes that the Andon light was in the specified state on the specified month</returns>
        [HttpGet("getMonthMinutes")]
        public ActionResult<int> GetAndonStateMinutesByMonth(int lightId, int stateId, string monthValue)
        {
            if (lightId < 0 || stateId < 0)
            {
                return BadRequest();
            }

            DateTime month;
            if (!DateTime.TryParse(monthValue, out month)) { return BadRequest(); }

            StatQuery param = new StatQueryBuilder().WithAndon(lightId).WithState(stateId).OnMonth(month).Build();

            int minute;
            try
            {
                minute = service.GetAndonStateMinutesByMonth(param);
            }
            catch (AndonFormatException e)
            {
                return BadRequest(e.Message);
            }

            return minute;
        }

        /// <summary>
        /// Endpoint to get the number of times that an Andon light was in a specific state on a given day.
        /// </summary>
        /// <param name="lightId">id of the andon light</param>
        /// <param name="stateId">id of the state</param>
        /// <param name="dayValue">Date of the day</param>
        /// <returns>The number of times that the Andon light was in the specified state on the specified day</returns>
        [HttpGet("getDailyCount")]
        public ActionResult<int> GetAndonStateCountByDay(int lightId, int stateId, string dayValue)
        {
            if (lightId < 0 || stateId < 0)
            {
                return BadRequest();
            }

            DateTime day;
            if (!DateTime.TryParse(dayValue, out day)) { return BadRequest(); }

            StatQuery param = new StatQueryBuilder().WithAndon(lightId).WithState(stateId).OnDay(day).Build();

            int count = 0;
            try
            {
                count = service.GetAndonStateCountByDay(param);
            }
            catch (AndonFormatException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"{e.Message}");
            }

            return count;
        }

        /// <summary>
        /// Endpoint to get the number of times that an Andon light was in a specific state on a given month.
        /// </summary>
        /// <param name="lightId">id of the andon light</param>
        /// <param name="stateId">id of the state</param>
        /// <param name="monthValue">Date of the month</param>
        /// <returns>The number of times that the Andon light was in the specified state on the specified month.</returns>
        [HttpGet("getMonthylCount")]
        public ActionResult<int> GetAndonStateCountByMonth(int lightId, int stateId, string monthValue)
        {
            if (lightId < 0 || stateId < 0)
            {
                return BadRequest();
            }

            DateTime month;
            if (!DateTime.TryParse(monthValue, out month)) { return BadRequest(); }

            StatQuery param = new StatQueryBuilder().WithAndon(lightId).WithState(stateId).OnMonth(month).Build();

            int count;
            try
            {
                count = service.GetAndonStateCountByMonth(param);
            }
            catch (AndonFormatException e)
            {
                return BadRequest(e.Message);
            }

            return count;
        }
    }
}
