using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OverallAuthDEMO.EFcore.Model;

namespace OverallAuthDEMO.EFcore.Config
{
    internal class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("T_User");

            builder.HasMany(r => r.Roles)
                .WithMany(u => u.Users)
                .UsingEntity(t => t.ToTable("T_UserRole"));
        }

        
    }
}
