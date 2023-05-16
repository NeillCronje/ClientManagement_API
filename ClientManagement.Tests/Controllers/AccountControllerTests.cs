using AutoMapper;
using ClientManagementAPI.Controllers;
using ClientManagementAPI.Models;
using ClientManagementAPI.Models.Dto;
using ClientManagementAPI.Models.Enums;
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
    public class AccountControllerTests
    {
        private readonly IAccountRepository _dbAccount;
        private readonly IMapper _mapper;
        public AccountControllerTests()
        {
            _dbAccount = A.Fake<IAccountRepository>();
            _mapper = A.Fake<IMapper>();
        }

        [Fact]
        public void AccountController_GetClients_ReturnOK()
        {
            //Arrange
            var accounts = A.Fake<ICollection<AccountDTO>>();
            var accountList = A.Fake<List<AccountDTO>>();
            A.CallTo(() => _mapper.Map<List<AccountDTO>>(accounts)).Returns(accountList);
            var controller = new AccountController(_dbAccount, _mapper);

            //Act
            var result = controller.GetAccounts();

            //Assert
            result.Should().NotBeNull();
            Assert.False(result.IsFaulted);
            Assert.True(result.IsCompletedSuccessfully);
        }

        [Fact]
        public void AccountController_GetClient_ReturnOK()
        {
            //Arrange
            int id = 1;
            var client = A.Fake<AccountDTO>();
            A.CallTo(() => _mapper.Map<AccountDTO>(client)).Returns(client);
            var controller = new AccountController(_dbAccount, _mapper);

            //Act
            var result = controller.GetAccount(id);

            //Assert
            result.Should().NotBeNull();
            Assert.False(result.IsFaulted);
            Assert.True(result.IsCompletedSuccessfully);
        }

        [Fact]
        public void AccountController_RemoveClient_ReturnOK()
        {
            //Arrange
            int id = 1;
            var client = A.Fake<AccountDTO>();
            A.CallTo(() => _mapper.Map<AccountDTO>(client));
            var controller = new AccountController(_dbAccount, _mapper);

            //Act
            var result = controller.DeleteAccount(id);

            //Assert
            result.Should().NotBeNull();
            Assert.False(result.IsFaulted);
            Assert.True(result.IsCompletedSuccessfully);
        }

        [Fact]
        public void AccountController_UpdateClient_ReturnOK()
        {
            //Arrange
            AccountUpdateDTO updateAccount = new AccountUpdateDTO()
            {
                Id = 1,
                BankName = "Test",
                BranchCode = "0123",
                Card = "12345643654654654",
                Name = "UpdateTest",
                Number = "123456789123456789",
                Type = AccountType.Savings
            };
            var account = A.Fake<AccountDTO>();
            A.CallTo(() => _mapper.Map<AccountDTO>(account));
            var controller = new AccountController(_dbAccount, _mapper);

            //Act
            var result = controller.UpdateAccount(updateAccount.Id, updateAccount);

            //Assert
            result.Should().NotBeNull();
            Assert.False(result.IsFaulted);
            Assert.True(result.IsCompletedSuccessfully);
        }
    }
}
