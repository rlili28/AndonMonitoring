using Microsoft.AspNetCore.Mvc;
namespace AndonMonitoring.Services
{
    public interface IAndonService
    {
        public ActionResult<Data.StateDto> GetState(int andonId);
        public IActionResult ChangeState(int andonId, int stateId);
        public ActionResult<Data.AndonDto> GetAndon(int andonId);
        public IActionResult AddAndon(Data.AndonDto andonLight);
        public IActionResult DeleteAndon(int andonId);
        public IActionResult UpdateAndon(Data.AndonDto andonLight);

    }
}
