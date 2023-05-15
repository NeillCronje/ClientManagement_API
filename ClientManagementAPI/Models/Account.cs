using ClientManagementAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClientManagementAPI.Models
{
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Client")]
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
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
