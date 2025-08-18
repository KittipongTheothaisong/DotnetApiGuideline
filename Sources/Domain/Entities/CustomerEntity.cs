using DotnetApiGuideline.Sources.Domain.Enums;
using DotnetApiGuideline.Sources.Domain.ValueObjects;
using OrderManagement.Domain.ValueObjects;

namespace DotnetApiGuideline.Sources.Domain.Entities;

public class CustomerEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public Email Email { get; set; } = Email.Empty();
    public string Phone { get; set; } = string.Empty;
    public Address Address { get; set; } = Address.Empty();
    public CustomerTier Tier { get; set; }
}
