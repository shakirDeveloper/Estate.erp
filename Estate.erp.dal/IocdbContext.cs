using Estate.erp.dal.Mapping;
using Estate.erp.dal.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
//using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace Estate.erp.dal
{
    public class IocDbContext : DbContext, IDbContext
    {
        public IocDbContext()
        {
        }
        public IocDbContext(DbContextOptions<IocDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> User { get; set; } = null!;
        //public virtual DbSet<UserProfile> UserProfile { get; set; } = null!;
        //public virtual DbSet<Role> Role { get; set; } = null!;
            
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            //var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
            //    .Where(type => !String.IsNullOrEmpty(type.Namespace))
            //    .Where(type => type.BaseType != null && type.BaseType.IsGenericType &&
            //                   type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));
            //foreach (var type in typesToRegister)
            //{
            //    dynamic configInstance = Activator.CreateInstance(type);
            //    modelBuilder.ApplyConfiguration(configInstance);
            //}

        }

        public new DbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
        {
            return base.Set<TEntity>();
        }
    }
}
