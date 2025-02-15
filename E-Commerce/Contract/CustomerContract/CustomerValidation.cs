using E_Commerce.Models;
using FluentValidation;

namespace E_Commerce.Contract
{
    public class CustomerValidation : AbstractValidator<ApplicationUser>
    {
        public CustomerValidation()
        {
            RuleFor(c => c.FirstName).NotEmpty();
            RuleFor(c => c.LastName).NotEmpty();
            RuleFor(c => c.Address).NotEmpty();
            RuleFor(c => c.Email).EmailAddress();
            RuleFor(c => c.PhoneNumber).NotEmpty();
        }

    }
}