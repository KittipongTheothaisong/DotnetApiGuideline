using DotnetApiGuideline.Sources.Application.Models;

namespace DotnetApiGuideline.Sources.Presentation.Responses;

public record CustomerResponse(Guid Id, string Name, string Email, string PhoneNumber)
{
    public static CustomerResponse FromCustomer(Customer customer)
    {
        return new CustomerResponse(
            Id: customer.Id,
            Name: customer.Name,
            Email: customer.Email.Value,
            PhoneNumber: customer.Phone
        );
    }
}
