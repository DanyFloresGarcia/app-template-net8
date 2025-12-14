namespace Application.Currencys.Calculators;

public class CurrencyCalculatorResolver: ICurrencyCalculatorResolver
{
    private readonly IEnumerable<ICurrencyCalculator> _calculators;
    public CurrencyCalculatorResolver(IEnumerable<ICurrencyCalculator> calculators)
    {
        _calculators = calculators;
    }
    public ICurrencyCalculator Resolve(CurrencyCode currencyCode)
    {
        return _calculators.First(c => c.CurrencyCode == currencyCode);
    }
}