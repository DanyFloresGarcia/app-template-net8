using ErrorOr;
using MediatR;

using Application.Common.Mappings;

namespace Application.Customers.GetAll;

public record GetAllCustomerQuery() : IRequest<ErrorOr<IReadOnlyList<CustomerDto>>>;