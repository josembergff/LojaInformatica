using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LojaInformatica.API.Entidades;

namespace LojaInformatica.API.Dados {
    public interface IRepositorioCollection {
        Task<string> Acrescentar<TEntidade> (TEntidade entidade) where TEntidade : EntidadeCollection;

        Task<TEntidade> BuscarId<TEntidade> (string id) where TEntidade : EntidadeCollection;

        Task<IEnumerable<TEntidade>> Listar<TEntidade> (Expression<Func<TEntidade, bool>> filtro) where TEntidade : EntidadeCollection;

        Task<bool> Atualizar<TEntidade> (TEntidade entidade) where TEntidade : EntidadeCollection;

        Task<bool> Remover<TEntidade> (Expression<Func<TEntidade, bool>> filtro) where TEntidade : EntidadeCollection;

        Task<bool> Remover<TEntidade> (string id) where TEntidade : EntidadeCollection, new ();
    }
}