using PersonCrud.Api.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PersonCrud.Api.Dtos
{
    public class PersonPutDto
    {
        public int PersonId { get; set; }

        public string DocType { get; set; }

        public string DocNum { get; set; }

        public string Country { get; set; }

        [Range(0, 1, ErrorMessage = "El valor debe coincidir con la enumeracion")]
        public Gender Gender { get; set; }

        [Range(18, int.MaxValue, ErrorMessage = "La persona debe ser mayor de 18 años")]
        public int Age { get; set; }

        public List<ContactPostDto> ContactDetails { get; set; }
    }
}
