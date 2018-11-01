using Microsoft.EntityFrameworkCore;
using spellbound_api.Models;
using System.Collections.Generic;

public class SqliteContext : DbContext
    {
        public SqliteContext(DbContextOptions<SqliteContext> options) : base(options)
        { }

        public DbSet<Spell> Spells { get; set; }
    }