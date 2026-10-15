using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ProductCatalog.API.Filters
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            // 1. Interceptar erros de validação do FluentValidation
            if (context.Exception is ValidationException validationException)
            {
                var errors = validationException.Errors
                    .Select(error => $"{error.PropertyName}: {error.ErrorMessage}")
                    .ToList();

                var result = new ObjectResult(new { Errors = errors })
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };

                context.Result = result;
                context.ExceptionHandled = true; // Impede que o erro continue a subir
            }
            // 2. Interceptar erros de regras de negócio ou de "Não Encontrado"
            else if (context.Exception is ArgumentNullException || context.Exception is KeyNotFoundException)
            {
                context.Result = new NotFoundObjectResult(new { Error = "Recurso não encontrado." });
                context.ExceptionHandled = true;
            }
            else if (context.Exception is InvalidOperationException ex)
            {
                // Ex: O erro que lançámos no Handler quando não encontra o produto para apagar
                context.Result = new BadRequestObjectResult(new { Error = ex.Message });
                context.ExceptionHandled = true;
            }
            // 3. Fallback genérico (Erro 500) para outros tipos de exceções não previstas
            else
            {
                // Devolvemos a mensagem real da exceção para sabermos exatamente o que falhou
                context.Result = new ObjectResult(new
                {
                    Erro = "Erro Interno no Servidor",
                    Detalhes = context.Exception.Message
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
                context.ExceptionHandled = true;
            }
        }
    }
}
