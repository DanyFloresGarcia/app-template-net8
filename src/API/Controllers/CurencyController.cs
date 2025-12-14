using Microsoft.AspNetCore.Mvc;
using Application.Currencys.Calculators;

namespace API.Controllers;

[Route("api/v1/currencys")]
public class CurencyController : ApiController
{
    private readonly ICurrencyCalculatorResolver _resolver;

    public CurencyController(ICurrencyCalculatorResolver resolver)
    {
        _resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
    }

    [HttpPost]
    public IActionResult Calculate(decimal amount, CurrencyCode fromCurrency)
    {
        // Pendiente de migrar a la capa de Application
        var calculator = _resolver.Resolve(fromCurrency);
        var result = calculator.Convert(amount);

        return Ok(new { Amount = amount, FromCurrency = fromCurrency, ToCurrency = calculator.CurrencyCode, Result = result });
    }
}

