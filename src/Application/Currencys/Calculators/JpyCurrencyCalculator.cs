namespace Application.Currencys.Calculators;

public class JpyCurrencyCalculator : ICurrencyCalculator
{
    public CurrencyCode CurrencyCode => CurrencyCode.JPY;
    public decimal Convert(decimal amount) => amount * 6.00m;
}