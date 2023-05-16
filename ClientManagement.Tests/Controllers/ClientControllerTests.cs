using AutoMapper;
using ClientManagementAPI.Controllers;
using ClientManagementAPI.Models;
using ClientManagementAPI.Models.Dto;
using ClientManagementAPI.Repository.IRepository;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ClientManagement.Tests.Controllers
{
    public class ClientControllerTests
    {
        private readonly IClientRepository _dbClient;
        private readonly IAccountRepository _dbAccount;
        private readonly IMapper _mapper;
        public ClientControllerTests()
        {
            _dbClient = A.Fake<IClientRepository>();
            _dbAccount = A.Fake<IAccountRepository>();
            _mapper = A.Fake<IMapper>();
        }

        [Fact]
        public void ClientController_GetClients_ReturnOK()
        {
            //Arrange
            var clients = A.Fake<ICollection<ClientDTO>>();
            var clientList = A.Fake<List<ClientDTO>>();
            A.CallTo(() => _mapper.Map<List<ClientDTO>>(clients)).Returns(clientList);
            var controller = new ClientController(_dbClient, _dbAccount, _mapper);

            //Act
            var result = controller.GetClients();

            //Assert
            result.Should().NotBeNull();
            Assert.False(result.IsFaulted);
            Assert.True(result.IsCompletedSuccessfully);
        }

        [Fact]
        public void ClientController_GetClient_ReturnOK()
        {
            //Arrange
            int id = 1;
            var client = A.Fake<ClientDTO>();
            A.CallTo(() => _mapper.Map<ClientDTO>(client)).Returns(client);
            var controller = new ClientController(_dbClient, _dbAccount, _mapper);

            //Act
            var result = controller.GetClient(id);

            //Assert
            result.Should().NotBeNull();
            Assert.False(result.IsFaulted);
            Assert.True(result.IsCompletedSuccessfully);
        }

        [Fact]
        public void ClientController_RemoveClient_ReturnOK()
        {
            //Arrange
            int id = 1;
            var client = A.Fake<ClientDTO>();
            A.CallTo(() => _mapper.Map<ClientDTO>(client));
            var controller = new ClientController(_dbClient, _dbAccount, _mapper);

            //Act
            var result = controller.DeleteClient(id);

            //Assert
            result.Should().NotBeNull();
            Assert.False(result.IsFaulted);
            Assert.True(result.IsCompletedSuccessfully);
        }

        [Fact]
        public void ClientController_UpdateClient_ReturnOK()
        {
            //Arrange
            ClientUpdateDTO updateClient = new ClientUpdateDTO()
            {
                Id = 1,
                FirstName = "Test",
                MiddleName = "Update",
                LastName = "Successful",
                Address = "Test Address 12",
                Age = 70,
                City = "Test City",
                PostalCode = "5123",
                Region = "Test Region",
                Email = "TestEmail@test.com"
            };
            var client = A.Fake<ClientDTO>();
            A.CallTo(() => _mapper.Map<ClientDTO>(client));
            var controller = new ClientController(_dbClient, _dbAccount, _mapper);

            //Act
            var result = controller.UpdateClient(updateClient.Id, updateClient);

            //Assert
            result.Should().NotBeNull();
            Assert.False(result.IsFaulted);
            Assert.True(result.IsCompletedSuccessfully);
        }
    }
}
