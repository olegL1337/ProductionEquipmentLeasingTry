using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductionEquipmentLeasing.API.Controllers;
using ProductionEquipmentLeasing.Application.DTOs;
using ProductionEquipmentLeasing.Application.Interfaces.Services;
using Xunit;

namespace Tests.UnitTests.Controllers
{
    public class EquipmentTypesControllerTests
    {
        private readonly Mock<IEquipmentTypeService> _serviceMock;
        private readonly EquipmentTypesController _controller;

        public EquipmentTypesControllerTests()
        {
            _serviceMock = new Mock<IEquipmentTypeService>();
            _controller = new EquipmentTypesController(_serviceMock.Object);
        }

        [Fact]
        public async Task GetById_ReturnsOk_WithEquipmentTypeDto()
        {
            // Arrange
            var id = Guid.NewGuid();
            var dto = new EquipmentTypeDto { Id = id, Name = "TypeA" };
            _serviceMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(dto);

            // Act
            var result = await _controller.GetById(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(dto, okResult.Value);
        }

        [Fact]
        public async Task GetAll_ReturnsOk_WithListOfEquipmentTypeDto()
        {
            // Arrange
            var dtos = new List<EquipmentTypeDto>
            {
                new EquipmentTypeDto { Id = Guid.NewGuid(), Name = "TypeA" }
            };
            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(dtos);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(dtos, okResult.Value);
        }

        [Fact]
        public async Task Create_ReturnsOk_WithCreatedId()
        {
            // Arrange
            var createDto = new CreateEquipmentTypeDto { Name = "TypeA" };
            var createdId = Guid.NewGuid();
            _serviceMock.Setup(s => s.CreateAsync(createDto)).ReturnsAsync(createdId);

            // Act
            var result = await _controller.Create(createDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(createdId, okResult.Value);
        }

        [Fact]
        public async Task Update_ReturnsNoContent()
        {
            // Arrange
            var updateDto = new UpdateEquipmentTypeDto { Id = Guid.NewGuid(), Name = "TypeB" };
            _serviceMock.Setup(s => s.UpdateAsync(updateDto)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Update(updateDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _serviceMock.Verify(s => s.UpdateAsync(updateDto), Times.Once);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent()
        {
            // Arrange
            var id = Guid.NewGuid();
            _serviceMock.Setup(s => s.DeleteAsync(id)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _serviceMock.Verify(s => s.DeleteAsync(id), Times.Once);
        }
    }
}