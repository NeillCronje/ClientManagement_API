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
        public async void AccountRepository_GetAccount_ReturnsAccount()
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
            Assert.True(result.IsCompletedSuccessfully);
            Assert.False(result.IsFaulted);
        }

        [Fact]
        public async void AccountRepository_GetAllAccount_ReturnsAllAccounts()
        {
            //Arrange
            var dbContext = await GetDatabaseContext();
            var accountRepo = new AccountRepository(dbContext);

            //Act
            var result = await accountRepo.GetAllAsync(null);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<List<Account>>();
        }

        [Fact]
        public async void ClientRepository_RemoveClient_ReturnsOk()
        {
            //Arrange
            var accountToRemove = new Account();
            var dbContext = await GetDatabaseContext();
            var accountRepo = new AccountRepository(dbContext);

            //Act
            var result = accountRepo.RemoveAsync(accountToRemove);

            //Assert
            result.Should().NotBeNull();
            Assert.True(result.IsCompletedSuccessfully);
            Assert.False(result.IsFaulted);
        }

        [Fact]
        public async void AccountRepository_CreateAccount_ReturnsCreatedAccount()
        {
            //Arrange
            var accountToAdd = new Account()
            {
                Id = 1,
                BankName = "Test",
                BranchCode = "0123",
                Card = "12345643654654654",
                Name = "UpdateTest",
                Number = "123456789123456789",
                Type = AccountType.Savings
            };
            var dbContext = await GetDatabaseContext();
            var accountRepo = new AccountRepository(dbContext);

            //Act
            var result = accountRepo.CreateAsync(accountToAdd);

            //Assert
            result.Should().NotBeNull();
            Assert.True(result.IsCompletedSuccessfully);
            Assert.False(result.IsFaulted);
        }

        [Fact]
        public async void AccountRepository_UpdateAccount_ReturnsUpdatedAccount()
        {
            //Arrange
            var accountToUpdate = new Account()
            {
                Id = 1,
                BankName = "Test",
                BranchCode = "0123",
                Card = "12345643654654654",
                Name = "UpdateTest",
                Number = "123456789123456789",
                Type = AccountType.Savings
            };
            var dbContext = await GetDatabaseContext();
            var accountRepo = new AccountRepository(dbContext);

            //Act
            var result = accountRepo.UpdateAccountAsync(accountToUpdate);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Task<Account>>();
            Assert.True(result.IsCompletedSuccessfully);
            Assert.False(result.IsFaulted);
        }
    }
}
