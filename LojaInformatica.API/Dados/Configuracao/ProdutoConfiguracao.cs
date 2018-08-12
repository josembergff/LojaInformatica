using LojaInformatica.API.Entidades;
using Microsoft.EntityFrameworkCore;

namespace LojaInformatica.API.Dados.Configuracao
{
    public class ProdutoConfiguracao : IEntityTypeConfiguration<Produto>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Produto> builder)
        {
            builder.HasMany(produto => produto.Imagens);
            builder.Ignore(produto => produto.Notifications);
        }
    }
}