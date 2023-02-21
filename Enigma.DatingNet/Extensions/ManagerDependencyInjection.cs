using Enigma.DatingNet.Repositories;
using Enigma.DatingNet.Repositories.Impls;
using Enigma.DatingNet.Services;
using Enigma.DatingNet.Services.Impls;
using Enigma.DatingNet.Utils;
using Microsoft.EntityFrameworkCore;

namespace Enigma.DatingNet;

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
            .AddTransient(typeof(AuthUtil));
    }
}