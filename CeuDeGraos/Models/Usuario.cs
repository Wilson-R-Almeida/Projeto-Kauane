using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CeuDeGraos.Models;

public class Usuario : IdentityUser
{
    public int UsuarioId { get; set; } // Chave primária personalizada (opcional)
}