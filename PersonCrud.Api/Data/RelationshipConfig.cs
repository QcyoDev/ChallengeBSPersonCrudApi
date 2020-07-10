using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonCrud.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonCrud.Api.Data
{
    public class RelationshipConfig : IEntityTypeConfiguration<Relationship>
    {

        public void Configure(EntityTypeBuilder<Relationship> builder)
        {

            builder.HasKey(r => new { r.PersonId, r.RelatedPersonId });

            builder.HasOne(r => r.RelatedPerson).WithMany().HasForeignKey(r => r.RelatedPersonId).OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(r => r.Person).WithMany(p => p.Relationships).HasForeignKey(pt => pt.PersonId);
        }
    }
}
