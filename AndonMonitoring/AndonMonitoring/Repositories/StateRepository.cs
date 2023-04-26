using AndonMonitoring.Data;
using AndonMonitoring.Model;
using AndonMonitoring.Repositories.Interface;

namespace AndonMonitoring.Repositories
{
    public class StateRepository : IStateRepository
    {
        private AndonDbContext db;
        public StateRepository(AndonDbContext context)
        {
            db = context;
        }

        public int AddState(StateDto dto)
        {
            try
            {
                if (dto == null)
                    return -1;

                var state = new State { Name = dto.Name };
                db.State.Add(state);
                db.SaveChanges();
                return state.Id;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public StateDto GetState(int id)
        {
            try
            {
                StateDto? state = db.State
                    .Where(s => s.Id == id)
                    .Select(s => new StateDto(s.Id, s.Name))
                    .FirstOrDefault();

                if (state == null)
                    throw new Exception("id doesn't exist");
                return state;
            }
            catch { throw; }
        }
    }
}
