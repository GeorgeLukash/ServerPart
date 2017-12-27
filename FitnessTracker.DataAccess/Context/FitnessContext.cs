using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Reflection;

namespace FitnessTracker.DataAccess
{
    public class FitnessContext : DbContext
    {
        //CHANGE YOUR CONNECTION
        public FitnessContext()
            : base("Data Source=(localdb)\\v11.0;Initial Catalog=Fitness;Integrated Security=True;MultipleActiveResultSets=True")
        {
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<FitnessContext, >());
        }

        public new DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => !string.IsNullOrEmpty(type.Namespace))
                .Where(type => type.BaseType != null && type.BaseType.IsGenericType
                     && type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));
            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }
            base.OnModelCreating(modelBuilder);
        }
    }
}
