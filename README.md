```cs
TraceX.Domain (Capa Central / Central layer)
 ├── Entities/ (Machine)
 ├── Enums/ (MachineStatus)
 └── Interfaces/ (IMachineRepository)

TraceX.Application (Reglas de Negocio / Bussiness Rules)
 ├── DTOs/ (CreateMachineDto, UpdateMachineDto, MachineDto)
 └── Validators/ (CreateMachineDtoValidator, UpdateMachineDtoValidator)

TraceX.Infrastructure (Acceso a Datos / Data layer)
 └── Repositories/ (MachineRepository con Entity Framework Core)

TraceX.Api (Punto de Entrada HTTP / HTTP Entry Point API)
 ├── Controllers/ (MachinesController - Thin Controller)
 └── Program.cs (Inyección de dependencias, JsonStringEnumConverter, etc.)
```