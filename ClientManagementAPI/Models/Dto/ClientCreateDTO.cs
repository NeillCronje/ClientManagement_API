using System.ComponentModel.DataAnnotations;

namespace ClientManagementAPI.Models.Dto
{
    public class ClientCreateDTO
    {
        [Required]
        [MaxLength(30)]
        public string FirstName { get; set; }
        public string MiddleName { get; set; } = string.Empty;
        [Required]
        [MaxLength(30)]
        public string LastName { get; set; }
        [Required]
        public int CompanyId { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Region { get; set; }
        [Required]
        public string PostalCode { get; set; }
    }
}
