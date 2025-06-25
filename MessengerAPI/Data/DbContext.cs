using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MessengerAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace MessengerAPI.Models;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Type_chat> TypeChats { get; set; }
    public DbSet<Chat> Chats { get; set; }
    public DbSet<User_chats> UserChats { get; set; }
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
            .HasForeignKey(uc => uc.Id_user)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User_chats>()
            .HasOne(uc => uc.Chat)
            .WithMany(c => c.UserChats)
            .HasForeignKey(uc => uc.Id_chat)
            .OnDelete(DeleteBehavior.Cascade);

        // Отношение между Chat и Type_chat
        modelBuilder.Entity<Chat>()
            .HasOne(c => c.TypeChat)
            .WithMany() // Нет обратной коллекции в Type_chat
            .HasForeignKey(c => c.Id_type_chat)
            .OnDelete(DeleteBehavior.Restrict);

        // Отношения для Message
        modelBuilder.Entity<Message>()
            .HasOne(m => m.User)
            .WithMany()
            .HasForeignKey(m => m.Id_user)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Message>()
            .HasOne(m => m.Chat)
            .WithMany(c => c.Messages)
            .HasForeignKey(m => m.Id_chat)
            .OnDelete(DeleteBehavior.Cascade);

        // Конфигурация ApplicationUser
        modelBuilder.Entity<ApplicationUser>()
            .Property(u => u.UserName)
            .HasMaxLength(20);

        modelBuilder.Entity<ApplicationUser>()
            .Property(u => u.Email)
            .HasMaxLength(100);

        // Уникальность Email
        modelBuilder.Entity<ApplicationUser>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Конфигурация полей моделей
        modelBuilder.Entity<Chat>()
            .Property(c => c.Chat_name)
            .HasMaxLength(20);

        modelBuilder.Entity<Message>()
            .Property(m => m.Msg_text)
            .HasMaxLength(500);

        modelBuilder.Entity<Message>()
            .Property(m => m.User_name)
            .HasMaxLength(20);

        modelBuilder.Entity<Type_chat>()
            .Property(t => t.Name_type)
            .HasMaxLength(10);
    }
}