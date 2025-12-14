namespace Application.Currencys.Calculators;

public interface ICurrencyCalculator
{
    CurrencyCode CurrencyCode { get; }
    decimal Convert(decimal amount);
}