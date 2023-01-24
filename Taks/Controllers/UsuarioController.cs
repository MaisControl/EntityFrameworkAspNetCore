using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Taks.Models;
using Taks.Repositorio.Interfaces;

namespace Taks.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        public UsuarioController(IUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        [Microsoft.AspNetCore.Mvc.HttpGet]
        public async Task<ActionResult>  BuscarTodosUsuarios()

        {
            var usuarios = await _usuarioRepositorio.BuscarTodosUsuarios();

            return Ok(usuarios);
        }
    }
}
