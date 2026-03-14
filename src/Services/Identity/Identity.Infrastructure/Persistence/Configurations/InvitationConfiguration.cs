namespace Identity.Infrastructure.Persistence.Configurations;

public sealed class InvitationConfiguration : IEntityTypeConfiguration<Invitation>
{
    public void Configure(EntityTypeBuilder<Invitation> builder)
    {
        builder.ToTable("invitations");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(i => i.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(i => i.Email)
            .HasColumnName("email")
            .HasMaxLength(255)
            .IsRequired()
            .HasConversion(
                email => email.Value,
                value => Email.Create(value));

        builder.Property(i => i.Token)
            .HasColumnName("token")
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(i => i.ExpiresAt)
            .HasColumnName("expires_at")
            .IsRequired();

        builder.Property(i => i.IsUsed)
            .HasColumnName("is_used")
            .IsRequired();

        builder.Property(i => i.UsedAt)
            .HasColumnName("used_at");

        builder.Property(i => i.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(i => i.UpdatedAt)
            .HasColumnName("updated_at");

        builder.HasIndex(i => i.Token)
            .IsUnique()
            .HasDatabaseName("ix_invitations_token");

        builder.HasIndex(i => i.UserId)
            .HasDatabaseName("ix_invitations_user_id");

        builder.Ignore(i => i.DomainEvents);
    }
}