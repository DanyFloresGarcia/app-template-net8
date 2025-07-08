using AutoMapper;
using ErrorOr;
using MediatR;

using Domain.Customers.Interfaces;
using Domain.Customers;
using Application.Common.Mappings;
using Application.Common;
using Microsoft.Extensions.Logging;

namespace Application.Customers.GetAll;

internal sealed class GetAllCustomerQueryHandler : IRequestHandler<GetAllCustomerQuery, ErrorOr<PaginatedResponse<CustomerDto>>>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ILogger<GetAllCustomerQueryHandler> _logger;
    private readonly IMapper _mapper;
    public GetAllCustomerQueryHandler(ICustomerRepository customerRepository, ILogger<GetAllCustomerQueryHandler> logger, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<ErrorOr<PaginatedResponse<CustomerDto>>> Handle(GetAllCustomerQuery query, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetAllCustomerQuery");

        // Llama al nuevo método con paginación
        var (customers, totalCount) = await _customerRepository
            .GetPagedAsync(query.Page, query.Size, cancellationToken);

        var customerDtos = _mapper.Map<List<CustomerDto>>(customers);

        _logger.LogInformation("OK - Successfully fetched all customers");

        return new PaginatedResponse<CustomerDto>(
            customerDtos,
            query.Page,
            query.Size,
            totalCount
        );
    }
}
