namespace Application.Currencys.Calculators;

public class UsdCurrencyCalculator : ICurrencyCalculator
{
    public CurrencyCode CurrencyCode => CurrencyCode.USD;
    public decimal Convert(decimal amount) => amount * 3.75m;
}