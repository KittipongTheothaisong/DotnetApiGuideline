using DotnetApiGuideline.Sources.Domain.Entities;
using DotnetApiGuideline.Sources.Domain.Enums;
using DotnetApiGuideline.Sources.Domain.ValueObjects;

namespace DotnetApiGuideline.Sources.Application.Models;

public record Customer(
    Guid Id,
    string Name,
    Email Email,
    string Phone,
    Address Address,
    CustomerTier Tier
)
{
    public static Customer FromEntity(CustomerEntity entity)
    {
        return new Customer(
            Id: entity.Id,
            Name: entity.Name,
            Email: entity.Email,
            Phone: entity.Phone,
            Address: entity.Address,
            Tier: entity.Tier
        );
    }
}
