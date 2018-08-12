using LojaInformatica.API.Dados.Configuracao;
using LojaInformatica.API.Entidades;
using LojaInformatica.API.ObjetosDeValor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace LojaInformatica.API.Dados {
    public class ContextoLojaInformatica : DbContext {
        public readonly IMongoDatabase database;
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Compra> Compras { get; set; }
        public DbSet<Produto> Produtos { get; set; }

        public ContextoLojaInformatica (DbContextOptions<ContextoLojaInformatica> options, IOptions<Settings> settings) : base (options) {
            database = new MongoClient (settings.Value.ConnectionString).GetDatabase (settings.Value.Database);
        }

        protected override void OnModelCreating (ModelBuilder modelBuilder) {
            modelBuilder.ApplyConfiguration (new ClienteConfiguracao ());
            modelBuilder.ApplyConfiguration (new ItemDaCompraConfiguracao ());
        }

        public IMongoCollection<TEntidade> DadosMongo<TEntidade> (TEntidade entidade) where TEntidade : EntidadeCollection {
            return database.GetCollection<TEntidade> (entidade.NomePlural ());
        }
    }
}