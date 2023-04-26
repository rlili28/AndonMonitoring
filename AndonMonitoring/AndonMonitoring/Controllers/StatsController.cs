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

        [HttpGet("dayMinutes")]
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

        [HttpGet("monthMinutes")]
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

        [HttpGet("dailyCount")]
        public ActionResult<int> GetAndonStateCountByDay([FromQuery]int lightId, [FromQuery]int stateId, [FromQuery]string dayValue)
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

        [HttpGet("monthylCount")]
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
