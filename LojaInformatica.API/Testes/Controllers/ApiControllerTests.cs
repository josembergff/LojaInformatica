using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using LojaInformatica.API.Controllers;
using LojaInformatica.API.Entidades;
using LojaInformatica.API.Testes.Configuracao;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace LojaInformatica.API.Testes.Controllers
{
    public abstract class ApiControllerTests<TEntidade> where TEntidade : Entidade<TEntidade>, new()
    {
        protected readonly string _chaveDoBanco;
        protected abstract IEntidadeApi<TEntidade> ObterApiController();
        protected abstract IEnumerable<TEntidade> ObterExemploEntidades();
        protected abstract TEntidade ObterExemploEntidadeInvalidaParaInsercao();
        protected abstract TEntidade ObterExemploEntidadeValidaParaInsercao();
        protected abstract TEntidade ObterExemploEntidadeInvalidaParaAtualizacao();
        protected abstract TEntidade ObterExemploEntidadeValidaParaAtualizacao();
        protected virtual bool CompararEntidades(IEnumerable<TEntidade> entidadesObtidas, IEnumerable<TEntidade> entidadesEsperadas)
        {
            if (entidadesObtidas.Count() != entidadesEsperadas.Count())
                return false;

            foreach (var entidadeEsperada in entidadesObtidas)
                if (!entidadesEsperadas.Any(entidadeObtida => entidadeObtida.EquivaleA(entidadeEsperada)))
                    return false;

            return true;
        }
        protected virtual void PersistirEntidades(IEnumerable<TEntidade> clientes)
        {
            var ambienteDeTeste = AmbienteDeTeste.NovoAmbiente(_chaveDoBanco);
            ambienteDeTeste.Contexto.Set<TEntidade>().AddRange(clientes);
            ambienteDeTeste.Contexto.SaveChanges();
        }
        protected virtual void PersistirEntidade(TEntidade entidade)
        {
            PersistirEntidades(new[] { entidade });
        }

        protected readonly AmbienteDeTeste _ambienteDeTeste;
        protected IEntidadeApi<TEntidade> _controller;
        protected ApiControllerTests()
        {
            _chaveDoBanco = Guid.NewGuid().ToString();
            _ambienteDeTeste = AmbienteDeTeste.NovoAmbiente(_chaveDoBanco);
            _controller = ObterApiController();
        }

        [Fact]
        public void Api_Get_Deve_retornar_uma_lista_vazia_quando_não_houver_dados_no_banco()
        {
            var resultado = _controller.Get() as OkObjectResult;
            var entidades = resultado.Value as IEnumerable<TEntidade>;

            entidades.Should().BeEmpty();
        }

        [Fact]
        public void Api_Get_Deve_retornar_a_lista_de_entidades_registradas_quando_houver_entidades_persistidas()
        {
            var entidadesPersistidas = ObterExemploEntidades();
            PersistirEntidades(entidadesPersistidas);

            _controller = ObterApiController();
            var resultado = _controller.Get() as OkObjectResult;
            var entidades = resultado.Value as IEnumerable<TEntidade>;

            CompararEntidades(entidades, entidadesPersistidas);
        }

        [Fact]
        public void Api_Get_Id_Deve_retornar_NotFound_quando_a_entidade_não_existir()
        {
            var idAusenteNoBanco = 404;

            var resultado = _controller.Get(idAusenteNoBanco);

            resultado.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void Api_Get_Id_Deve_retornar_a_entidade_específica_se_a_mesma_existir()
        {
            var entidades = ObterExemploEntidades();
            PersistirEntidades(entidades);
            var entidadeDeExemplo = entidades.First();

            var resultado = _controller.Get(entidadeDeExemplo.Id) as OkObjectResult;
            var entidade = resultado.Value as TEntidade;

            entidade.ShouldBeEquivalentTo(entidadeDeExemplo);
        }

        [Fact]
        public void Api_Post_Deve_retornar_BadRequest_quando_a_entidade_não_estiver_válida_para_inserção()
        {
            var entidadeDeExemplo = ObterExemploEntidadeInvalidaParaInsercao();

            var resultado = _controller.Post(entidadeDeExemplo);

            resultado.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public void Api_Post_Deve_retornar_CreatedAtRoute_quando_a_entidade_estiver_válida_para_inserção()
        {
            var entidadeDeExemplo = ObterExemploEntidadeValidaParaInsercao();

            var resultado = _controller.Post(entidadeDeExemplo) as CreatedAtRouteResult;
            var entidade = resultado.Value as TEntidade;
            var id = resultado.RouteValues["Id"].As<int>();

            entidade.Should().NotBeNull();
            id.Should().Be(entidade.Id);
        }

        [Fact]
        public void Api_Put_Deve_retornar_BadRequest_quando_a_entidade_não_estiver_válida_para_atualização()
        {
            var entidadeDeExemplo = ObterExemploEntidadeInvalidaParaAtualizacao();

            var resultado = _controller.Put(entidadeDeExemplo);

            resultado.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public void Api_Put_Deve_retornar_NotFound_quando_a_id_solicitada_não_constar_no_banco()
        {
            var entidadeDeExemplo = ObterExemploEntidadeValidaParaAtualizacao();
            entidadeDeExemplo.Id = 400;

            var resultado = _controller.Put(entidadeDeExemplo);

            resultado.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void Api_Put_Deve_retornar_NoContentResult_quando_a_entidade_estiver_válida_para_atualização()
        {
            var entidadeDeExemplo = ObterExemploEntidadeValidaParaAtualizacao();
            PersistirEntidade(entidadeDeExemplo);

            var resultado = _controller.Put(entidadeDeExemplo);

            resultado.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public void Api_Delete_Deve_retornar_NotFound_quando_a_entidade_não_existir()
        {
            var idAusenteNoBanco = 400;

            var resultado = _controller.Delete(idAusenteNoBanco);

            resultado.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void Api_Delete_Deve_retornar_NoContent_quando_a_entidade_for_excluída_com_sucesso()
        {
            var entidadeDeExemplo = ObterExemploEntidadeValidaParaInsercao();
            PersistirEntidade(entidadeDeExemplo);
            var idEntidade = entidadeDeExemplo.Id;

            var resultado = _controller.Delete(idEntidade);

            resultado.Should().BeOfType<NoContentResult>();
        }
    }
}