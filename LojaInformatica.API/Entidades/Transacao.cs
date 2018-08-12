using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using prmToolkit.NotificationPattern;

namespace LojaInformatica.API.Entidades
{
    public class Transacao : EntidadeCollection
    {
        [BsonDateTimeOptions]
        public DateTime DataTransacao { get; set; }
        public string Nome { get; set; }
        public Compra Compra { get; set; }
        public Cliente Cliente { get; set; }
        public IEnumerable<Produto> Produtos { get; set; }
        public override string NomePlural()
        {
            return "Transacoes";
        }

        internal override bool EstaValidoParaInsercao()
        {
            EstaValidoParaInsercao();
            return IsValid();
        }

        internal override bool EstaValidoParaAtualizacao()
        {
            ValidaTodosOsCamposObrigatorios();
            return IsValid();
        }

        private void ValidaTodosOsCamposObrigatorios()
        {
            new AddNotifications<Transacao>(this).IfNull(x => x, "Pelo menos um parâmetro deve ser informado");

            new AddNotifications<Transacao>(this).IfNullOrEmpty(x => x.Nome, "O nome não pode ser vazio");

            new AddNotifications<Transacao>(this).IfLengthLowerThan(x => x.Nome, 4, "O nome têm que ter mais que 3 caracteres");

            new AddNotifications<Transacao>(this).IfLengthGreaterThan(x => x.Nome, 200, "O nome têm que ter menos que 200 caracteres");

            new AddNotifications<Transacao>(this).IfLowerOrEqualsThan(x => x.DataTransacao, DateTime.MinValue, "A data da transação esta inválida");

        }
    }
}