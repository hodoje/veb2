using System;
using WebApp.Models.Dtos;

namespace WebApp.Models.DomainModels
{
    public class ApplicationUserDto
    {        
        public string Email { get; set; }
        public string Name { get; set; }        
        public string Lastname { get; set; }
        public DateTime Birthday { get; set; }
        public string Address { get; set; }
        public bool IsSuccessfullyRegistered { get; set; }
        public string DocumentImage { get; set; }
        public UserTypeDto UserType { get; set; }
    }
}