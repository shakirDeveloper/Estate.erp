using Estate.erp.dal.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estate.erp.dal.Mapping
{
    public class UserProfileMap : EntityTypeConfiguration<UserProfile>
    {
        public UserProfileMap()
        {
            //key  
            HasKey(t => t.ID);
            //properties             
            Property(t => t.FirstName).IsRequired().HasMaxLength(100).HasColumnType("nvarchar");
            Property(t => t.LastName).IsRequired().HasMaxLength(100).HasColumnType("nvarchar");
            Property(t => t.Address).IsRequired().HasMaxLength(200).HasColumnType("nvarchar");
            Property(t => t.CreatedDate).IsRequired();
            Property(t => t.UpdatedDate).IsRequired();
            Property(t => t.CreatedBy);
            //table  
            ToTable("UserProfile");
            //relation            
            HasRequired(s => s.User)
            .WithMany(g => g.UserProfile)
            .HasForeignKey<int>(s => s.UserId);
        }
    }
}
