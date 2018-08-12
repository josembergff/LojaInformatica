using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LojaInformatica.API.Entidades;
using MongoDB.Driver;

namespace LojaInformatica.API.Dados
{
    public class RepositorioMongodb : IRepositorioCollection
    {
        public RepositorioMongodb(ContextoLojaInformatica context)
        {
            Context = context;
        }

        public ContextoLojaInformatica Context { get; }

        public async Task<string> Acrescentar<TEntidade>(TEntidade entidade) where TEntidade : EntidadeCollection
        {
            entidade.DataCadastro = DateTime.Now;
            await Context.DadosMongo<TEntidade>(entidade).InsertOneAsync(entidade);
            return entidade._id;
        }

        public async Task<bool> Atualizar<TEntidade>(TEntidade entidade) where TEntidade : EntidadeCollection
        {
            entidade.DataEdicao = DateTime.Now;
            await Context.DadosMongo<TEntidade>(entidade).UpdateOneAsync(Builders<TEntidade>.Filter.Eq(s => s._id, entidade._id), Builders<TEntidade>.Update.Set(s => s, entidade));

            return true;
        }

        public async Task<TEntidade> BuscarId<TEntidade>(string id) where TEntidade : EntidadeCollection
        {
            var novo = Activator.CreateInstance<TEntidade>();
            var lista = await Context.DadosMongo<TEntidade>(novo).FindAsync(f => f._id == id);

            return lista.FirstOrDefault();
        }

        public async Task<IEnumerable<TEntidade>> Listar<TEntidade>(Expression<Func<TEntidade, bool>> filtro) where TEntidade : EntidadeCollection
        {
            var novo = Activator.CreateInstance<TEntidade>();

            var lista = await Context.DadosMongo<TEntidade>(novo).FindAsync(filtro);

            return lista.ToList();
        }

        public async Task<bool> Remover<TEntidade>(Expression<Func<TEntidade, bool>> filtro) where TEntidade : EntidadeCollection
        {
            var novo = Activator.CreateInstance<TEntidade>();

            var lista = await Context.DadosMongo<TEntidade>(novo).DeleteOneAsync(filtro);

            return true;
        }

        public async Task<bool> Remover<TEntidade>(string id) where TEntidade : EntidadeCollection, new()
        {
            var novo = Activator.CreateInstance<TEntidade>();

            var lista = await Context.DadosMongo<TEntidade>(novo).DeleteOneAsync(f => f._id == id);

            return true;

        }
    }
}