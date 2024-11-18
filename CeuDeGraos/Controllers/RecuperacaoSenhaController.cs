using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CeuDeGraos.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace CeuDeGraos.Controllers;

public class RecuperacaoSenhaController : Controller
{
    private readonly ILogger<RecuperacaoSenhaController> _logger;

    private readonly SignInManager<Usuario> _signInManager;

    private readonly UserManager<Usuario> _userManager;

    private readonly IEmailSender _emailSender;

    public RecuperacaoSenhaController(ILogger<RecuperacaoSenhaController> logger, SignInManager<Usuario> signInManager, UserManager<Usuario> userManager, IEmailSender emailSender)
    {
        _logger = logger;
        _signInManager = signInManager;
        _userManager = userManager;
        _emailSender = emailSender;
    }
    public IActionResult FormularioEmail()
    {
        return View();
    }

    public IActionResult ConfirmacaoEmailEnviado()
    {
        return View();
    }

    public IActionResult ConfirmacaoSenhaAlterada()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> EnvioFormularioEmail(ForgotPasswordViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            
            if (user == null){
                return View("FormularioEmail");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = Url.Action("FormularioNovaSenha", "RecuperacaoSenha", new { token, email = model.Email }, Request.Scheme);

            await _emailSender.SendEmailAsync(model.Email, "Redefinição de Senha", $"Clique no link para redefinir sua senha: {resetLink}");
            _logger.LogInformation(model.Email);
            return View("ConfirmacaoEmailEnviado", new ForgotPasswordViewModel { Email = model.Email });
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult FormularioNovaSenha(string token, string email)
    {
        if (token == null || email == null)
        {
            return RedirectToAction("Index", "Home");
        }
        return View(new ResetPasswordViewModel { Token = token, Email = email });
    }

    [HttpPost]
    public async Task<IActionResult> EnvioFormularioNovaSenha(ResetPasswordViewModel model)
    {
        _logger.LogInformation(model.Email);
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            
            if (user == null) return RedirectToAction("ResetPasswordConfirmation");

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ConfirmacaoSenhaAlterada");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
        return RedirectToAction("Formulario", "Login");
    }


}
