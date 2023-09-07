using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions.Card
{
    public class NoCardIdException : Exception
    {
        public NoCardIdException() : base($"The requested card doesn't exist.") { }
    }
}
