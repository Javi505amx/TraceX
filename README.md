```cs
TraceX.Domain (Capa Central / Central layer)
 ├── Entities/ (Machine, Product)
 ├── Enums/ (MachineStatus)
 └── Interfaces/ (IMachineRepository, IProductRepository)

TraceX.Application (Reglas de Negocio / Bussiness Rules)
 ├── DTOs/
 │    ├── Machines(CreateMachineDto, UpdateMachineDto, MachineDto)
 │    ├── Products(CreateProductDto, UpdateProductDto, ProductDto)
 └── Validators/ 
 │    ├── Machines(CreateMachineDtoValidator, UpdateMachineDtoValidator, MachineDtoValidator)
 │    ├── Products(CreateProductDtoValidator, UpdateProductDtoValidator, ProductDtoValidator)
TraceX.Infrastructure (Acceso a Datos / Data layer)
 └── Repositories/ (MachineRepository, ProductRepository) //con Entity Framework Core

TraceX.Api (Punto de Entrada HTTP / HTTP Entry Point API)
 ├── Controllers/ (MachinesController, ProductsController ) //Thin Controller
 └── Program.cs (Inyección de dependencias, JsonStringEnumConverter, etc.)
```