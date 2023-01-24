using Microsoft.EntityFrameworkCore;
using Taks.Data.Map;
using Taks.Models;

namespace Taks.Data
{
    public class SistemaTarefaDbContext : DbContext
    {
        public SistemaTarefaDbContext(DbContextOptions<SistemaTarefaDbContext>options) : base(options)
        {
            
        }

        public DbSet<UsuarioModel> Usuarios { get; set; }
        public DbSet<TarefaModel> Tarefas { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsuarioMap());
            modelBuilder.ApplyConfiguration(new TarefaMap());
            base.OnModelCreating(modelBuilder);
        }
    }
}
