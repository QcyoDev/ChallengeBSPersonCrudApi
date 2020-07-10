﻿using PersonCrud.Api.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PersonCrud.Api.Dtos
{
    public class ContactPostDto
    {
        public ContactType ContactType { get; set; }
        public string ContactInfo { get; set; }
    }
}