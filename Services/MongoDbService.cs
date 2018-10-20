using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using spellbound_api.Models;

namespace spellbound_api.Services
{
  public class MongoDbService : IDataService
  {
    private readonly MongoDbOptions _options;
    private readonly IMongoDatabase _database;

    public MongoDbService(IOptions<MongoDbOptions> options)
    {
      _options = options?.Value ?? throw new ArgumentNullException(nameof(options));

      var conventionPack = new  ConventionPack {new CamelCaseElementNameConvention()};
      ConventionRegistry.Register("camelCase", conventionPack, t => true);
      var client = new MongoClient(_options.ConnectionString);
      if (client != null)
        _database = client.GetDatabase(_options.DatabaseName);
    }

    private IMongoCollection<Spell> Spells
    {
      get
      {
        return _database.GetCollection<Spell>("spells");
      }
    }

    public async Task<IEnumerable<Spell>> GetSpells() {
        return await Spells.Find(_ => true).ToListAsync();
    }

  }
}