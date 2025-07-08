using ErrorOr;
using MediatR;
using Application.Common;
using Application.Common.Mappings;

namespace Application.Customers.GetAll;

public record GetAllCustomerQuery(int Page, int Size) : IRequest<ErrorOr<PaginatedResponse<CustomerDto>>>;