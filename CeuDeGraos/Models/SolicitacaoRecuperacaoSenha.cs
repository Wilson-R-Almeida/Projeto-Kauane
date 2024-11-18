namespace CeuDeGraos.Models;

public class SolicitacaoRecuperacaoSenha 
{
    public int Id { get; set; }
    public string? CodigoRecuperacao { get; set; }
    public Cliente? Clnt { get; set; }
    public DateTime? DataCriacao { get; set; }
    public DateTime? DataExpiracao { get; set; }
    public bool? FoiUtilizado { get; set; }
}