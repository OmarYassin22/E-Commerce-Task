using E_Commerce.Models;
using FluentValidation;

namespace E_Commerce.Contract
{
    public class OrderValidation : AbstractValidator<Order>
    {
        public OrderValidation()
        {
            RuleFor(o=>o.Name).NotEmpty();
            RuleFor(o=>o.Price).NotEmpty();
            RuleFor(o=>o.Quantity).NotEmpty();

        }

    }
}