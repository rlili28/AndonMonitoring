using AndonMonitoring.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        /// Retrieves the current state of the specified andon using its id.
        /// </summary>
        /// <param name="andonId">The andon's unique identifier</param>
        /// <returns>Returns a StateDto object with the current state information if successful, otherwise returns an appropriate error response.</returns>
        /// <response code="200">Retrieving successful</response>
        /// <response code="404"></response>
        /// <response code ="500"></response>
        [HttpGet("getState")]
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
                return StatusCode(500, e.Message);
            }

            if (state == null)
            {
                return NotFound();
            }
            return state;
        }

        /// <summary>
        /// Retreives the andon light record with the specified id
        /// </summary>
        /// <param name="andonId">The unique identifier of the andon to retrieve.</param>
        /// <returns>Returns an AndonDto object containing the information about the specified andon if successful, otherwise returns an error response.</returns>
        [HttpGet("getAndon")]
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
                return StatusCode(500, e.Message);
            }

            if(andonLight == null)
            {
                return NotFound();
            }

            return andonLight;
        }

        /// <summary>
        /// Updates the state of an Andon based on the provided Andon ID and new state ID.
        /// </summary>
        /// <param name="andonId">The ID of the Andon to update.</param>
        /// <param name="stateId">The ID of the new state to set.</param>
        /// <returns>Returns an HTTP status code indicating success or failure of the update operation.</returns>
        [HttpPost("updateState")]
        public IActionResult UpdateState(int andonId, int stateId)
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
                return StatusCode(500, e.Message);
            }

            if(newEvent == null)
            {
                return NotFound();
            }

            return Ok();
        }

        /// <summary>
        /// Adds a new Andon with the provided name
        /// </summary>
        /// <param name="light">The AndonDto object containing the name.</param>
        /// <returns>The id of the new andon if adding was successful, otherwise returns an error response</returns>
        [HttpPost("addAndon")]
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
                return StatusCode(500, e.Message);
            }
            
            //if(newAndonId < 0)
            //{
            //    return BadRequest();
            //}

            return newAndonId;
        }

        /// <summary>
        /// Updates an existing Andon with the provided data.
        /// </summary>
        /// <param name="andonLight">The AndonDto object containing the updated information about the andon</param>
        /// <returns>Returns an HTTP status code indicating the success or failure of the update operation.</returns>
        [HttpPut("UpdateAndon")]
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
                return StatusCode(500, e.Message);
            }
            
            return Ok();
        }

        /// <summary>
        /// Deletes the Andon with the specified id.
        /// </summary>
        /// <param name="id">The id of the Andon to delete.</param>
        /// <returns>Returns an HTTP status code indicating the success or failure of the delete operation.</returns>
        [HttpDelete("DeleteAndon")]
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
                return StatusCode(500, e.Message);
            }
            
            return Ok();
        }

    }
}
