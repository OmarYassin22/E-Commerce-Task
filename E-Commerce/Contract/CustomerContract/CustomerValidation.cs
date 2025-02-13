using E_Commerce.Models;
using FluentValidation;

namespace E_Commerce.Contract
{
    public class CustomerValidation : AbstractValidator<Customer>
    {
        public CustomerValidation()
        {
            RuleFor(c => c.FirstNme).NotEmpty();
            RuleFor(c => c.LastNme).NotEmpty();
            RuleFor(c => c.Address).NotEmpty();
            RuleFor(c => c.Email).EmailAddress();
            RuleFor(c => c.Phone).NotEmpty();
        }

    }
}