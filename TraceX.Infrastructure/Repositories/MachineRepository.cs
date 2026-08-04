using Microsoft.EntityFrameworkCore;
using TraceX.Domain.Entities;
using TraceX.Domain.Interfaces;
using TraceX.Infrastructure.Data;

namespace TraceX.Infrastructure.Repositories
{
    public class MachineRepository : IMachineRepository
    {
        public readonly TraceXDbContext _context;
        public MachineRepository(TraceXDbContext context)
        {
            _context = context;
        }

        public async Task<List<Machine>> GetAllAsync()
        {
            return await _context.Machines.ToListAsync();
        }

        public async Task<Machine?> GetByIdAsync(int id)
        {
            return await _context.Machines.FindAsync(id);
        }


        public async Task<Machine> AddAsync(Machine machine)
        {
            // 1. Le avisamos a EF Core que queremos rastrear esta nueva entidad
            await _context.Machines.AddAsync(machine);

            // 2. Impactamos físicamente la base de datos ejecutando el INSERT
            await _context.SaveChangesAsync();

            // Al retornar la máquina, ya llevará el 'Id' autonumérico generado por SQL Server
            return machine;
        }



        public async Task<int> UpdateAsync(Machine machine)
        {
            _context.Machines.Update(machine);
            return await _context.SaveChangesAsync();
        }

        public Task<int> DeleteAsync(Machine id)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteAsync(int id)
        {
            return await _context.Machines
                .Where(m => m.Id == id)
                .ExecuteDeleteAsync();
        }
    }
}
