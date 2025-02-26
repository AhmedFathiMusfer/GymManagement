

using ErrorOr;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using GymManagement.Application.Common.Behaviors;
using GymManagement.Application.Gyms.Commands.CreateGym;
using GymManagement.Domain.Gyms;
using MediatR;
using NSubstitute;
using TestCommon.Gyms;

namespace GymManagement.Application.UnitTests.Common.Behavior
{

    public class ValidationBehaviorTests
    {
        private readonly IValidator<CreateGymCommand> _mockValidator;
        private readonly RequestHandlerDelegate<ErrorOr<Gym>> _mockNextBeahvior;
        private readonly ValidationBehavior<CreateGymCommand, ErrorOr<Gym>> _mockValidationBehavior;
        public ValidationBehaviorTests()
        {
            //create mock validator
            _mockValidator = Substitute.For<IValidator<CreateGymCommand>>();
            //create mock next behavior
            _mockNextBeahvior = Substitute.For<RequestHandlerDelegate<ErrorOr<Gym>>>();
            //create validation behavior
            _mockValidationBehavior = new ValidationBehavior<CreateGymCommand, ErrorOr<Gym>>(_mockValidator);


        }
        [Fact]
        public async Task InvokeBehavior_WhenValidatorResultIsVaild_ShouldInvokeNextBehavior()
        {
            //Arrange
            var mockRequest = GymCommandFactory.CreateCreateGymCommand();
            _mockValidator.ValidateAsync(mockRequest, default).Returns(new FluentValidation.Results.ValidationResult());
            var gym = GymFactory.CreateGym();
            _mockNextBeahvior.Invoke().Returns(gym);
            //Act
            var result = await _mockValidationBehavior.Handle(mockRequest, _mockNextBeahvior, default);
            //Assert
            result.IsError.Should().BeFalse();
            result.Value.Should().Be(gym);

        }
        [Fact]
        public async Task InvokeBehavior_WhenValidatorResultIsInVaild_ShouldReturnListOfError()
        {
            //Arrange
            var mockRequest = GymCommandFactory.CreateCreateGymCommand();
            List<ValidationFailure> validationFailures = [new(propertyName: "foo", errorMessage: "bad foo")];
            _mockValidator.ValidateAsync(mockRequest, default).Returns(new ValidationResult(validationFailures));


            //Act
            var result = await _mockValidationBehavior.Handle(mockRequest, _mockNextBeahvior, default);
            //Assert
            result.IsError.Should().BeTrue();
            result.FirstError.Code.Should().Be("foo");
            result.FirstError.Description.Should().Be("bad foo");

        }
    }
}