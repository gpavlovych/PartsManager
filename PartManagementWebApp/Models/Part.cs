using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartManagementWebApp.Models
{
    public class Part
    {
        public int Id { get; set; }

        public string PartNumber { get; set; }

        public ApplicationUser Owner { get; set; }

        public byte[] Datasheet { get; set; }
    }

    public class PartConfiguration : EntityTypeConfiguration<Part>
    {
        public PartConfiguration()
        {
            ToTable("parts");
            Property(p => p.PartNumber).HasMaxLength(20);
            HasKey(p => p.Id);
            HasRequired(it => it.Owner);
        }
    }
}
