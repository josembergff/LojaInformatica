using System.Collections.Generic;
using System.Linq;
using prmToolkit.NotificationPattern;

namespace LojaInformatica.API.Entidades {
    public class Produto : Entidade<Produto> {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }

        public virtual ICollection<Imagem> Imagens { get; set; }
        public virtual ICollection<ItemDaCompra> ItemComprados { get; set; }

        internal override bool EstaValidoParaInsercao () {
            EstaValidoParaInsercao ();
            return base.EstaValidoParaAtualizacao () && IsValid ();
        }

        internal override bool EstaValidoParaAtualizacao () {
            ValidaTodosOsCamposObrigatorios ();
            return base.EstaValidoParaAtualizacao () && IsValid ();
        }

        public override bool EquivaleA (Produto outroProduto) {
            return base.EquivaleA (outroProduto) &&
                Nome == outroProduto.Nome &&
                Descricao == outroProduto.Descricao &&
                Preco == outroProduto.Preco &&
                Imagens.EquivalemA (outroProduto.Imagens);
        }

        private void ValidaTodosOsCamposObrigatorios () {
            new AddNotifications<Produto> (this).IfNull (x => x, "Pelo menos um parâmetro deve ser informado");

            new AddNotifications<Produto> (this).IfCollectionIsNull (x => x.Imagens, "Pelo menos uma imagem deve ser informado");

            new AddNotifications<Produto> (this).IfNullOrEmpty (x => x.Nome, "O nome não pode ser vazio");

            new AddNotifications<Produto> (this).IfLengthLowerThan (x => x.Nome, 4, "O nome têm que ter mais que 3 caracteres");

            new AddNotifications<Produto> (this).IfLengthGreaterThan (x => x.Nome, 200, "O nome têm que ter menos que 200 caracteres");

            new AddNotifications<Produto> (this).IfNullOrEmpty (x => x.Descricao, "A descrição não pode ser vazio");

            new AddNotifications<Produto> (this).IfLengthLowerThan (x => x.Descricao, 11, "A descrição têm que ter mais que 10 caracteres");

            new AddNotifications<Produto> (this).IfLengthGreaterThan (x => x.Descricao, 200, "A descrição têm que ter menos que 200 caracteres");
        }
    }

    public class Imagem : Entidade {
        public string URL { get; set; }
        public bool ImagemPrincipal { get; set; }

        public int ProdutoId { get; set; }

    }

    public static class ImagemExtensions {
        public static bool EquivalemA (this IEnumerable<Imagem> imagens, IEnumerable<Imagem> outrasImagens) {
            var idsDasImagens = imagens.Select (imagem => imagem.Id).Distinct ();
            var idsDasOutrasImagens = outrasImagens.Select (imagem => imagem.Id).Distinct ();

            return idsDasImagens.Count () == idsDasOutrasImagens.Count () &&
                idsDasImagens.All (idDaImagem => idsDasOutrasImagens.Contains (idDaImagem));
        }
    }
}