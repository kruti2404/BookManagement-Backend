
using Data;
using Microsoft.EntityFrameworkCore;


namespace Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DbSet<T> _table;
        private readonly ProgramDbContent _context;

        public GenericRepository(ProgramDbContent context)
        {
            _context = context;
            _table = _context.Set<T>();
        }
        public void Delete(T obj)
        {
            _table.Remove(obj);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _table.ToListAsync();
        }

        public async Task<T> GetById(object id)
        {
            return await _table.FindAsync(id);
        }

        public async Task Insert(T obj)
        {
            await _table.AddAsync(obj);
        }

        public void Update(T obj)
        {
            _table.Update(obj);
        }
    }
}
