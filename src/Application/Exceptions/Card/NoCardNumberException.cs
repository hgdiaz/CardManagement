using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions.Card
{
    public class NoCardNumberException : Exception
    {
        public NoCardNumberException(string number) : base($"Card number {number} doesn't exist.") { }
    }
}
