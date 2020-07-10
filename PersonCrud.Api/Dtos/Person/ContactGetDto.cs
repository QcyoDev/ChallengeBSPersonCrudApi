using PersonCrud.Api.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PersonCrud.Api.Dtos
{
    public class ContactGetDto
    {
        public string ContactType { get; set; }
        public string ContactInfo { get; set; }
    }
}