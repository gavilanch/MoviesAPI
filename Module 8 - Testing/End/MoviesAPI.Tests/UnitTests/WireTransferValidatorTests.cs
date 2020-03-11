using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoviesAPI.Testing;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoviesAPI.Tests.UnitTests
{
    [TestClass]
    public class WireTransferValidatorTests
    {
        [TestMethod]
        public void ValidateReturnsErrorWhenInsufficientFunds()
        {
            Account origin = new Account() { Funds = 0 };
            Account destination = new Account() { Funds = 0 };
            decimal amountToTransfer = 5m;

            var services = new WireTransferValidator();

            var result = services.Validate(origin, destination, amountToTransfer);

            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual("The origin account does not have enough funds available", result.ErrorMessage);
        }

        [TestMethod]
        public void ValidateReturnsSuccessfulOperation()
        {
            Account origin = new Account() { Funds = 7 };
            Account destination = new Account() { Funds = 0 };
            decimal amountToTransfer = 5m;

            var services = new WireTransferValidator();

            var result = services.Validate(origin, destination, amountToTransfer);

            Assert.IsTrue(result.IsSuccessful);
        }

    }
}
