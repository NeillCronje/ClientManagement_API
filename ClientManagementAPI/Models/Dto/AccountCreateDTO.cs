using ClientManagementAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClientManagementAPI.Models.Dto
{
    public class AccountCreateDTO
    {
        public int ClientId { get; set; }
        [Required]
        public string BankName { get; set; }
        [Required]
        public string Number { get; set; }
        [Required]
        public AccountType Type { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string BranchCode { get; set; }
        [Required]
        public string Card { get; set; }
    }
}
