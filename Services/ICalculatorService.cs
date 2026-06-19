namespace NSubstituteTest.Services;

public interface ICalculatorService
{
    int Add(int a, int b);
    int Subtract(int a, int b);
    int Multiply(int a, int b);
    decimal Divide(int a, int b);
}
