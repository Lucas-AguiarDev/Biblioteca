using System.Linq;
using System.Collections.Generic;
using System.Collections;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Models
{
    public class UsuarioService
    {

        public void incluirUsuario(Usuario novoUsuario)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                bc.Usuarios.Add(novoUsuario);
                bc.SaveChanges();
            }
        }

        public void editarUsuario(Usuario editarUsuario)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                Usuario usuario = bc.Usuarios.Find(editarUsuario.Id);
                usuario.Nome = editarUsuario.Nome;
                usuario.Login = editarUsuario.Login;
                usuario.Senha = editarUsuario.Senha;

                bc.SaveChanges();
            }
        }

         public void excluirUsuario(int id)
         {
             using(BibliotecaContext bc = new BibliotecaContext())
             {
                 bc.Usuarios.Remove(bc.Usuarios.Find(id));
                 bc.SaveChanges();
             }
         }

         public List<Usuario> Listar()
         {
             using(BibliotecaContext bc = new BibliotecaContext())
             {
                 return bc.Usuarios.ToList();
             }
         }

         public Usuario Listar (int id)
         {
             using(BibliotecaContext bc = new BibliotecaContext())
             {
                 return bc.Usuarios.Find(id);
             }
         }

    }
}