```cs
TraceX.Domain (Capa Central)
 ├── Entities/ (Machine)
 ├── Enums/ (MachineStatus)
 └── Interfaces/ (IMachineRepository)

TraceX.Application (Reglas de Negocio)
 ├── DTOs/ (CreateMachineDto, UpdateMachineDto, MachineDto)
 └── Validators/ (CreateMachineDtoValidator, UpdateMachineDtoValidator)

TraceX.Infrastructure (Acceso a Datos)
 └── Repositories/ (MachineRepository con Entity Framework Core)

TraceX.Api (Punto de Entrada HTTP)
 ├── Controllers/ (MachinesController - Thin Controller)
 └── Program.cs (Inyección de dependencias, JsonStringEnumConverter, etc.)
```