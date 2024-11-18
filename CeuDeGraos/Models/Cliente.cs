
namespace CeuDeGraos.Models;

public abstract class Cliente : Usuario
{
    public int ClienteId { get; set; }
    public string? CEP { get; set; }
    public string? Telefone { get; set; }
    
    
}