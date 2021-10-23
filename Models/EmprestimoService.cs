using System.Linq;
using System.Collections.Generic;
using System.Collections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Biblioteca.Models
{
    public class EmprestimoService 
    {
        public void Inserir(Emprestimo e)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                bc.Emprestimos.Add(e);
                bc.SaveChanges();
            }
        }

        public void Atualizar(Emprestimo e)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                Emprestimo emprestimo = bc.Emprestimos.Find(e.Id);
                emprestimo.NomeUsuario = e.NomeUsuario;
                emprestimo.Telefone = e.Telefone;
                emprestimo.LivroId = e.LivroId;
                emprestimo.DataEmprestimo = e.DataEmprestimo;
                emprestimo.DataDevolucao = e.DataDevolucao;

                bc.SaveChanges();
            }
        }

        /* public ICollection<Emprestimo> ListarTodos(FiltrosEmprestimos filtro)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                return bc.Emprestimos.Include(e => e.Livro).ToList();
            }
        } */

        public ICollection<Emprestimo> ListarTodos(FiltrosEmprestimos filtro = null)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                IQueryable<Emprestimo> query;
                
                if(filtro != null)
                {
                    //definindo dinamicamente a filtragem
                    switch(filtro.TipoFiltro)
                    {
                        case "Usuario":
                            query = bc.Emprestimos.Where(e => e.NomeUsuario.Contains(filtro.Filtro));
                        break;

                        case "Livro":
                            query = bc.Emprestimos.Where(e => e.Livro.Titulo.Contains(filtro.Filtro));
                        break;

                        default:
                            query = bc.Emprestimos;
                        break;
                    }
                }
                else
                {
                    // caso filtro não tenha sido informado
                    query = bc.Emprestimos;
                }
                
                //ordenação padrão
                return query.Include(e => e.Livro).OrderByDescending (e=> e.DataDevolucao).ToList();
            }
        }

        public ICollection<Livro> ListarDisponiveis()
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                //busca os livros onde o id não está entre os ids de livro em empréstimo
                // utiliza uma subconsulta
                return
                    bc.Livros
                    .Where(l =>  !(bc.Emprestimos.Where(e => e.Devolvido == false).Select(e => e.LivroId).Contains(l.Id)) )
                    .ToList();
            }
        }
                

        public Emprestimo ObterPorId(int id)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                return bc.Emprestimos.Find(id);
            }
        }
    }
}