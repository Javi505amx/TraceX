using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using TraceX.Api.Controllers;
using TraceX.Api.DTOs;
using TraceX.Application.DTOs.WorkOrders;
using TraceX.Domain.Entities;
using TraceX.Domain.Interfaces;

namespace TraceX.Tests.Controllers;

public class WorkOrdersControllerTests
{
    private readonly Mock<IWorkOrderRepository> _repositoryMock;
    private readonly Mock<IValidator<CreateWorkOrderDto>> _createValidatorMock;
    private readonly WorkOrdersController _controller;

    public WorkOrdersControllerTests()
    {
        _repositoryMock = new Mock<IWorkOrderRepository>();
        _createValidatorMock = new Mock<IValidator<CreateWorkOrderDto>>();

        _controller = new WorkOrdersController(
            _repositoryMock.Object,
            _createValidatorMock.Object);
    }

    [Fact]
    public async Task GetWorkOrderById_ShouldReturnOk_WhenOrderExists()
    {
        // Arrange
        int orderId = 1;
        var existingOrder = new WorkOrder
        {
            Id = orderId,
            OrderNumber = "M5107-26050009",
            TargetQuantity = 100
        };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(orderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingOrder);

        // Act
        var result = await _controller.GetWorkOrderById(orderId, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var dto = Assert.IsType<WorkOrderDto>(okResult.Value);
        Assert.Equal(orderId, dto.Id);
        Assert.Equal("M5107-26050009", dto.OrderNumber);
    }

    [Fact]
    public async Task GetWorkOrderById_ShouldReturnNotFound_WhenOrderDoesNotExist()
    {
        // Arrange
        int nonexistentId = 99;

        _repositoryMock
            .Setup(r => r.GetByIdAsync(nonexistentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((WorkOrder?)null);

        // Act
        var result = await _controller.GetWorkOrderById(nonexistentId, CancellationToken.None);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task Create_ShouldReturnConflict_WhenOrderNumberAlreadyExists()
    {
        // Arrange
        var dto = new CreateWorkOrderDto("M5107-26050009", 100, 1, 1);
        var duplicatedOrder = new WorkOrder { Id = 1, OrderNumber = dto.OrderNumber };

        _createValidatorMock
            .Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _repositoryMock
            .Setup(r => r.GetByOrderNumberAsync(dto.OrderNumber, It.IsAny<CancellationToken>()))
            .ReturnsAsync(duplicatedOrder);

        // Act
        var result = await _controller.Create(dto, CancellationToken.None);

        // Assert
        var conflictResult = Assert.IsType<ConflictObjectResult>(result.Result);
        Assert.NotNull(conflictResult.Value);
    }

    [Fact]
    public async Task Create_ShouldReturnCreatedAtAction_WhenOrderIsValid()
    {
        // Arrange
        var dto = new CreateWorkOrderDto("M5107-26050009", 100, 1, 1);
        var createdOrder = new WorkOrder
        {
            Id = 10,
            OrderNumber = dto.OrderNumber,
            TargetQuantity = 100
        };

        _createValidatorMock
            .Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _repositoryMock
            .Setup(r => r.GetByOrderNumberAsync(dto.OrderNumber, It.IsAny<CancellationToken>()))
            .ReturnsAsync((WorkOrder?)null);

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<WorkOrder>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdOrder);

        // Cambiamos createdOrder.Id por It.IsAny<int>() para capturar cualquier ID
        _repositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdOrder);

        // Act
        var result = await _controller.Create(dto, CancellationToken.None);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var responseDto = Assert.IsType<WorkOrderDto>(createdResult.Value);
        Assert.Equal(10, responseDto.Id);
    }

    [Fact]
    public async Task UpdateProgress_ShouldReturnConflict_WhenConcurrencyExceptionOccurs()
    {
        // Arrange
        int orderId = 1;
        var patchDto = new UpdateWorkOrderProgressDto(WorkOrderStatus.InProgress, 50);

        // Se agrega OrderNumber requerido
        var existingOrder = new WorkOrder
        {
            Id = orderId,
            OrderNumber = "M5107-26050009",
            TargetQuantity = 100
        };

        var patchValidatorMock = new Mock<IValidator<UpdateWorkOrderProgressDto>>();
        patchValidatorMock
            .Setup(v => v.ValidateAsync(patchDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _repositoryMock
            .Setup(r => r.GetByIdAsync(orderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingOrder);

        _repositoryMock
            .Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new DbUpdateConcurrencyException());

        // Act
        var result = await _controller.UpdateProgress(
            orderId,
            patchDto,
            patchValidatorMock.Object,
            CancellationToken.None);

        // Assert
        Assert.IsType<ConflictObjectResult>(result.Result);
    }
}