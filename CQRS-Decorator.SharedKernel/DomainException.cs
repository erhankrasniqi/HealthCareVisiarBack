using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS_Decorator.SharedKernel
{
    public class DomainException : Exception
    {
        public readonly int ErrorCode;

        public DomainException()
            : base("Domain exception.")
        {
            ErrorCode = ReservedErrorCode.SystemError;
        }

        public DomainException(string message)
            : base(message)
        {
            ErrorCode = ReservedErrorCode.SystemError;
        }

        public DomainException(int errorCode, string message)
            : base(message)
        {
            ErrorCode = errorCode;
        }

        public DomainException(int errorCode, string message, Exception innerException)
            : base(message, innerException)
        {
            ErrorCode = errorCode;
        }
    }
}
