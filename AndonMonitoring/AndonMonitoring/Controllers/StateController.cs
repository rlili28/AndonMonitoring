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

        /// <summary>
        /// Adds a new state item.
        /// </summary>
        /// <param name="state">The StateDto object representing the state to be added.</param>
        /// <returns>Returns the id of the new state.</returns>
        /// <response code="200">Returns the id of the added state.</response>
        /// <response code="500">If there is an error in adding the state.</response>
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
