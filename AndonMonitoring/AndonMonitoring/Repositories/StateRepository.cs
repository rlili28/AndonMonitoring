using AndonMonitoring.Data;
using AndonMonitoring.Model;

namespace AndonMonitoring.Repositories
{
    public class StateRepository : IStateRepository
    {
        private AndonDbContext db;
        public StateRepository(AndonDbContext context)
        {
            this.db = context;
        }

        public int AddState(StateDto dto)
        {
            if (dto == null)
                return -1;

            var state = new State { Name = dto.Name };
            db.State.Add(state);
            db.SaveChanges();
            return state.Id;
        }
    }
}
