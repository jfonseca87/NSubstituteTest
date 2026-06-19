namespace NSubstituteTest.Services;

public class CalculatorService : ICalculatorService
{
    public int Add(int a, int b)
    {
        return a + b;
    }

    public decimal Divide(int a, int b)
    {
        return (decimal)a / b;
    }

    public int Multiply(int a, int b)
    {
        return a * b;
    }

    public int Subtract(int a, int b)
    {
        return a - b;
    }
}
