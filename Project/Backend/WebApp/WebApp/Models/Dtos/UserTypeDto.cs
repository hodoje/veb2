using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.Dtos
{
    public class UserTypeDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}