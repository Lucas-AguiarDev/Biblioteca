using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Biblioteca.Models;

namespace Biblioteca.Models
{
    public class Autenticacao
    {
        public static void CheckLogin(Controller controller)
        {
            if (string.IsNullOrEmpty(controller.HttpContext.Session.GetString("login")))
            {
                controller.Request.HttpContext.Response.Redirect("/Home/Login");
            }
        }

        public static bool verificaLoginSenha (string login, string senha, Controller controller)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                verificaSeUsuarioAdminExiste(bc);

                senha = Criptografo.TextoCriptografado(senha);

                IQueryable<Usuario> UsuarioEncontrado = bc.Usuarios.Where(u => u.Login == login && u.Senha == senha);
                List<Usuario> ListarUsuarioEncontrado = UsuarioEncontrado.ToList();

                if (ListarUsuarioEncontrado.Count==0)
                {
                    return false;
                }
                else
                {
                    controller.HttpContext.Session.SetString("login", ListarUsuarioEncontrado[0].Login);
                    controller.HttpContext.Session.SetString("nome", ListarUsuarioEncontrado[0].Nome);
                    controller.HttpContext.Session.SetInt32("tipo", ListarUsuarioEncontrado[0].Tipo);
                    return true;
                }
            }
        }

        public static void verificaSeUsuarioAdminExiste (BibliotecaContext bc)
        {
             IQueryable<Usuario> UserEncontrado = bc.Usuarios.Where(u => u.Login == "admin");

             if (UserEncontrado.ToList().Count==0)
             {
                 Usuario admin = new Usuario();
                 admin.Login = "admin";
                 admin.Senha = Criptografo.TextoCriptografado("123");
                 admin.Tipo = Usuario.ADMIN;
                 admin.Nome = "administrador";

                 bc.Usuarios.Add(admin);
                 bc.SaveChanges();
             }
        }

        public static void verificaSeUsuarioEAdmin (Controller controller)
        {
            if (!(controller.HttpContext.Session.GetInt32("tipo") == Usuario.ADMIN))
            {
                controller.Request.HttpContext.Response.Redirect("/Usuarios/NeedAdmin");
            }      
        }
    }
}