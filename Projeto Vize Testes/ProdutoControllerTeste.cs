using AutoFixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Projeto_Vize.Controllers;
using Projeto_Vize.Models;
using Projeto_Vize.Repositorio.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Projeto_Vize_Testes
{
    [TestClass]
    public class ProdutoControllerTeste
    {
        private readonly Mock<IProdutoRepositorio> _mockProdutoRepositorio;
        private readonly Fixture _fixture;
        private ProdutoController? _produtoController;

        public ProdutoControllerTeste()
        {
            _fixture = new Fixture();
            _mockProdutoRepositorio = new Mock<IProdutoRepositorio>();
        }

        [Fact]
        public async Task ConsulteProdutosTeste()
        {
            Task<List<ProdutoModel>> produtos = _fixture.Create<Task<List<ProdutoModel>>>();

            _mockProdutoRepositorio.Setup(repo => repo.ConsulteProdutos()).Returns(produtos);

            _produtoController = new ProdutoController(_mockProdutoRepositorio.Object);

            ActionResult<List<ProdutoModel>> result = await _produtoController.ObtenhaProdutos();

            Xunit.Assert.NotNull(result.Value);

        }

        [Fact]
        public async Task ConsulteProdutosPorIdTeste()
        {
            Task<ProdutoModel> produto = _fixture.Create<Task<ProdutoModel>>();

            int id = ObtenhaResultadoProduto(produto).Id;

            _mockProdutoRepositorio.Setup(repo => repo.ConsulteProdutoPorId(id)).Returns(produto);

            _produtoController = new ProdutoController(_mockProdutoRepositorio.Object);

            ActionResult<ProdutoModel> result = await _produtoController.ObtenhaProdutoPorId(id);

            Xunit.Assert.NotNull(result.Value);

        }

        [Fact]
        public async Task AdicioneProdutoTeste()
        {
            Task<ProdutoModel> produto = _fixture.Create<Task<ProdutoModel>>();

            ProdutoModel produtoAdicionado = ObtenhaResultadoProduto(produto);

            _mockProdutoRepositorio.Setup(repo => repo.AdicioneProduto(produtoAdicionado)).Returns(produto);

            _produtoController = new ProdutoController(_mockProdutoRepositorio.Object);

            ActionResult<ProdutoModel> result = await _produtoController.AdicioneProduto(produtoAdicionado);

            Xunit.Assert.NotNull(result.Value);

        }

        [Fact]
        public async Task RemovaProdutoTeste()
        {
            Task<ProdutoModel> produto = _fixture.Create<Task<ProdutoModel>>();

            int idProdutoAdicionado = ObtenhaResultadoProduto(produto).Id;

            _mockProdutoRepositorio.Setup(repo => repo.RemovaProduto(idProdutoAdicionado)).Returns(produto);

            _produtoController = new ProdutoController(_mockProdutoRepositorio.Object);

            ActionResult<ProdutoModel> result = await _produtoController.RemovaProduto(idProdutoAdicionado);

            Xunit.Assert.Null(result.Value);

        }

        [Fact]
        public async Task EditeProdutoTeste()
        {
            Task<ProdutoModel> produto = _fixture.Create<Task<ProdutoModel>>();

            ProdutoModel produtoEditado = ObtenhaResultadoProduto(produto);

            _mockProdutoRepositorio.Setup(repo => repo.EditeProduto(produtoEditado, produtoEditado.Id)).Returns(produto);

            _produtoController = new ProdutoController(_mockProdutoRepositorio.Object);

            ActionResult<ProdutoModel> result = await _produtoController.EditeProduto(produtoEditado, produtoEditado.Id);

            Xunit.Assert.NotNull(result.Value);

        }

        [Fact]
        public async Task EstatisticasProdutoTeste()
        {
            Task<EstatisticasPorTipoModel> estatisticasProduto = _fixture.Create<Task<EstatisticasPorTipoModel>>();

            EstatisticasPorTipoModel produtoEditado = ObtenhaResultadoEstatisticasProduto(estatisticasProduto);

            _mockProdutoRepositorio.Setup(repo => repo.ConsulteEstatisticasProdutosPorTipo()).Returns(estatisticasProduto);

            _produtoController = new ProdutoController(_mockProdutoRepositorio.Object);

            ActionResult<EstatisticasPorTipoModel> resultado = await _produtoController.ConsulteEstatisticasProdutos();

            Xunit.Assert.NotNull(resultado.Value);

        }

        private static ProdutoModel ObtenhaResultadoProduto(Task<ProdutoModel> produto)
        {
            return produto.Result;
        }

        private static EstatisticasPorTipoModel ObtenhaResultadoEstatisticasProduto(Task<EstatisticasPorTipoModel> produto)
        {
            return produto.Result;
        }
    }
}