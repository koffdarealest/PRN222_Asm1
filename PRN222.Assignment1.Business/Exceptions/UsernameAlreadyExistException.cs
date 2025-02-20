using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.Assignment1.Business.Exceptions
{
    public class UsernameAlreadyExistException : Exception
    {
        public UsernameAlreadyExistException(string message) : base(message) { }
    }
}
