namespace CeuDeGraos.Models;

public class Produto
{
    public int ProdutoId { get; set; }
    public string? ImagemUrl { get; set; }
    public string? NmProduto { get; set; }
    public double? Unidade { get; set; }
    public string? TipoPacote { get; set; }
    public int? Qtde { get; set; }
    public double? Preco { get; set; }
}