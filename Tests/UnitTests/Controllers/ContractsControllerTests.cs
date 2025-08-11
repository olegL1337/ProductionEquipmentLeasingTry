using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductionEquipmentLeasing.API.Controllers;
using ProductionEquipmentLeasing.Application.DTOs;
using ProductionEquipmentLeasing.Application.Interfaces.Services;
using Xunit;

namespace Tests.UnitTests.Controllers
{
    public class ContractsControllerTests
    {
        private readonly Mock<IContractService> _contractServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ContractsController _controller;

        public ContractsControllerTests()
        {
            _contractServiceMock = new Mock<IContractService>();
            _mapperMock = new Mock<IMapper>();
            _controller = new ContractsController(_contractServiceMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetById_ReturnsOk_WithContract()
        {
            // Arrange
            var contractId = Guid.NewGuid();
            var contractDto = new ContractDto();
            _contractServiceMock.Setup(s => s.GetByIdAsync(contractId))
                .ReturnsAsync(contractDto);

            // Act
            var result = await _controller.GetById(contractId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(contractDto, okResult.Value);
        }

        [Fact]
        public async Task GetAll_ReturnsOk_WithContracts()
        {
            // Arrange
            var contracts = new List<ContractDto> { new ContractDto() };
            _contractServiceMock.Setup(s => s.GetAllAsync())
                .ReturnsAsync(contracts);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(contracts, okResult.Value);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtAction_WithId()
        {
            // Arrange
            var createDto = new CreateContractDto();
            var contractId = Guid.NewGuid();
            _contractServiceMock.Setup(s => s.CreateAsync(createDto))
                .ReturnsAsync(contractId);

            // Act
            var result = await _controller.Create(createDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_controller.GetById), createdResult.ActionName);
            Assert.Equal(contractId, createdResult.Value);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent()
        {
            // Arrange
            var id = Guid.NewGuid();
            _contractServiceMock.Setup(s => s.DeleteAsync(id))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
        [Fact]
        public async Task Update_ReturnsNoContent_WhenUpdateIsSuccessful()
        {
            // Arrange
            var updateDto = new UpdateContractDto(Guid.NewGuid(), 10);
            _contractServiceMock.Setup(s => s.UpdateAsync(updateDto)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Update(updateDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _contractServiceMock.Verify(s => s.UpdateAsync(updateDto), Times.Once);
        }

        [Fact]
        public async Task Update_ThrowsException_ReturnsException()
        {
            // Arrange
            var updateDto = new UpdateContractDto(Guid.NewGuid(), 5);
            _contractServiceMock
                .Setup(s => s.UpdateAsync(updateDto))
                .ThrowsAsync(new Exception("Update failed"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _controller.Update(updateDto));
        }
    }
}