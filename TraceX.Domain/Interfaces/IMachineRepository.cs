using TraceX.Domain.Entities;


namespace TraceX.Domain.Interfaces
{
    public interface IMachineRepository
    {
        Task<List<Machine>> GetAllAsync();

        // Tenemos activada una característica llamada Nullable Reference Types (Tipos de referencia anulables).
        // Si dejas el retorno como Task<Machine>, le estás diciendo al compilador:
        // "Te prometo que este método SIEMPRE va a devolver una máquina, nunca va a ser nulo".
        Task<Machine?> GetByIdAsync(int id);
        Task<Machine?> GetBySerialNumberAsync(string serialNumber);
        Task<Machine> AddAsync(Machine machine, CancellationToken cancellationToken);
        Task<int> UpdateAsync(Machine machine);
        Task<int> DeleteAsync(int id);
    }
}
