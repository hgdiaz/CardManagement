using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.Repositories;
using Dapper;
using Domain.Entities;
using Infrastructure.Contexts;

namespace Infrastructure.Repositories
{
    public class CardRepository: ICardRepository
    {
        private readonly DapperContext _context;

        public CardRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Card>> GetAllAsync()
        {
            var query = "SELECT * FROM Cards";
            using (var connection = _context.CreateConnection())
            {
                var items = await connection.QueryAsync<Card>(query);
                return items.ToList();
            }
        }

        public async Task<Card> GetByNumberAsync(string number) 
        {
            var query = "SELECT * FROM Cards WHERE Number = @number";
            using (var connection = _context.CreateConnection())
            {
                var item = await connection.QuerySingleOrDefaultAsync<Card>(query, new { number });
                return item;
            }
        }

        public async Task<Card> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM Cards WHERE Id = @id";
            using (var connection = _context.CreateConnection())
            {
                var item = await connection.QuerySingleOrDefaultAsync<Card>(query, new { id });
                return item;
            }
        }

        public async Task<Card> AddAsync(Card card)
        {
            var query = "INSERT INTO Cards (Number, CardHolderName, ExpirationMonth, ExpirationtYear, CVC) VALUES (@Number, @CardHolderName, @ExpirationMonth, @ExpirationtYear, @CVC)" +
                        "SELECT CAST(SCOPE_IDENTITY() as int)";
            var parameters = new DynamicParameters();
            parameters.Add("Number", card.Number, DbType.String);
            parameters.Add("CardHolderName", card.CardHolderName, DbType.String);
            parameters.Add("ExpirationMonth", card.ExpirationMonth, DbType.Int32);
            parameters.Add("ExpirationtYear", card.ExpirationtYear, DbType.Int32);
            parameters.Add("CVC", card.CVC, DbType.String);
            using (var connection = _context.CreateConnection())
            {
                var id = await connection.QuerySingleAsync<int>(query, parameters);
                var createdCard = new Card
                {
                    Id = id,
                    Number = card.Number,
                    CardHolderName = card.CardHolderName,
                    ExpirationMonth = card.ExpirationMonth,
                    ExpirationtYear = card.ExpirationtYear,
                    CVC = card.CVC
                };
                return createdCard;
            }
        }

        public async Task<int> UpdateCardAsync(Card card)
        {
            int affectedRows = 0;
            var query = "UPDATE Cards SET Number = @Number, CardHolderName = @CardHolderName, ExpirationMonth = @ExpirationMonth, ExpirationtYear = @ExpirationtYear, CVC = @CVC  WHERE Id = @Id";
            var parameters = new DynamicParameters();
            parameters.Add("Id", card.Id, DbType.Int32);
            parameters.Add("Number", card.Number, DbType.String);
            parameters.Add("CardHolderName", card.CardHolderName, DbType.String);
            parameters.Add("ExpirationMonth", card.ExpirationMonth, DbType.Int32);
            parameters.Add("ExpirationtYear", card.ExpirationtYear, DbType.Int32);
            parameters.Add("CVC", card.CVC, DbType.String);
            using (var connection = _context.CreateConnection())
            {
                affectedRows = await connection.ExecuteAsync(query, parameters);
            }
            return affectedRows;
        }

        public async Task<int> DeleteCardAsync(int id)
        {
            int affectedRows = 0;
            var query = "DELETE FROM Cards WHERE Id = @Id";
            using (var connection = _context.CreateConnection())
            {
                affectedRows = await connection.ExecuteAsync(query, new { id });
            }
            return affectedRows;
        }
    }
}
