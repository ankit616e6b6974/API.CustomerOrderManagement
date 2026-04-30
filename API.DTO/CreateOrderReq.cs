using FluentValidation;
using FluentValidation.Results;

namespace API.DTO
{
    public class CreateOrderReq
    {
        public int? CustomerId { get; set; }
        public List<OrderItemReq> Items { get; set; }
    }

    public class CreateOrderValidator : AbstractValidator<CreateOrderReq>
    {
        public CreateOrderValidator()
        {
            RuleSet("ItemsNullOrEmpty", () =>
            {
                RuleFor(x => x.Items).NotEmpty().WithMessage("Order must contain at least one item.");

                RuleForEach(x => x.Items).ChildRules(item =>
                {
                    item.RuleFor(i => i.ProductId).GreaterThan(0).WithMessage("Invalid product.");
                    item.RuleFor(i => i.Quantity).GreaterThan(0).WithMessage("Quantity must be at least 1.");
                });
            });

            RuleSet("CustomerIdNullOrEmpty", () =>
            {
                RuleFor(x => x.CustomerId).NotEmpty().WithMessage("Customer id must be provided.");
            });            
        }

        protected override bool PreValidate(ValidationContext<CreateOrderReq> context, ValidationResult result)
        {
            if (context.InstanceToValidate != null)
            {
                return true;
            }
            result.Errors.Add(new ValidationFailure("Input", "Input can not be null"));
            return false;
        }
    }
}
