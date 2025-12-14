namespace Application.Currencys.Calculators;

public class EurCurrencyCalculator : ICurrencyCalculator
{
    public CurrencyCode CurrencyCode => CurrencyCode.EUR;
    public decimal Convert(decimal amount) => amount * 4.15m;
}