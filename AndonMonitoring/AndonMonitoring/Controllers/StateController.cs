using AndonMonitoring.Data;
using AndonMonitoring.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AndonMonitoring.Controllers
{
    [ApiController]
    [Route("/State")]
    public class StateController
    {
        private readonly IStateService stateService;

        public StateController(IStateService stateS)
        {
            stateService = stateS;
        }

        [HttpPost("addState")]
        public ActionResult<int> AddState(StateDto state)
        {
            try
            {
                return stateService.AddState(state);
            }
            catch (Exception)
            {
                return StatusCodes.Status500InternalServerError;
            }
        }
    }
}
