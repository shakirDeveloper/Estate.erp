using Estate.erp.dal.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estate.erp.dal.Mapping
{
    public class RoleMap : EntityTypeConfiguration<Role>
    {
        public RoleMap()
        //public override void Map(EntityTypeBuilder<Role> builder)
        {
            //key  
            HasKey(t => t.RoleId);
            //properties             
            Property(t => t.Description).HasMaxLength(100).HasColumnType("nvarchar").IsRequired();
            //table  
            ToTable("Role");

            HasMany(g => g.User)
             .WithRequired(s => s.Role)
             .HasForeignKey(s => s.RoleId);
        }
    }
}
