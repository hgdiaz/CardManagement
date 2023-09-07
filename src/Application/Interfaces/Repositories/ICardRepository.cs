using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface ICardRepository
    {
        Task<IEnumerable<Card>> GetAllAsync();
        Task<Card> GetByNumberAsync(string number);
        Task<Card> GetByIdAsync(int id);
        Task<Card> AddAsync(Card card);
        Task<int> UpdateCardAsync(Card card);
        Task<int> DeleteCardAsync(int id);
    }
}
