namespace CeuDeGraos.Models;

public class PessoaFisica : Cliente
{
    public string? Cpf { get; set; }
    public string? NmCliente { get; set; }
    public DateOnly? DtNasc { get; set; }
}