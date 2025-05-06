using AutoMapper;
using ErrorOr;
using MediatR;

using Domain.Customers.Interfaces;
using Domain.Customers;
using Application.Common.Mappings;
using Microsoft.Extensions.Logging;

namespace Application.Customers.GetAll;

internal sealed class GetAllCustomerQueryHandler : IRequestHandler<GetAllCustomerQuery, ErrorOr<IReadOnlyList<CustomerDto>>>
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

    public async Task<ErrorOr<IReadOnlyList<CustomerDto>>> Handle(GetAllCustomerQuery query, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetAllCustomerQuery");
        IEnumerable<Customer> customers = await _customerRepository.GetAllAsync();

        var customerDtos = _mapper.Map<List<CustomerDto>>(customers);

        _logger.LogInformation("OK - Successfully fetched all customers");
        return customerDtos.AsReadOnly();
    }
}
