using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PersonCrud.Api.Models
{
    public class Relationship
    {
        [ForeignKey("Person")]
        public int PersonId { get; set; }
        public Person Person { get; set; }

        [ForeignKey("RelatedPerson")]
        public int RelatedPersonId { get; set; }
        public Person RelatedPerson { get; set; }

        public Relations RelationType { get; set; }
        
    }
}
