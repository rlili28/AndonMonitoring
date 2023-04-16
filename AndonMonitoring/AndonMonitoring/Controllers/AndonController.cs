using AndonMonitoring.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AndonMonitoring.Controllers
{
    [ApiController]
    [Route("Andon")]
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
            if (andonId < 0)
            {
                return BadRequest();
            }

            Data.StateDto state;

            try
            {
                state = service.GetState(andonId);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"server error: {e.Message}");
            }

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
            if (andonId < 0)
            {
                return BadRequest();
            }

            Data.AndonDto andonLight;
            
            try
            {
                andonLight = service.GetAndon(andonId);
            }
            catch(Exception e)
            {
                return StatusCode(500, $"server error: {e.Message}");
            }

            if(andonLight == null)
            {
                return NotFound();
            }

            return andonLight;
        }

        /// <summary>
        /// Unintuitively this method is to update the state of the specified andon light object, and not a state object as the name would suggest
        /// </summary>
        /// <param name="andonId">the unique id of the andon light</param>
        /// <param name="stateId">the uniquw id of the light's new state</param>
        /// <returns>whether the update was successful or not</returns>
        [HttpPost]
        public ActionResult UpdateState(int andonId, int stateId)
        {
            if (andonId < 0 || stateId < 0)
                return BadRequest();

            Data.EventDto newEvent;

            try
            {
                newEvent = service.ChangeState(andonId, stateId);
            }
            catch(Exception e)
            {
                return StatusCode(500, $"server error: {e.Message}");
            }

            if(newEvent == null)
            {
                return NotFound();
            }

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

            int newAndonId;
            try
            {
                newAndonId = service.AddAndon(light);
            }
            catch(Exception e)
            {
                return StatusCode(500, $"server error: {e.Message}");
            }
            
            if(newAndonId < 0)
            {
                return BadRequest();
            }

            return newAndonId;
        }

        [HttpPut]
        public IActionResult UpdateAndon(Data.AndonDto andonLight)
        {
            if (andonLight == null || andonLight.Id < 0)
            {
                return BadRequest();
            }

            try
            {
                service.UpdateAndon(andonLight);
            }
            catch(Exception e)
            {
                return StatusCode(500, $"error: {e.Message}");
            }
            
            return Ok();
        }

        public IActionResult DeleteAndon(int id)
        {
            if(id < 0)
            {
                return BadRequest();
            }

            try
            {
                service.DeleteAndon(id);
            }
            catch(Exception e)
            {
                return StatusCode(500, $"{e.Message}");
            }
            
            return Ok();
        }

    }
}
