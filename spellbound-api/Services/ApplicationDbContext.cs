using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using spellbound_api.Models;
using System;
using System.Collections.Generic;
using System.IO;

public class ApplicationDbContext : IdentityDbContext<User>
{
  public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
  { }

  public DbSet<Spell> Spells { get; set; }
  public DbSet<Character> Characters { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.Entity<CharacterSpell>()
      .HasKey(x => new { x.CharacterId, x.SpellId });

    modelBuilder.Entity<CharacterSpell>()
      .HasOne(x => x.Character)
      .WithMany(x => x.CharacterSpells)
      .HasForeignKey(x => x.CharacterId);

    modelBuilder.Entity<CharacterSpell>()
      .HasOne(x => x.Spell)
      .WithMany(x => x.CharacterSpells)
      .HasForeignKey(x => x.SpellId);

    var adminId = Guid.NewGuid().ToString();
    var userId = Guid.NewGuid().ToString();
    modelBuilder.Entity<IdentityRole>().HasData(new { Id = adminId, Name = "Admin", NormalizedName = "ADMIN" });
    modelBuilder.Entity<IdentityRole>().HasData(new { Id = userId, Name = "User", NormalizedName = "USER" });
  }

}