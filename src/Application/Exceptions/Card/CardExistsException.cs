using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions.Card
{
    public class CardExistsException : Exception
    {
        public CardExistsException(string number) : base($"Card number {number} already exist.") { }
    }
}
