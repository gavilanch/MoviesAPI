using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.Testing
{
    public class WireTransferValidator : IValidateWireTransfer
    {
        public OperationResult Validate(Account origin, Account destination, decimal amount)
        {
            if (amount > origin.Funds)
            {
                return new OperationResult(false, "The origin account does not have enough funds available");
            }

            // other validations

            return new OperationResult(true);
        }
    }
}
