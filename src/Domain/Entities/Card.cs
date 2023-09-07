
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;

namespace Domain.Entities
{
    public class Card : IEntity<int>
    {
        public Card()
        {
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(15), MinLength(15)]
        public string Number { get; set; }

        [Required]
        [MaxLength(100)]
        public string CardHolderName { get; set; }

        [Required]
        public int ExpirationMonth { get; set; }

        [Required]
        public int ExpirationtYear { get; set; }

        [Required]
        [MaxLength(3), MinLength(3)]
        public string CVC { get; set; }



        [NotMapped]
        public string Expiration => $"{ExpirationMonth}/{ExpirationtYear}";

    }
}
