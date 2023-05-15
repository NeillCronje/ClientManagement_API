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
    public class ClientRepositoryTests
    {
        private async Task<ApiContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<ApiContext>()
                .UseInMemoryDatabase(databaseName: "ClientDB").Options;

            var apiContext = new ApiContext(options);
            apiContext.Database.EnsureCreated();
            if (await apiContext.Clients.CountAsync() <= 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    apiContext.Clients.AddRange(
                    new Client()
                    {
                        Address = "1 Blue Street",
                        Age = 32,
                        City = "Pretoria",
                        CreatedDate = DateTime.Now,
                        FirstName = "Neill",
                        LastName = "Cronje",
                        Region = "Gauteng",
                        PostalCode = "0044",
                        Email = "neillcronje@gmail.com",
                        Account = new List<Account>() {
                            new Account()
                            {
                                Id = 1,
                                BankName = "FNB",
                                BranchCode = "6448002",
                                Card = "4512334151262",
                                CreatedDate = DateTime.Now,
                                Name = "Neill",
                                Number = "61231151142030",
                                Type = AccountType.Cheque
                            },
                            new Account()
                            {
                                Id = 4,
                                BankName = "FNB",
                                BranchCode = "6448002",
                                Card = "4515885612356",
                                CreatedDate = DateTime.Now,
                                Name = "Neill",
                                Number = "61231151142030",
                                Type = AccountType.Savings
                            }
                        }
                    },
                    new Client
                    {
                        Address = "123 Red Street",
                        Age = 30,
                        City = "Cape Town",
                        CreatedDate = DateTime.Now,
                        FirstName = "Gideon",
                        LastName = "Dewd",
                        Region = "Western Cape",
                        PostalCode = "5521",
                        Email = "randomdude04@gmail.com",
                        Account = new List<Account>() {
                            new Account()
                            {
                                Id = 2,
                                BankName = "ABSA",
                                BranchCode = "8421108",
                                Card = "8721611423058",
                                CreatedDate = DateTime.Now,
                                Name = "Alan",
                                Number = "84213315440211",
                                Type = AccountType.Savings
                            },
                            new Account()
                            {
                                Id = 5,
                                BankName = "ABSA",
                                BranchCode = "8421108",
                                Card = "8784421115478",
                                CreatedDate = DateTime.Now,
                                Name = "Alan",
                                Number = "84213315440211",
                                Type = AccountType.Credit
                            }
                        }
                    },
                    new Client
                    {
                        Address = "7 Law Street",
                        Age = 44,
                        City = "Johannesburg",
                        CreatedDate = DateTime.Now,
                        FirstName = "Ivan",
                        LastName = "Smith",
                        Region = "Gauteng",
                        PostalCode = "1223",
                        Email = "hippylover52@gmail.com",
                        Account = new List<Account>() {
                            new Account()
                            {
                                Id = 3,
                                BankName = "Nedbank",
                                BranchCode = "1124456",
                                Card = "2301550021300",
                                CreatedDate = DateTime.Now,
                                Name = "Ricky",
                                Number = "77408042315446",
                                Type = AccountType.Credit
                            },
                            new Account()
                            {
                                Id = 6,
                                BankName = "Nedbank",
                                BranchCode = "1124456",
                                Card = "2351124454445",
                                CreatedDate = DateTime.Now,
                                Name = "Ricky",
                                Number = "77408042315446",
                                Type = AccountType.Cheque
                            }
                        }
                    });

                    apiContext.Accounts.AddRange(
                    new Account()
                    {
                        Id = 1,
                        BankName = "FNB",
                        BranchCode = "6448002",
                        Card = "4512334151262",
                        CreatedDate = DateTime.Now,
                        Name = "Neill",
                        Number = "61231151142030",
                        Type = AccountType.Cheque
                    },
                    new Account()
                    {
                        Id = 2,
                        BankName = "ABSA",
                        BranchCode = "8421108",
                        Card = "8721611423058",
                        CreatedDate = DateTime.Now,
                        Name = "Alan",
                        Number = "84213315440211",
                        Type = AccountType.Savings
                    },
                    new Account()
                    {
                        Id = 3,
                        BankName = "Nedbank",
                        BranchCode = "1124456",
                        Card = "2301550021300",
                        CreatedDate = DateTime.Now,
                        Name = "Ricky",
                        Number = "77408042315446",
                        Type = AccountType.Credit
                    });

                    await apiContext.SaveChangesAsync();
                }
            }
            return apiContext;
        }

        [Fact]
        public async void ClientRepository_GetClient_ReturnsClient()
        {
            //Arrange
            var id = 1;
            var dbContext = await GetDatabaseContext();
            var clientRepo = new ClientRepository(dbContext);

            //Act
            var result = clientRepo.GetAsync(u => u.Id == id, false);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Task<Client>>();
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
