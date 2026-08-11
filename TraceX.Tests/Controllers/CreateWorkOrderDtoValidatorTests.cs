using FluentValidation.TestHelper;
using Moq;
using TraceX.Api.DTOs;
using TraceX.Application.Validators.WorkOrders;
using TraceX.Domain.Entities;
using TraceX.Domain.Interfaces;

namespace TraceX.Tests.Controllers;

public class CreateWorkOrderDtoValidatorTests
{
    private readonly Mock<IProductRepository> _productRepoMock;
    private readonly Mock<IMachineRepository> _machineRepoMock;
    private readonly CreateWorkOrderDtoValidator _validator;

    public CreateWorkOrderDtoValidatorTests()
    {
        _productRepoMock = new Mock<IProductRepository>();
        _machineRepoMock = new Mock<IMachineRepository>();

        // Inyectamos los objetos simulados (Mocks) en el validador
        _validator = new CreateWorkOrderDtoValidator(
            _productRepoMock.Object,
            _machineRepoMock.Object
        );
    }

    #region OrderNumber Validation Tests

    [Theory]
    [InlineData("M5107-26050009")] // México, Tipo 5107, Mayo 2026, Consecutivo 0009
    [InlineData("I1200-25120001")] // India, Tipo 1200, Diciembre 2025, Consecutivo 0001
    [InlineData("A0001-24010000")] // América, Tipo 0001, Enero 2024, Consecutivo 0000
    public async Task OrderNumber_ShouldNotHaveValidationError_WhenFormatIsValid(string validOrderNumber)
    {
        // Arrange
        var dto = new CreateWorkOrderDto(validOrderNumber, 100, 1, null);

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.OrderNumber);
    }

    [Theory]
    [InlineData("X5107-26050009")] // Centro 'X' no permitido
    [InlineData("M5107-26130009")] // Mes '13' fuera de rango (01-12)
    [InlineData("M510-26050009")]  // Tipo de orden de solo 3 dígitos
    [InlineData("M510726050009")]   // Falta el guion medio
    [InlineData("")]               // Vacío
    public async Task OrderNumber_ShouldHaveValidationError_WhenFormatIsInvalid(string invalidOrderNumber)
    {
        // Arrange
        var dto = new CreateWorkOrderDto(invalidOrderNumber, 100, 1, null);

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.OrderNumber);
    }

    #endregion

    #region TargetQuantity Validation Tests

    [Fact]
    public async Task TargetQuantity_ShouldNotHaveValidationError_WhenGreaterThanZero()
    {
        // Arrange
        var dto = new CreateWorkOrderDto("M5107-26050009", 50, 1, null);

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.TargetQuantity);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public async Task TargetQuantity_ShouldHaveValidationError_WhenZeroOrNegative(int invalidQuantity)
    {
        // Arrange
        var dto = new CreateWorkOrderDto("M5107-26050009", invalidQuantity, 1, null);

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.TargetQuantity);
    }

    #endregion

    #region ProductId Asynchronous Validation Tests

    [Fact]
    public async Task ProductId_ShouldNotHaveValidationError_WhenProductExistsInDb()
    {
        // Arrange
        int existingProductId = 1;

        _productRepoMock
            .Setup(r => r.GetByIdAsync(existingProductId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Product { Id = existingProductId, Name = "Placa SMT MainBoard" });

        var dto = new CreateWorkOrderDto("M5107-26050009", 100, existingProductId, null);

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.ProductId);
    }

    [Fact]
    public async Task ProductId_ShouldHaveValidationError_WhenProductDoesNotExistInDb()
    {
        // Arrange
        int nonexistentProductId = 99;

        _productRepoMock
            .Setup(r => r.GetByIdAsync(nonexistentProductId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product?)null);

        var dto = new CreateWorkOrderDto("M5107-26050009", 100, nonexistentProductId, null);

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ProductId);
    }

    #endregion

    #region MachineId Asynchronous Validation Tests

    [Fact]
    public async Task MachineId_ShouldNotHaveValidationError_WhenMachineIdIsNull()
    {
        // Arrange
        var dto = new CreateWorkOrderDto("M5107-26050009", 100, 1, null);

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.MachineId);
    }

    [Fact]
    public async Task MachineId_ShouldNotHaveValidationError_WhenMachineExistsInDb()
    {
        // Arrange
        int existingMachineId = 5;
        int validProductId = 1;

        _productRepoMock
        .Setup(r => r.GetByIdAsync(validProductId, It.IsAny<CancellationToken>()))
        .ReturnsAsync(new Product { Id = validProductId, Name = "Placa SMT MainBoard" });

        // Configurar la máquina como inexistente (retorna null)
        _machineRepoMock
            .Setup(r => r.GetByIdAsync(existingMachineId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Machine
            {
                Id = existingMachineId,
                SerialNumber = "SMT-LINE-01",
                ProductionLine = "Line 1" // <-- Agregar la propiedad requerida
            });

        var dto = new CreateWorkOrderDto("M5107-26050009", 100, 1, existingMachineId);

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.MachineId);
    }

    [Fact]
    public async Task MachineId_ShouldHaveValidationError_WhenMachineDoesNotExistInDb()
    {
        // Arrange
        int nonexistentMachineId = 88;
        int validProductId = 1;

        // Configurar el producto como válido para aislar la prueba de MachineId
        _productRepoMock
            .Setup(r => r.GetByIdAsync(validProductId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Product { Id = validProductId, Name = "Placa SMT MainBoard" });

        // Configurar la máquina como inexistente (retorna null)
        _machineRepoMock
            .Setup(r => r.GetByIdAsync(nonexistentMachineId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Machine?)null);

        var dto = new CreateWorkOrderDto("M5107-26050009", 100, validProductId, nonexistentMachineId);

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.MachineId);
    }

    #endregion
}