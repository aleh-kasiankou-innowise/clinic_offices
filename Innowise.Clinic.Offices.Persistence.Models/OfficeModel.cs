﻿using MongoDB.Bson.Serialization.Attributes;

namespace Innowise.Clinic.Offices.Persistence.Models;

public class OfficeModel
{
    [BsonId]
    public Guid Id { get; set; }
    public string RegistryPhone { get; set; } = null!;
    public OfficeStatus OfficeStatus { get; set; }
    public OfficeAddressModel OfficeAddress { get; set; } = null!;
    
}