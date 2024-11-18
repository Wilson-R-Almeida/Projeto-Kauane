using CeuDeGraos.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity;

namespace CeuDeGraos.Data;

public class BancoContext : IdentityDbContext<Usuario>
{

    public BancoContext(DbContextOptions<BancoContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Cliente>()
            .HasDiscriminator<string>("ClienteTipo")
            .HasValue<PessoaFisica>("PessoaFisica")
            .HasValue<PessoaJuridica>("PessoaJuridica");
    }

    public DbSet<Cliente> Clientes { get; set; }

    public DbSet<PessoaFisica> PessoasFisicas { get; set; }
    public DbSet<PessoaJuridica> PessoasJuridicas { get; set; }
    public DbSet<Produto> Produtos { get; set; }

    public DbSet<SolicitacaoRecuperacaoSenha> SolicitacaoRecuperacaoSenha { get; set; }

}