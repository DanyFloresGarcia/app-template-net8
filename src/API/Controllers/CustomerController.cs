using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Customers.GetAll;

namespace API.Controllers;

[Route("api/v1/customers")]
public class CustomerController : ApiController
{
    private readonly ISender _mediator;

    public CustomerController(ISender mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    /// Gets all customers.
    /// </summary>
    /// <returns>The result of the operation, which is either a list of customers or a list of errors</returns>
    
    [HttpGet]
    public async Task<IActionResult> GetAllCustomer()
    {
        var customersResult = await _mediator.Send(new GetAllCustomerQuery());

        return customersResult.Match(
            customers => Ok(customers),
            errors => Problem(errors)
        );
    }
}

