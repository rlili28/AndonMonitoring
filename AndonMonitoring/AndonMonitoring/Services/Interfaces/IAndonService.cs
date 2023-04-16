using Microsoft.AspNetCore.Mvc;
namespace AndonMonitoring.Services.Interfaces
{
    public interface IAndonService
    {

        public Data.StateDto GetState(int andonId);
        public Data.EventDto ChangeState(int andonId, int stateId);
        public Data.AndonDto GetAndon(int andonId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="andonLight"></param>
        /// <returns>the id of the newly created andon light</returns>
        public int AddAndon(Data.AndonDto andonLight);
        public void DeleteAndon(int andonId);
        public void UpdateAndon(Data.AndonDto andonLight);

    }
}
