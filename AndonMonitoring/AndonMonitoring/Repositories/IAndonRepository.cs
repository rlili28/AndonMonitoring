using AndonMonitoring.Data;

namespace AndonMonitoring.Repositories
{
    public interface IAndonRepository
    {
        public AndonDTO GetAndon(int id);
        public int AddAndon(AndonDTO light);
        public void DeleteAndon(int id);
        public void UpdateAndon(AndonDTO light);
    }
}
