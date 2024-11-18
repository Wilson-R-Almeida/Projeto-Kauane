using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CeuDeGraos.Models;
using CeuDeGraos.Data;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

namespace CeuDeGraos.Controllers;

public class CadastroController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly UserManager<Usuario> _userManager;
    private readonly SignInManager<Usuario> _signInManager;
    public CadastroController(ILogger<HomeController> logger, UserManager<Usuario> userManager, SignInManager<Usuario> signInManager)
    {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public IActionResult Formulario()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SalvarClienteFisico(ClienteViewModel formulario)

    {
        if (ModelState.IsValid)
        {
            string sanitizedCpf = Regex.Replace(formulario.Cpf.Trim(), @"[^\d]", "");
            PessoaFisica cliente = new()
            {
                Cpf = sanitizedCpf,
                CEP = formulario.CEP,
                UserName = sanitizedCpf,
                Email = formulario.Email,
                DtNasc = formulario.DtNasc,
                NmCliente = formulario.NmCliente,
                Telefone = formulario.Telefone,

            };
            var result = await _userManager.CreateAsync(cliente, formulario.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ConfirmacaoCadastro", "Cadastro");
            }


            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

        }

        // Retorna a view com os dados em caso de erro de validação
        return View("Formulario", formulario);
    }

    [HttpPost]
    public async Task<IActionResult> SalvarClienteJuridico(ClienteViewModel formulario)

    {
        if (ModelState.IsValid)
        {

            string sanitizedCnpj = Regex.Replace(formulario.Cnpj.Trim(), @"[^\d]", "");
            PessoaJuridica cliente = new()
            {
                Cnpj = sanitizedCnpj,
                CEP = formulario.CEP,
                UserName = sanitizedCnpj,
                Email = formulario.Email,
                RazaoSocial = formulario.RazaoSocial,
                Telefone = formulario.Telefone,

            };
            var result = await _userManager.CreateAsync(cliente, formulario.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ConfirmacaoCadastro", "Cadastro");
            }

        }
        return View("Formulario", formulario);
    }

    public IActionResult Sucesso()
    {
        return View();
    }

    public IActionResult ConfirmacaoCadastro()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
