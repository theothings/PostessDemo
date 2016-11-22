using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PostService.LobAPI;
using PostServiceTests.Properties;

namespace PostServiceTests
{
    [TestClass]
    public class LobTest
    {
        private dynamic address1 = new
        {
            name = "test address",
            company = "test company",
            address_line1 = "test address line 1",
            address_city = "test city",
            address_country = "GB"
        };

        [TestMethod]
        public void CreateAddress()
        {
            var lob = new Lob(Settings.Default.LobTestApiKey);
            var newAddress = lob.Addresses.Create(address1);

            Assert.AreEqual(address1.name, (string)newAddress.name, "name of newly created address does not match");
            Assert.AreEqual(address1.address_line1, (string)newAddress.address_line1, "address_line1 of newly created address does not match");
            
        }
    }
}
