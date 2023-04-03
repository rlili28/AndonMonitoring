using AndonMonitoring.Repositories;

namespace AndonMonitoring.Services
{
    public class AndonService
    {
        private readonly IAndonRepository andonRepository;

        public AndonService(IAndonRepository ar)
        {
            this.andonRepository = ar;
        }
    }
}
