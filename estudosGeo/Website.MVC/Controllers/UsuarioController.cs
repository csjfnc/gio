using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Website.MVC.Models;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using Website.MVC.Helpers.CustomController;
using Website.Identity.Configuration;
using Website.Identity.Model;
using Website.MVC.Helpers.CustomAttribute;
using Website.Identity.CustomAutorizes;
using System;

namespace Website.MVC.Controllers
{
    public class UsuarioController : BaseController
    {
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [AppAuthorize(Modulos = new Modules[] { Modules.USUARIOS }, Permissoes = new Permissions[] { Permissions.CONSULTAR })]
        public ActionResult Index()
        {
            ViewBag.Msg = TempData["MSG"];
            return View(UserManager.Users.ToList());
        }

        [Authorize]
        public ActionResult VerDadosUsuario()
        {
            return View();
        }

        [AppAuthorize(Modulos = new Modules[] { Modules.USUARIOS }, Permissoes = new Permissions[] { Permissions.ADICIONAR })]
        public ActionResult AddUsuario()
        {
            UsuarioViewModel Model = new UsuarioViewModel()
            {
                CheckBoxModulos = ((ICollection<Modules>)Enum.GetValues(typeof(Modules))).Where(m => m != Modules.MODULO).Select(m => new CheckViewModel() { Name = m.GetString(), Id = (int)m, Checked = false }).ToList(),
                CheckBoxPermissoes = ((ICollection<Permissions>)Enum.GetValues(typeof(Permissions))).Where(p => p != Permissions.PERMISSAO).Select(m => new CheckViewModel() { Name = m.GetString(), Id = (int)m, Checked = false }).ToList()
            };

            return View(Model);
        }

        [HttpPost]
        [AppAuthorize(Modulos = new Modules[] { Modules.USUARIOS }, Permissoes = new Permissions[] { Permissions.ADICIONAR })]
        public ActionResult AddUsuario(UsuarioViewModel Usuario)
        {
            if (ModelState.IsValid)
            {
                if (Usuario.Senha != null && (Usuario.Senha.Length >= 6 && Usuario.Senha.Length <= 20))
                {
                    ApplicationUser NewUsuario = new ApplicationUser() { UserName = Usuario.NomeUser, UserLogin = Usuario.LoginUser };
                    IdentityResult ResultInsertDB = UserManager.Create(NewUsuario, Usuario.Senha);
                    if (ResultInsertDB.Succeeded)
                    {
                        foreach (CheckViewModel m in Usuario.CheckBoxModulos)
                        {
                            if (m.Checked)
                            {
                                UserManager.AddClaim(NewUsuario.Id, new Claim(Modules.MODULO.GetString(), ((Modules)m.Id).GetString()));
                            }                            
                        }

                        foreach (CheckViewModel p in Usuario.CheckBoxPermissoes)
                        {
                            if (p.Checked)
                            {
                                UserManager.AddClaim(NewUsuario.Id, new Claim(Permissions.PERMISSAO.GetString(), ((Permissions)p.Id).GetString()));
                            }
                        }

                        TempData["MSG"] = "O Usuário " + NewUsuario.UserName + "foi cadastrado com Sucesso.";
                        return RedirectToAction("Index", "Usuario");
                    }
                    else if (ResultInsertDB.Errors != null)
                    {
                        foreach (string erro in ResultInsertDB.Errors)
                            ModelState.AddModelError("Erro : ", erro);
                    }
                }
            }

            return View(Usuario);
        }

        [AppAuthorize(Modulos = new Modules[] { Modules.USUARIOS }, Permissoes = new Permissions[] { Permissions.ATUALIZAR })]
        public ActionResult EditUsuario(string id_user)
        {
            ApplicationUser Usuario = UserManager.FindById(id_user);
            UsuarioViewModel UsuarioModel = new UsuarioViewModel() { CheckBoxModulos = new List<CheckViewModel>(), CheckBoxPermissoes = new List<CheckViewModel>() };

            if (Usuario != null)
            {
                UsuarioModel = new UsuarioViewModel()
                {
                    LoginUser = Usuario.UserLogin,
                    NomeUser = Usuario.UserName,
                    IdUser = Usuario.Id,
                    CheckBoxModulos = ((ICollection<Modules>)Enum.GetValues(typeof(Modules))).Where(m => m != Modules.MODULO).Select(m => new CheckViewModel() { Name = m.GetString(), Id = (int)m }).ToList(),
                    CheckBoxPermissoes = ((ICollection<Permissions>)Enum.GetValues(typeof(Permissions))).Where(p => p != Permissions.PERMISSAO).Select(m => new CheckViewModel() { Name = m.GetString(), Id = (int)m }).ToList()
                };

                foreach (CheckViewModel mod in UsuarioModel.CheckBoxModulos)
                {
                    if (Usuario.Claims.Where(c => c.ClaimType == Modules.MODULO.GetString() && (((int)(Modules)Enum.Parse(typeof(Modules), c.ClaimValue)) == mod.Id)).Any())
                        mod.Checked = true;
                }

                foreach (CheckViewModel per in UsuarioModel.CheckBoxPermissoes)
                {
                    if (Usuario.Claims.Where(c => c.ClaimType == Permissions.PERMISSAO.GetString() && (((int)(Permissions)Enum.Parse(typeof(Permissions), c.ClaimValue)) == per.Id)).Any())
                        per.Checked = true;
                }
            }
            else
            {
                TempData["MSG"] = Resources.Messages.Not_Register_Data_Base;
                return RedirectToAction("Index", "Usuario");
            }

            return View(UsuarioModel);
        }

        [HttpPost]
        [AppAuthorize(Modulos = new Modules[] { Modules.USUARIOS }, Permissoes = new Permissions[] { Permissions.ATUALIZAR })]
        public ActionResult EditUsuario(UsuarioViewModel UserAndClaims)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser Usuario = UserManager.FindById(UserAndClaims.IdUser);
                if (Usuario != null)
                {
                    Usuario.UserName = UserAndClaims.NomeUser;
                    Usuario.UserLogin = UserAndClaims.LoginUser;
                    UserManager.Update(Usuario);

                    // Removendo todas as Claims do Usuário
                    List<Claim> ClaimsUser = UserManager.GetClaims(Usuario.Id).Where(c => c.Type == Modules.MODULO.GetString() || c.Type == Permissions.PERMISSAO.GetString()).ToList();
                    foreach (Claim claim in ClaimsUser) UserManager.RemoveClaim(Usuario.Id, claim);

                    foreach (CheckViewModel m in UserAndClaims.CheckBoxModulos)
                    {
                        if (m.Checked)
                            UserManager.AddClaim(Usuario.Id, new Claim(Modules.MODULO.GetString(), ((Modules)m.Id).GetString()));
                    }

                    foreach (CheckViewModel p in UserAndClaims.CheckBoxPermissoes)
                    {
                        if (p.Checked)
                            UserManager.AddClaim(Usuario.Id, new Claim(Permissions.PERMISSAO.GetString(), ((Permissions)p.Id).GetString()));
                    }

                    TempData["MSG"] = Resources.Messages.Save_OK;
                    return RedirectToAction("Index", "Usuario");
                }
                else
                {
                    TempData["MSG"] = Resources.Messages.Not_Register_Data_Base;
                    return RedirectToAction("Index", "Usuario");
                }
            }

            return View(UserAndClaims);
        }

        [AppAuthorize(Modulos = new Modules[] { Modules.USUARIOS }, Permissoes = new Permissions[] { Permissions.REMOVER })]
        public ActionResult ExcluirDefinitivo(string id_user)
        {
            ApplicationUser Usuario = UserManager.FindById(id_user);
            IdentityResult result = UserManager.Delete(Usuario);

            if (result.Succeeded)
                TempData["MSG"] = Resources.Messages.Save_OK;
            else
                TempData["MSG"] = Resources.Messages.Error_Save_Changes;

            return RedirectToAction("Index", "Usuario");
        }

        [HttpGet]
        [AppAuthorize(Modulos = new Modules[] { Modules.ADMINISTRACAO })]
        public ActionResult EditPasswordUsuario(string idUser)
        {
            ApplicationUser Usuario = UserManager.FindById(idUser);
            if (Usuario != null)
            {
                return View(new UsuarioViewModel() { IdUser = Usuario.Id, LoginUser = Usuario.UserLogin, NomeUser = Usuario.UserName });
            }
            else
            {
                TempData["MSG"] = Resources.Messages.Not_Register_Data_Base;
                return RedirectToAction("Index", "Usuario");
            }
        }

        [AppAuthorize(Modulos = new Modules[] { Modules.ADMINISTRACAO })]
        public ActionResult EditPasswordUsuario(UsuarioViewModel User)
        {
            /// Removendo a senha Atual do usuário
            IdentityResult Result = UserManager.RemovePassword(User.IdUser);
            if (!Result.Succeeded)
            {
                TempData["MSG"] = Resources.Messages.Error_Save_Changes;
                return RedirectToAction("Index", "Usuario");
            }
                        
            /// Adicionando uma nova senha para o usuário
            Result = UserManager.AddPassword(User.IdUser, User.NovaSenha);
            if (Result.Succeeded)
                TempData["MSG"] = Resources.Messages.Save_OK;
            else
                TempData["MSG"] = Resources.Messages.Error_Save_Changes;

            return RedirectToAction("Index", "Usuario");
        }

        [HttpGet]
        [AppAuthorize]
        public ActionResult ResetPassword()
        {
            ApplicationUser Usuario = UserManager.FindById(User.Identity.GetUserId());
            return View(new UsuarioViewModel() { IdUser = Usuario.Id, LoginUser = Usuario.UserLogin, NomeUser = Usuario.UserName });
        }

        [AppAuthorize]
        public ActionResult ResetPassword(UsuarioViewModel User)
        {
            if (ModelState.IsValid)
            {
                IdentityResult Result = UserManager.ChangePassword(User.IdUser, User.Senha, User.NovaSenha);
                if (!Result.Succeeded)
                {
                    ModelState.AddModelError("Atenção", Resources.Messages.User_Or_Passord_Incorrect);
                    ApplicationUser Usuario = UserManager.FindById(User.IdUser);
                    return View(new UsuarioViewModel() { IdUser = Usuario.Id, LoginUser = Usuario.UserLogin, NomeUser = Usuario.UserName });
                }
            }

            return RedirectToAction("Index", "Home");
        }
    }
}