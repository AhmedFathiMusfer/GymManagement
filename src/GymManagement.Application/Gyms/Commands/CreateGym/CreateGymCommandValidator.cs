using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace GymManagement.Application.Gyms.Commands.CreateGym
{
    public class CreateGymCommandValidator : AbstractValidator<CreateGymCommand>
    {
        public CreateGymCommandValidator()
        {
            RuleFor(g => g.Name).MinimumLength(3).MaximumLength(30);
        }
    }
}