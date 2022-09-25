using Estate.erp.dal.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estate.erp.dal.Mapping
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            //key  
            HasKey(t => t.ID);
            //properties             
            Property(t => t.Email).HasMaxLength(100).HasColumnType("nvarchar").IsRequired();
            Property(t => t.ApplicationUserId).HasMaxLength(255).HasColumnType("nvarchar").IsRequired();
            Property(t => t.FirstName).HasMaxLength(100).HasColumnType("nvarchar").IsRequired();
            Property(t => t.LastName).HasMaxLength(100).HasColumnType("nvarchar").IsRequired();
            Property(t => t.Address).HasMaxLength(200).HasColumnType("nvarchar").IsRequired();

            Property(t => t.CreatedDate).IsOptional();
            Property(t => t.UpdatedDate).IsOptional();
            Property(t => t.CreatedBy).IsOptional();
            //table  
            ToTable("User");          
        }
    }
}
