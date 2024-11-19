using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CeuDeGraos.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Text.RegularExpressions;
using System.Security.Claims;
using CeuDeGraos.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace CeuDeGraos.Controllers;

public class LoginController : Controller
{
    private readonly ILogger<LoginController> _logger;

    private readonly BancoContext _dbContext;


    public LoginController(ILogger<LoginController> logger, BancoContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
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

            var usuario = _dbContext.Usuarios.FirstOrDefault(u => u.CPF_CNPJ == model.CpfCnpj);

            if (usuario != null && BCrypt.Net.BCrypt.Verify(model.Senha, usuario.Senha))
            {
                // Criar claims para o usuário
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuario.Nome),
                    new Claim(ClaimTypes.Email, usuario.Email),
                    new Claim("TipoUsuario", usuario.TipoUsuario),
                    new Claim("NomeCliente", usuario.Nome)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                // Configurar autenticação
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Email ou senha inválidos.");

        }

        return View(model);
    }


    /*
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

    */

    /*
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    */
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }



}
