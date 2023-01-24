using Microsoft.EntityFrameworkCore;
using Taks.Data;
using Taks.Models;
using Taks.Repositorio.Interfaces;

namespace Taks.Repositorio
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly SistemaTarefaDbContext _dbContext;

        public UsuarioRepositorio(SistemaTarefaDbContext sistemaTarefaDbContext)
        {
            _dbContext = sistemaTarefaDbContext;
        }

        public async Task<List<UsuarioModel>> BuscarTodosUsuarios()
        {
            return await _dbContext.Usuarios.ToListAsync();
        }

        public async Task<UsuarioModel> BuscarUsuarioId(int id)
        {
            return await _dbContext.Usuarios.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<UsuarioModel> Adicionar(UsuarioModel usuario)
        {
            await _dbContext.Usuarios.AddAsync(usuario);
            await _dbContext.SaveChangesAsync();

            return  usuario;
        }

        public async Task<bool> Apagar(int id)
        {
            var usuarioPorId = await BuscarUsuarioId(id);

            if (usuarioPorId != null)
            {
                _dbContext.Usuarios.Remove(usuarioPorId);
                await _dbContext.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<UsuarioModel> Atualizar(UsuarioModel usuario, int id)
        {
            var usuarioPorId = await BuscarUsuarioId(id);

            if (usuarioPorId != null)
            {
                usuarioPorId.Nome = usuario.Nome;
                usuario.Email = usuario.Email;

                _dbContext.Usuarios.Update(usuarioPorId);
                await _dbContext.SaveChangesAsync();

                return usuarioPorId;
            }

            throw new Exception($"Usuario {id.ToString()} não foi encontrado na base de dados.");
        }

 
    }
}
