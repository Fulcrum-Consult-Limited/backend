namespace Identity.Infrastructure.Persistence.Configurations;

public sealed class PasswordResetTokenConfiguration : IEntityTypeConfiguration<PasswordResetToken>
{
    public void Configure(EntityTypeBuilder<PasswordResetToken> builder)
    {
        builder.ToTable("password_reset_tokens");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(p => p.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(p => p.Email)
            .HasColumnName("email")
            .HasMaxLength(255)
            .IsRequired()
            .HasConversion(
                email => email.Value,
                value => Email.Create(value));

        builder.Property(p => p.Token)
            .HasColumnName("token")
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(p => p.ExpiresAt)
            .HasColumnName("expires_at")
            .IsRequired();

        builder.Property(p => p.IsUsed)
            .HasColumnName("is_used")
            .IsRequired();

        builder.Property(p => p.UsedAt)
            .HasColumnName("used_at");

        builder.Property(p => p.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(p => p.UpdatedAt)
            .HasColumnName("updated_at");

        builder.HasIndex(p => p.Token)
            .IsUnique()
            .HasDatabaseName("ix_password_reset_tokens_token");

        builder.HasIndex(p => p.UserId)
            .HasDatabaseName("ix_password_reset_tokens_user_id");

        builder.Ignore(p => p.DomainEvents);
    }
}