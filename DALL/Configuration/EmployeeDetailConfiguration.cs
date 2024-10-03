using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configuration
{
    public class EmployeeDetailConfiguration : IEntityTypeConfiguration<EmployeeDetail>
    {
        public void Configure(EntityTypeBuilder<EmployeeDetail> builder)
        {
            builder.ToTable("EmployeeDetails");

            builder.HasKey(ed => ed.EmployeeDetailId);

            builder.Property(ed => ed.Address)
                   .HasMaxLength(200);

            builder.Property(ed => ed.City)
                   .HasMaxLength(100);

            builder.Property(ed => ed.Education)
                   .HasMaxLength(200);

            builder.Property(ed => ed.Certifications)
                   .HasMaxLength(200);

            builder.Property(ed => ed.Experience)
                   .HasMaxLength(500);

            builder.Property(ed => ed.Position)
                   .HasMaxLength(50);

            builder.Property(ed => ed.Department)
                   .HasMaxLength(50);

            builder.Property(ed => ed.RemainingLeaveDays)
                   .IsRequired()
                   .HasDefaultValue(0);

            // Relationships
            builder.HasOne(ed => ed.Employee)
                   .WithOne()
                   .HasForeignKey<EmployeeDetail>(ed => ed.EmployeeId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
