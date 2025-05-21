using ErrorOr;
using MediatR;

namespace Application.Customers.Create;

public record CreateCustomerCommand(
    string Name,
    string Lastame,
    string Email,
    string Phone,
    string UserCreated,
    string ApplicationName
) : IRequest<ErrorOr<int>>;