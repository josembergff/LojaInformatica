using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;
using prmToolkit.NotificationPattern;

namespace LojaInformatica.API.Entidades
{
    public abstract class Entidade : Notifiable
    {
        public int Id { get; set; }

        internal virtual bool EstaValidoParaInsercao()
        {
            return Id == 0;
        }
        internal virtual bool EstaValidoParaAtualizacao()
        {
            return Id > 0;
        }
    }

    public abstract class EntidadeCollection : Entidade
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string _id { get; set; }

        [BsonDateTimeOptions]
        public DateTime DataCadastro { get; set; }

        [BsonDateTimeOptions]
        public DateTime DataEdicao { get; set; }
        public abstract string NomePlural();
    }

    public abstract class Entidade<TEntidade> : Entidade where TEntidade : Entidade
    {
        public virtual bool EquivaleA(TEntidade outraEntidade)
        {
            return Id == outraEntidade.Id;
        }
    }

    public static class EntidadeExtensions
    {
        public static IEnumerable<Entidade> EmMemoria(this IQueryable<Entidade> entidades)
        {
            return entidades.ToList();
        }

        public static bool PossuiAlgumValor(this IQueryable<Entidade> entidades)
        {
            return entidades.Any();
        }

        public static bool ConstaNoBanco(this IQueryable<Entidade> entidades, int id)
        {
            return entidades.Any(entidade => entidade.Id == id);
        }

        public static Entidade PorId(this IQueryable<Entidade> entidades, int id)
        {
            return entidades.SingleOrDefault(entidade => entidade.Id == id);
        }
    }
}