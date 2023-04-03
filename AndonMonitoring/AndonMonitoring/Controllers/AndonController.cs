using AndonMonitoring.Services;
using Microsoft.AspNetCore.Mvc;

namespace AndonMonitoring.Controllers
{
    [Route("api/Andon")]
    [ApiController]
    public class AndonController : ControllerBase
    {
        private readonly IAndonService service;
        public AndonController(IAndonService andonService)
        {
            service = andonService;
        }

        /// <summary>
        /// Gets current state associated with the specified andon id
        /// </summary>
        /// <param name="andonId">the unique andon id</param>
        /// <returns>a state data transfer object containing the state the specified andon light is currently in</returns>
        [HttpGet("{andonId}")]
        public ActionResult<Data.StateDto> GetState(int andonId)   
        {
            if (andonId == null || andonId < 1)
            {
                return BadRequest();
            }
            var state = service.GetState(andonId);
            if (state == null)
            {
                   return NotFound();
            }
            return state;
        }

        /// <summary>
        /// Gets the andon light record with the specified id
        /// </summary>
        /// <param name="andonId">the unique andon id</param>
        /// <returns>an andon data transfer object containing the the andon object that has the specified id (or bad request:c)</returns>
        [HttpGet("{andonId}")]
        public ActionResult<Data.AndonDto> GetAndon(int andonId)
        {
            if (andonId == null || andonId < 1)
            {
                return BadRequest();
            }
            var andonLight = service.GetAndon(andonId);
            if(andonLight == null)
            {
                return NotFound();
            }
            return andonLight;
        }

        /// <summary>
        /// Unintuitively this method is to update the state of the specified andon light object, and not a state object as the name would suggest
        /// so maybe I should rename it
        /// </summary>
        /// <param name="andonId">the unique id of the andon light</param>
        /// <param name="stateId">the uniquw id of the light's new state</param>
        /// <returns>whether the update was successful or not</returns>
        [HttpPost]
        public IActionResult UpdateState(int andonId, int stateId)
        {
            if (andonId < 1 || stateId < 1)
                return BadRequest();
                       
            var queryResult = service.ChangeState(andonId, stateId);

            return Ok();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="light"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<int> AddAndon(Data.AndonDto light)
        {
            if(light == null || light.Name == null)
                return BadRequest();

            var queryResult = service.AddAndon(light);
            
            if(queryResult == null)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPut]
        public IActionResult UpdateAndon(Data.AndonDto andonLight)
        {
            if(andonLight == null || andonLight.Id < 1)
            {
                return BadRequest();
            }

            var queryResult = service.AddAndon(andonLight);
            
            return NoContent();
        }

        public IActionResult DeleteAndon(int id)
        {
            if(id < 1)
            {
                return BadRequest();
            }

            var queryResult = service.DeleteAndon(id);
            
            return NoContent();
        }

    }
}
