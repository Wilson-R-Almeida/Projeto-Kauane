using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CeuDeGraos.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Text.RegularExpressions;
using System.Security.Claims;

namespace CeuDeGraos.Controllers;

public class LoginController : Controller
{
    private readonly ILogger<LoginController> _logger;

    private readonly SignInManager<Usuario> _signInManager;

    private readonly UserManager<Usuario> _userManager;


    public LoginController(ILogger<LoginController> logger, SignInManager<Usuario> signInManager, UserManager<Usuario> userManager)
    {
        _logger = logger;
        _signInManager = signInManager;
        _userManager = userManager;
    }
    public IActionResult Formulario()
    {
        return View();
    }

    [HttpPost]
public async Task<IActionResult> Login(LoginViewModel model)
{
    if (ModelState.IsValid)
    {
        var cpfCnpj = Regex.Replace(model.CpfCnpj, @"\D", "");
        var result = await _signInManager.PasswordSignInAsync(cpfCnpj, model.Password, false, lockoutOnFailure: false);
        
        if (result.Succeeded)
        {
            var user = await _userManager.FindByNameAsync(cpfCnpj);

            if (user != null)
            {
                string nomeCliente = null;

                if (user is PessoaFisica pessoaFisica)
                {
                    nomeCliente = pessoaFisica.NmCliente;
                }
                else if (user is PessoaJuridica pessoaJuridica)
                {
                    nomeCliente = pessoaJuridica.RazaoSocial;
                }

                var claims = new List<Claim>
                {
                    new Claim("NomeCliente", nomeCliente ?? "Usuário")
                };
                await _userManager.AddClaimsAsync(user, claims);
                await _signInManager.SignInAsync(user, isPersistent: false);
            }

            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError(string.Empty, "Login inválido");
    }

    return View("Formulario", model);
}


    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }


}
