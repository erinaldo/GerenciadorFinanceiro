using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Gerenciador_Financeiro.Model;
using Gerenciador_Financeiro.Context;
using Microsoft.EntityFrameworkCore;

namespace Gerenciador_Financeiro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceitaController : ControllerBase
    {        
        private readonly GerenciadorFinanceiroContext _context;

        public ReceitaController(GerenciadorFinanceiroContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public IEnumerable<Receita> Todos()
        {
            return _context.Receitas.Include(e => e.Conta);
        }
     
        [HttpGet("{id}")]
        public ActionResult<Receita> Obter(long id)
        {
            var usuarioEncontrado = _context.Receitas.Find(id);

            if (usuarioEncontrado == null)
                return NotFound();
            
            return usuarioEncontrado;
        }

        [HttpPost]
        public void Novo([FromBody] Receita receita)
        {
            var contaEncontrado = _context.Contas.Find(receita.Conta.Id);

            if(contaEncontrado != null)
                receita.Conta = contaEncontrado;

            _context.Receitas.Add(receita);
            _context.SaveChanges();
        }

        [HttpPut]
        public IActionResult Atualizar([FromBody] Receita receita)
        {
            var receitaEncontrada = _context.Receitas.Find(receita.Id);
            if (receitaEncontrada == null)
                return NotFound();

            receitaEncontrada.DataReceita = receita.DataReceita;
            receitaEncontrada.Descricao = receita.Descricao;

            var contaEncontrado = _context.Contas.Find(receita.Conta.Id);

            if(contaEncontrado == null)
                return NotFound();

            receitaEncontrada.Conta = contaEncontrado;

            _context.Receitas.Update(receitaEncontrada);
            _context.SaveChanges();
            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public IActionResult Excluir(int id)
        {
            var receitaEncontrada = _context.Receitas.Find(id);

            if (receitaEncontrada == null)
                return NotFound();

            _context.Receitas.Remove(receitaEncontrada);
            _context.SaveChanges();

            return Ok(receitaEncontrada);
        }
    }
}