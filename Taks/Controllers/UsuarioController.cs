using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Formatting;
using System.Web.Http;
using Taks.Models;
using Taks.Repositorio.Interfaces;

namespace Taks.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ApiController
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        public UsuarioController(IUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        [Microsoft.AspNetCore.Mvc.HttpGet]
        public async Task<HttpResponseMessage>  BuscarTodosUsuarios()

        {
            var usuarios = await _usuarioRepositorio.BuscarTodosUsuarios();

            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                Code = "OK",
                Data = usuarios
            }, new JsonMediaTypeFormatter());
        }


        [Microsoft.AspNetCore.Mvc.HttpGet]
        public async Task<HttpResponseMessage> BuscarUsuario(int id)

        {
            var usuario = await _usuarioRepositorio.BuscarUsuarioId(id);

            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                Code = "OK",
                Data = usuario
            }, new JsonMediaTypeFormatter());
        }
    }
}
