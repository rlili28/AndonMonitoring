using AndonMonitoring.Data;

namespace AndonMonitoring.Repositories.Interface
{
    public interface IAndonRepository
    {
        public AndonDto GetAndon(int id);
        public int AddAndon(AndonDto light);
        public void DeleteAndon(int id);
        public void UpdateAndon(AndonDto light);
    }
}
