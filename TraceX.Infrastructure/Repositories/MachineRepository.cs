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
            return await _context.Machines
                .AsNoTracking()
                .ToListAsync();
        }

        //public async Task<List<Machine>> GetAllActiveMachinesAsync()
        //{
        //    return await _context.Machines
        //        .AsNoTracking()
        //        .Where(m => m.I)
        //}

        public async Task<Machine?> GetByIdAsync(int id)
        {
            return await _context.Machines.FindAsync(id);
        }

        public async Task<Machine?> GetBySerialNumberAsync(string serialNumber)
        {
            return await _context.Machines
                .FirstOrDefaultAsync(m => m.SerialNumber == serialNumber);
        }


        public async Task<Machine> AddAsync(Machine machine, CancellationToken cancellationToken)
        {
            // 1. Le avisamos a EF Core que queremos rastrear esta nueva entidad
            await _context.Machines.AddAsync(machine);
            // 2. Impactamos físicamente la base de datos ejecutando el INSERT
            await _context.SaveChangesAsync(cancellationToken);
            // Al retornar la máquina, ya llevará el 'Id' autonumérico generado por SQL Server
            return machine;
        }



        public async Task<int> UpdateAsync(Machine machine)
        {
            _context.Machines.Update(machine);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var machine = await _context.Machines.FindAsync(id);
            if (machine is null) return 0;

            _context.Machines.Remove(machine);
            return await _context.SaveChangesAsync();
        }

        //public Task<bool> IsActiveAsync(int id)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
