using Enigma.DatingNet.Entities;
using Microsoft.EntityFrameworkCore;

namespace Enigma.DatingNet.Repositories;

public class AppDbContext : DbContext
{
    public DbSet<MemberUserAccess> MemberUserAccesses => Set<MemberUserAccess>();
    public DbSet<MemberContactInformation> MemberContactInformation => Set<MemberContactInformation>();
    public DbSet<MemberPersonalInformation> MemberPersonalInformation => Set<MemberPersonalInformation>();
    public DbSet<MemberPreferences> MemberPreferences => Set<MemberPreferences>();
    public DbSet<MasterInterest> MasterInterests => Set<MasterInterest>();
    public DbSet<MemberInterest> MemberInterest => Set<MemberInterest>();
    public DbSet<MemberPartner> MemberPartners => Set<MemberPartner>();

    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    protected AppDbContext()
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MemberInterest>()
            .HasOne(mi => mi.Member)
            .WithMany(access => access.MemberInterests)
            .HasForeignKey(mi => mi.MemberId);
        modelBuilder.Entity<MemberInterest>()
            .HasOne(mi => mi.Interest)
            .WithMany(interest => interest.MemberInterests)
            .HasForeignKey(mi => mi.InterestId);

        modelBuilder.Entity<MemberPartner>()
            .HasOne(mp => mp.Member)
            .WithMany(access => access.Members)
            .HasForeignKey(mp => mp.MemberId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<MemberPartner>()
            .HasOne(mp => mp.Partner)
            .WithMany(access => access.Partners)
            .HasForeignKey(mp => mp.PartnerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}