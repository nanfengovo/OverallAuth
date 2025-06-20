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
    internal class RoleConfig : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("T_Role");

            builder.HasMany(m => m.Menus)
                .WithMany(r => r.Roles)
                .UsingEntity(t => t.ToTable("T_RoleMenu"));
        }
    }
}
