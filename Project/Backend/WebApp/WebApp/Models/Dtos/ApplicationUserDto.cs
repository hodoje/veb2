using Newtonsoft.Json;
using System;
using WebApp.Models.Dtos;

namespace WebApp.Models.DomainModels
{
    public class ApplicationUserDto
    {
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("lastname")]
        public string Lastname { get; set; }
        [JsonProperty("birthday")]
        public DateTime Birthday { get; set; }
        [JsonProperty("address")]
        public string Address { get; set; }
        [JsonProperty("isSuccessfullyRegistered")]
        public bool IsSuccessfullyRegistered { get; set; }
        [JsonProperty("documentImage")]
        public string DocumentImage { get; set; }
        [JsonProperty("userType")]
        public UserTypeDto UserType { get; set; }
    }
}