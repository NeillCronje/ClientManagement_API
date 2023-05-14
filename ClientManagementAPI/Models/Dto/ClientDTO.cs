﻿using System.ComponentModel.DataAnnotations;

namespace ClientManagementAPI.Models.Dto
{
    public class ClientDTO
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string FirstName { get; set; }
        public string MiddleName { get; set; } = string.Empty;
        [Required]
        [MaxLength(30)]
        public string LastName { get; set; }
        public int Age { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
    }
}