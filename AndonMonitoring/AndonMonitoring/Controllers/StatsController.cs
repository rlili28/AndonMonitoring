using AndonMonitoring.QueryBuilder;
using AndonMonitoring.Services;
using Microsoft.AspNetCore.Mvc;

namespace AndonMonitoring.Controllers
{
    [Route("api/Stats")]
    [ApiController]
    public class StatsController : ControllerBase
    {
        private readonly IStatsService service;
        public StatsController(IStatsService statsService)
        {
            service = statsService;
        }

        [HttpGet("{light, state, day}")]
        public ActionResult<int> GetAndonStateMinutesByDay(Data.AndonDto light, Data.StateDto state, DateTime day)
        {
            if(light == null || state == null)
            {
                return BadRequest();
            }
            StatQuery param = new StatQueryBuilder().WithAndon(light.Id).WithState(state.Id).OnDay(day).Build();

            int minute = 0;
            
            minute = service.GetAndonStateMinutesByDay(param);

            return minute;
        }

        [HttpGet]
        public ActionResult<int> GetAndonStateMinutesByMonth(Data.AndonDto light, Data.StateDto state, DateTime month)
        {
            if (light == null || state == null)
            {
                return BadRequest();
            }
            StatQuery param = new StatQueryBuilder().WithAndon(light.Id).WithState(state.Id).OnMonth(month).Build();

            int minute = 0;

            minute = service.GetAndonStateMinutesByMonth(param);

            return minute;
        }

        [HttpGet]
        public ActionResult<int> GetAndonStateCountByDay(Data.AndonDto light, Data.StateDto state, DateTime day)
        {
            if (light == null || state == null)
            {
                return BadRequest();
            }
            StatQuery param = new StatQueryBuilder().WithAndon(light.Id).WithState(state.Id).OnDay(day).Build();

            int count = service.GetAndonStateCountByDay(param);

            return count;
        }

        [HttpGet]
        public ActionResult<int> GetAndonStateCountByMonth(Data.AndonDto light, Data.StateDto state, DateTime day)
        {
            if (light == null || state == null)
            {
                return BadRequest();
            }
            StatQuery param = new StatQueryBuilder().WithAndon(light.Id).WithState(state.Id).OnDay(day).Build();

            int count = service.GetAndonStateCountByMonth(param);

            return count;
        }
    }
}
