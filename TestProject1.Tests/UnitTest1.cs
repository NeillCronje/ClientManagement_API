using ClientManagementAPI.Models;
using ClientManagementAPI.Repository.IRepository;
using Moq;

namespace TestProject1.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
        }
        [TestMethod]
        public async Task ClientController_GetClients_ReturnOK()
        {
            //Arrange
            var clients = new Mock<IClientRepository>();
            clients.Setup(c => c.GetAllAsync(null))
                .Returns(() => new List<Client> { });

            //Act
            var resultList = await clients.Object.GetAllAsync();

            //Assert
            CollectionAssert.AllItemsAreNotNull(resultList);
            CollectionAssert.AreEquivalent(new List<Client> { }, resultList);
        }
    }
}