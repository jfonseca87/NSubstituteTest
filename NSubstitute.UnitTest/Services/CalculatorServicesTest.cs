using NSubstituteTest.Services;

namespace NSubstitute.UnitTest.Services;

public class CalculatorServicesTest
{
    private readonly ICalculatorService _mockCalculatorService;

    public CalculatorServicesTest()
    {
        _mockCalculatorService = Substitute.For<ICalculatorService>();
    }

    [Fact]
    public void Should_Add_Two_Numbers()
    {
        // Arrange
        _mockCalculatorService.Add(Arg.Any<int>(), Arg.Any<int>()).Returns(5);

        // Act
        var result = _mockCalculatorService.Add(2, 3);

        // Assert
        Assert.Equal(5, result);
    }

    [Fact]
    public void Should_Throw_Exception_On_Divide_By_Zero()
    {
        // Arrange
        _mockCalculatorService
            .When(x => x.Divide(Arg.Any<int>(), Arg.Any<int>()))
            .Throw(x => new DivideByZeroException());

        // Act & Assert
        Assert.Throws<DivideByZeroException>(() => _mockCalculatorService.Divide(10, 0));
    }
}
