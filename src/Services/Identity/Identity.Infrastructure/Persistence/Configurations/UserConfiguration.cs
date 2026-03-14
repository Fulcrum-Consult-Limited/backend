namespace Identity.Infrastructure.Persistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(u => u.PasswordHash)
            .HasColumnName("password_hash")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(u => u.Forename)
            .HasColumnName("forename")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(u => u.Surname)
            .HasColumnName("surname")
            .HasMaxLength(100)
            .IsRequired();

        builder.Ignore(u => u.FullName);

        builder.Property(u => u.Role)
            .HasColumnName("role")
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(u => u.IsActive)
            .HasColumnName("is_active")
            .IsRequired();

        builder.Property(u => u.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(u => u.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(u => u.LastLoginAt)
            .HasColumnName("last_login_at");

        builder.Property(u => u.Email)
            .HasColumnName("email")
            .HasMaxLength(255)
            .IsRequired()
            .HasConversion(
                email => email.Value,
                value => Email.Create(value));

        builder.HasIndex(u => u.Email)
            .IsUnique()
            .HasDatabaseName("ix_users_email");

        builder.Ignore(u => u.DomainEvents);
    }
}