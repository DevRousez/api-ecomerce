using Api_comerce.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api_comerce.Dtos
{
    public class AccountsDtocs
    {
        public int Id { get; set; }
        public int AccountTypeId { get; set; }
              
        public string FullName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string? GoogleId { get; set; }
        public string? Picture { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

      
        public AccountsTypes AccountsTypes { get; set; }

             
    }
    public class AccountDto
    {
        public int Id { get; set; }
        public int AccountTypeId { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string? GoogleId { get; set; }
        public string? Picture { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }


        public AccountsTypes AccountsTypes { get; set; }


    }

}
