using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace spellbound_api.Models
{
    public class Entity
    {
        // [BsonRepresentation(BsonType.ObjectId)]
        [BsonId]
        public ObjectId Id { get; set; }
    }
}