using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chapter.WebApi.Models;
using Chapter.WebApi.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chapter.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LivrosController : ControllerBase
    {
        private readonly LivroRepository _livroRepository;
        public LivrosController(LivroRepository livroRepository)
        {
            _livroRepository = livroRepository;
        }
        [HttpGet]
        public IActionResult Listar()
        {
            return Ok(_livroRepository.Listar());
        }

        // get/api/livros/{id} 
        [HttpGet("{id}")] // busca pelo id passado 
        public IActionResult BuscarPorId(int id)
        {
            Livro livro = _livroRepository.BuscarPorId(id);
            if (livro == null)
            {
                return NotFound();
            }
            return Ok(livro);
        }

        // put/api/livros/{id} 
        // recebe a informação do livro 
        // atualiza o corpo da requisição 
        [HttpPut("{id}")] // o id passado no put /api/livros/1 
        public IActionResult Atualizar(int id, Livro livro)
        {
            _livroRepository.Atualizar(id, livro);
            return StatusCode(204);
        }
        // post /api/livros/ 
        // recebe a info do livro que deseja salvar do corpo da requisição 
        [HttpPost]
        public IActionResult Cadastrar(Livro livro)
        {
            _livroRepository.Cadastrar(livro);
            return StatusCode(201);
        }


        // Delete /api/livros/{id} 
        [HttpDelete("{id}")] // o id passado no delete /api/livros/1 
        public IActionResult Deletar(int id)
        {
            try
            {
                _livroRepository.Deletar(id);
                return StatusCode(204);
            }
            catch (System.Exception)
            {

                return BadRequest();
            }

        }

    }
}