using LojaInformatica.API.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LojaInformatica.API.Dados.Configuracao {
    public class ItemDaCompraConfiguracao : IEntityTypeConfiguration<ItemDaCompra> {
        public void Configure (EntityTypeBuilder<ItemDaCompra> builder) {
            builder.HasKey (itemDaCompra => new { itemDaCompra.CompraId, itemDaCompra.ProdutoId });
            builder.HasOne (itemDaCompra => itemDaCompra.Compra)
                .WithMany (compra => compra.ItensDaCompra)
                .HasForeignKey (ItemDaCompra => ItemDaCompra.CompraId);
            builder.HasOne (itemDaCompra => itemDaCompra.Produto)
                .WithMany (produto => produto.ItemComprados)
                .HasForeignKey (itemDaCompra => itemDaCompra.Compra.Notifications);
        }
    }
}