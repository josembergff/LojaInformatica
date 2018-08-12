using System;
using System.Linq;
using prmToolkit.NotificationPattern;

namespace LojaInformatica.API.Entidades
{
    public class Cliente : Entidade<Cliente>
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public Guid ChaveDeAcesso { get; set; }

        internal override bool EstaValidoParaInsercao()
        {
            ValidaTodosOsCamposObrigatorios();

            return base.EstaValidoParaInsercao() && IsValid();
        }

        internal override bool EstaValidoParaAtualizacao()
        {
            ValidaTodosOsCamposObrigatorios();

            return base.EstaValidoParaAtualizacao() && IsValid();
        }



        private void ValidaTodosOsCamposObrigatorios()
        {
            new AddNotifications<Cliente>(this).IfNull(x => x, "Pelo menos um parâmetro deve ser informado");

            new AddNotifications<Cliente>(this).IfNullOrEmpty(x => x.Nome, "O nome não pode ser vazio");

            new AddNotifications<Cliente>(this).IfLengthLowerThan(x => x.Nome, 4, "O nome têm que ter mais que 3 caracteres");

            new AddNotifications<Cliente>(this).IfLengthGreaterThan(x => x.Nome, 200, "O nome têm que ter menos que 200 caracteres");

            new AddNotifications<Cliente>(this).IfNullOrEmpty(x => x.Email, "O e-mail não pode ser vazio");

            new AddNotifications<Cliente>(this).IfLengthLowerThan(x => x.Email, 11, "O e-mail têm que ter mais que 10 caracteres");

            new AddNotifications<Cliente>(this).IfLengthGreaterThan(x => x.Email, 200, "O e-mail têm que ter menos que 200 caracteres");
        }

        public override bool EquivaleA(Cliente outroCliente)
        {
            return base.EquivaleA(outroCliente) &&
                Nome == outroCliente.Nome &&
                Email == outroCliente.Email;
        }
    }

    public static class ClienteExtensions
    {
        public static IQueryable<Cliente> PorNome(this IQueryable<Cliente> clientes, string nome)
        {
            var nomeMinusculo = nome.ToLower();
            return clientes.Where(cliente => cliente.Nome.ToLower().Contains(nomeMinusculo));
        }

        public static IQueryable<Cliente> PorNomeExato(this IQueryable<Cliente> clientes, string nomeExato)
        {
            return clientes.Where(cliente => cliente.Nome == nomeExato);
        }
    }
}