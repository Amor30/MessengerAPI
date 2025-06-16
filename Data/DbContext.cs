using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MessengerAPI.Models;

namespace MessengerAPI.Models;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Type_chat> Type_Chats { get; set; }
    public DbSet<Chat> Chats { get; set; }
    public DbSet<User_chats> User_Chats { get; set; }
    public DbSet<Message> Messages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Составной ключ для User_chats
        modelBuilder.Entity<User_chats>()
            .HasKey(uc => new { uc.Id_user, uc.Id_chat });

        // Отношения для User_chats
        modelBuilder.Entity<User_chats>()
            .HasOne(uc => uc.User)
            .WithMany(u => u.UserChats)
            .HasForeignKey(uc => uc.Id_user);

        modelBuilder.Entity<User_chats>()
            .HasOne(uc => uc.Chat)
            .WithMany(c => c.UserChats)
            .HasForeignKey(uc => uc.Id_chat);

        // Отношение между Chat и Type_chat
        modelBuilder.Entity<Chat>()
            .HasOne(c => c.TypeChat)
            .WithMany()
            .HasForeignKey(c => c.Id_type_chat);

        // Отношения для Message
        modelBuilder.Entity<Message>()
            .HasOne(m => m.User)
            .WithMany()
            .HasForeignKey(m => m.Id_user);

        modelBuilder.Entity<Message>()
            .HasOne(m => m.Chat)
            .WithMany(c => c.Messages)
            .HasForeignKey(m => m.Id_chat);

        // Ограничения для ApplicationUser
        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.Property(e => e.UserName).HasMaxLength(20);
            entity.Property(e => e.Email).HasMaxLength(100);
        });

        // Уникальность email
        modelBuilder.Entity<ApplicationUser>()
            .HasIndex(u => u.Email)
            .IsUnique();
    }
}