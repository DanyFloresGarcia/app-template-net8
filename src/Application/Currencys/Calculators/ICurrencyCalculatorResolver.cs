namespace Application.Currencys.Calculators;

public interface ICurrencyCalculatorResolver
{
    ICurrencyCalculator Resolve(CurrencyCode currencyCode);
}