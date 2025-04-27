using System.ComponentModel.DataAnnotations;

namespace User.Api.Common;

[ExcludeFromCodeCoverage]
public sealed class Request
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "Formato de e-mail inválido.")]
    [MaxLength(100, ErrorMessage = "O e-mail deve ter no máximo 100 caracteres.")]
    public string? Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(50, ErrorMessage = "O nome deve ter entre 3 e 50 caracteres.", MinimumLength = 3)]
    public string? Name { get; set; } = string.Empty;
}