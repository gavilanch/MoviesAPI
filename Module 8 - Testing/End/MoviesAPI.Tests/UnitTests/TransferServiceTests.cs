using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MoviesAPI.Testing;
using System;

namespace MoviesAPI.Tests.UnitTests
{
    [TestClass]
    public class TransferServiceTests
    {
        [TestMethod]
        public void WireTransferWithInsufficientFundsThrowsAnError()
        {
            // Preparation
            Account origin = new Account() { Funds = 0 };
            Account destination = new Account() { Funds = 0 };
            decimal amountToTransfer = 5m;
            string errorMessage = "custom error message";
            var mockValidateWireTransfer = new Mock<IValidateWireTransfer>();
            mockValidateWireTransfer.Setup(x => x.Validate(origin, destination, amountToTransfer))
                .Returns(new OperationResult(false, errorMessage));

            var service = new TransferService(mockValidateWireTransfer.Object);
            Exception expectedException = null;

            // Testing
            try
            {
                service.WireTransfer(origin, destination, amountToTransfer);
            }
            catch (Exception ex)
            {
                expectedException = ex;
            }

            // Verification

            if (expectedException == null)
            {
                Assert.Fail("An exception was expected");
            }

            Assert.IsTrue(expectedException is ApplicationException);
            Assert.AreEqual(errorMessage, expectedException.Message);
        }

        [TestMethod]
        public void WireTransferCorrectlyEditFunds()
        {
            // Preparation
            Account origin = new Account() { Funds = 10 };
            Account destination = new Account() { Funds = 5 };
            decimal amountToTransfer = 7m;
            var mockValidateWireTransfer = new Mock<IValidateWireTransfer>();
            mockValidateWireTransfer.Setup(x => x.Validate(origin, destination, amountToTransfer))
                .Returns(new OperationResult(true));

            var service = new TransferService(mockValidateWireTransfer.Object);

            // Testing
            service.WireTransfer(origin, destination, amountToTransfer);

            // Verification
            Assert.AreEqual(3, origin.Funds);
            Assert.AreEqual(12, destination.Funds);
        }

    }
}
