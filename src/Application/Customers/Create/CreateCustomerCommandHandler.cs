namespace Application.Customers.Create;

using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Domain.Customers.Interfaces;
using Domain.Customers;
using Domain.ValueObjects;
using Domain.Primitives;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using ErrorOr;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, ErrorOr<int>>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateCustomerCommandHandler> _logger;

    public CreateCustomerCommandHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork, ILogger<CreateCustomerCommandHandler> logger)
    {
        _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ErrorOr<int>> Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
    {
        using (LogContext.PushProperty("Identifier", string.Concat("Create Customers: " + string.Concat(command.Name, " ", command.Lastame))))
        {
            _logger.LogInformation("Initial Creating customer");
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                _logger.LogInformation("Validating audit Record");
                if (AuditRecord.Create(command.UserCreated, "localhost", command.ApplicationName) is not AuditRecord auditRecord)
                {
                    _logger.LogError("Audit record is null");
                    await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                    throw new ArgumentNullException(nameof(auditRecord));
                }

                _logger.LogInformation("Creating customer");
                var customer = new Customer(command.Name, command.Lastame, command.Email, command.Phone, auditRecord);

                await _customerRepository.AddAsync(customer);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Creating customer Finish: {@Command}", command);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                return customer.Id;
            }
            catch (Exception ex)
            {
                string errorMessage = $"Error creating customer: {ex.Message}";
                _logger.LogError(errorMessage);
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);

                return Error.Failure(
                    code: "Customer.Create.Failure",
                    description: errorMessage
                );
            }
        }
    }
}
