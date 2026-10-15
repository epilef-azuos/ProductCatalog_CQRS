using FluentValidation;

namespace ProductCatalog.Application.Products.Commands.Validations
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("O Nome é obrigatório.")
                .Length(3, 100).WithMessage("O Nome deve ter entre 3 e 100 caracteres.");

            RuleFor(c => c.Price)
                .GreaterThan(0).WithMessage("O Preço deve ser maior que zero.");

            RuleFor(c => c.StockQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("A quantidade em estoque não pode ser negativa.");
        }
    }
}
