using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TraceX.Application.DTOs.Machines;
using TraceX.Domain.Interfaces;
using Machine = TraceX.Domain.Entities.Machine;

namespace TraceX.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MachinesController : ControllerBase
    {
        private readonly IMachineRepository _machineRepository; // <-- readonly
        private readonly IValidator<CreateMachineDto> _createValidator;
        private readonly IValidator<UpdateMachineDto> _updateValidator;

        // readonly evita que por un error de dedo más adelante, cambiemos el valor de la variable dentro del código del controlador.

        // Si metemos el DbContext directo aquí,
        // estamos rompiendo la Clean Architecture que planeamos para TraceX.
        // El controlador no debe saber qué es Entity Framework ni cómo se conecta a SQL;
        // solo debe conocer la interfaz del repositorio.

        // private TraceXDbContext _context;
        // public MachinesController(TraceXDbContext context)
        // {
        //     _context = context;
        // }

        //.NET inyectará la implementación real (MAchineRepository) aquií de forma automática.

        public MachinesController(IMachineRepository machineRepository,
                                IValidator<CreateMachineDto> createValidator,
                                IValidator<UpdateMachineDto> updateValidator)
        {
            _machineRepository = machineRepository;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        [HttpGet] // GET: api/machines
        public async Task<ActionResult<List<MachineDto>>> GetMachines(CancellationToken cancellationToken)
        {
            var machines = await _machineRepository.GetAllAsync();
            var machineDtos = machines.Adapt<List<MachineDto>>();
            return Ok(machineDtos); // <-- Envuelve tu lista en un HTTP 200 OK explícito
        }

        // Este mapa le dice a la API: "Si mandan un GET a /api/machines/{id}, ejecuta esto"
        [HttpGet("{id:int}")]
        public async Task<ActionResult<MachineDto>> GetMachineById(int id, CancellationToken cancellationToken)
        {
            var machine = await _machineRepository.GetByIdAsync(id, cancellationToken);
            if (machine == null) return NotFound();

            return Ok(machine.Adapt<MachineDto>());
        }

        [HttpPost] // POST: api/machines
        public async Task<ActionResult<MachineDto>> CreateMachine(CreateMachineDto dto, CancellationToken cancellationToken)
        {
            // check: this null reference
            if (dto == null) return BadRequest("Machine data dto is required.");

            var validationResult = await _createValidator.ValidateAsync(dto);
            if (!validationResult.IsValid) return BadRequest(validationResult.ToDictionary());

            var existingMachine = await _machineRepository.GetBySerialNumberAsync(dto.SerialNumber);
            if (existingMachine != null)
                return Conflict(new { Message = "A machine with this Serial Number already exists." });

            // Mapear el DTO de entrada de la entidad de dominio
            var machineEntity = dto.Adapt<Machine>();
            var createdMachine = await _machineRepository.AddAsync(machineEntity, cancellationToken);
            var responseDto = createdMachine.Adapt<MachineDto>();

            return CreatedAtAction(
                nameof(GetMachineById),
                new { id = responseDto.Id },
                responseDto);
        }

        [HttpPut("{id:int}")] // PUT: api/machines/1
        public async Task<ActionResult> UpdateMachine(int id, UpdateMachineDto dto, CancellationToken cancellationToken)
        {
            try
            {

                if (dto == null) return BadRequest("Machine info needed for update");

                var validationResult = await _updateValidator.ValidateAsync(dto);
                if (!validationResult.IsValid) return BadRequest(validationResult.ToDictionary());

                var existingMachine = await _machineRepository.GetByIdAsync(id, cancellationToken);
                if (existingMachine == null) return NotFound();

                dto.Adapt(existingMachine);

                await _machineRepository.UpdateAsync(existingMachine);
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict(new
                {
                    message = "Registry was updated by another user. Please, Refresh data and try again... "
                });
            }

        }

        [HttpDelete("{id:int}")] // DELETE: api/machines/1
        public async Task<ActionResult> DeleteMachine(int id, CancellationToken cancellationToken)
        {
            var existingMachine = await _machineRepository.GetByIdAsync(id, cancellationToken);
            if (existingMachine == null) return NotFound();

            await _machineRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
