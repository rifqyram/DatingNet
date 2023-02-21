using Enigma.DatingNet.Repositories;
using Enigma.DatingNet.Repositories.Impls;
using Enigma.DatingNet.Securities;
using Enigma.DatingNet.Services;
using Enigma.DatingNet.Services.Impls;
using Microsoft.EntityFrameworkCore;

namespace Enigma.DatingNet.Extensions;

public static class ManagerDependencyInjection
{
    public static void AddRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(builder =>
            {
                builder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            })
            .AddTransient(typeof(IRepository<>), typeof(Repository<>))
            .AddTransient<IPersistence, DbPersistence>()
            .AddTransient<IPartnerRepository, PartnerRepository>();
    }

    public static void AddServices(this IServiceCollection services)
    {
        services
            .AddTransient<IAuthService, AuthService>()
            .AddTransient<IFileService, FileService>()
            .AddTransient<IMasterInterestService, MasterInterestService>()
            .AddTransient<IMemberContactInfoService, MemberContactInfoService>()
            .AddTransient<IMemberPersonalInfoService, MemberPersonalInfoService>()
            .AddTransient<IMemberPreferencesService, MemberPreferencesService>()
            .AddTransient<IMemberInterestService, MemberInterestService>()
            .AddTransient<IPartnerService, PartnerService>()
            .AddTransient<IJwtUtils, JwtUtils>()
            .AddTransient(typeof(AuthUtil));
    }
}