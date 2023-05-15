using ClientManagementAPI.Data;
using ClientManagementAPI.Models;
using ClientManagementAPI.Models.Enums;
using ClientManagementAPI.Repository;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientManagement.Tests.Repository
{
    public class AccountRepositoryTests
    {
        private async Task<ApiContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<ApiContext>()
                .UseInMemoryDatabase(databaseName: "AccountDB").Options;

            var apiContext = new ApiContext(options);
            apiContext.Database.EnsureCreated();
            
            return apiContext;
        }

        [Fact]
        public async void ClientRepository_GetAccount_ReturnsAccount()
        {
            //Arrange
            var id = 2;
            var dbContext = await GetDatabaseContext();
            var accountRepo = new AccountRepository(dbContext);

            //Act
            var result = accountRepo.GetAsync(u => u.Id == id, false);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Task<Account>>();
        }

        [Fact]
        public async void ClientRepository_GetAllClient_ReturnsAllClients()
        {
            //Arrange
            var dbContext = await GetDatabaseContext();
            var clientRepo = new ClientRepository(dbContext);

            //Act
            var result = await clientRepo.GetAllAsync(null);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<List<Client>>();
        }

        [Fact]
        public async void ClientRepository_RemoveClient_ReturnsOk()
        {
            //Arrange
            var clientToRemove = new Client();
            var dbContext = await GetDatabaseContext();
            var clientRepo = new ClientRepository(dbContext);

            //Act
            var result = clientRepo.RemoveAsync(clientToRemove);

            //Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async void ClientRepository_CreateClient_ReturnsCreatedClient()
        {
            //Arrange
            var clientToAdd = new Client()
            {
                Address = "New Address",
                Age = 77,
                City = "Delft",
                Email = "DelftPunk@gmail.com",
                FirstName = "NoScoped720",
                LastName = "SniperLord",
                PostalCode = "12345",
                Region = "Netherlands"
            };
            var dbContext = await GetDatabaseContext();
            var clientRepo = new ClientRepository(dbContext);

            //Act
            var result = clientRepo.CreateAsync(clientToAdd);

            //Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async void ClientRepository_UpdateClient_ReturnsUpdatedClient()
        {
            //Arrange
            var clientToUpdate = new Client()
            {
                Id = 1,
                Address = "New Address 2",
                Age = 88,
                City = "Delft 2",
                Email = "DelftPunk22@gmail.com",
                FirstName = "NoScoped420",
                LastName = "CamperLord",
                PostalCode = "54321",
                Region = "Netherlands"
            };
            var dbContext = await GetDatabaseContext();
            var clientRepo = new ClientRepository(dbContext);

            //Act
            var result = clientRepo.UpdateClientAsync(clientToUpdate);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Task<Client>>();
        }
    }
}
