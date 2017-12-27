using FitnessTracker.DataAccess;
using FitnessTracker.Operations.Abstraction;
using FitnessTracker.Operations.Implementation;
using FitnessTracker.SecurityContext.Abstraction;
using FitnessTracker.SecurityContext.Implementation;
using LightInject;

namespace FitnessTracker.CompositionRoot
{
    public class Bootstrap
    {
        public static void Configure(ServiceContainer container)
        {
            // Common
            container.Register<ICrypthographyContext, CrypthographyContext>(new PerRequestLifeTime());
            container.Register<IPasswordContext, PasswordContext>(new PerRequestLifeTime());

            container.Register<FitnessContext, FitnessContext>();
            container.Register<IUnitOfWork, UnitOfWork>();
            container.Register(typeof(IRepository<>), typeof(Repository<>), new PerRequestLifeTime());

            container.Register<IAuthenticationOperations, AuthenticationOperations>(new PerRequestLifeTime());
            container.Register<IMyPlanOperations, MyPlanOperations>(new PerRequestLifeTime());
            container.Register<IUserProfileOperations, UserProfileOperations>(new PerRequestLifeTime());
            container.Register<IExpirationTokenOperations, ExpirationTokenOperations>(new PerRequestLifeTime());
            container.Register<INewsOperations, NewsOperations>(new PerRequestLifeTime());
            container.Register<IAdminOperations, AdminOperations>(new PerRequestLifeTime());

            container.Register<IBlockOperations, BlockOperations>(new PerRequestLifeTime());
        }
    }
}