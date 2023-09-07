using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions.Card
{
    public class CardDataNotValidException : Exception
    {
        public CardDataNotValidException(string messages) : base($"Please correct these errors: {messages}") { }
    }
}
