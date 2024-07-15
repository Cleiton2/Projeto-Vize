using Projeto_Vize.Models;
using Projeto_Vize.Repositorio.Consultas;
using Projeto_Vize.Repositorio.Interfaces;

namespace Projeto_Vize.Repositorio
{
    public class ProdutoRepositorio(IConfiguration configuration) : IProdutoRepositorio
    {
        private readonly IConfiguration _configuration = configuration;

        public async Task AdicioneProduto(ProdutoModel produtoModel) =>
            await new ConsultasProduto(_configuration).AdicioneProduto(produtoModel);

        public async Task<bool> EhIdCadastrado(int id) =>
            await new ConsultasProduto(_configuration).EhIdCadastrado(id);

        public async Task<EstatisticasPorTipoModel> ConsulteEstatisticasProdutosPorTipo() =>
            await new ConsultasProduto(_configuration).ConsulteEstatisticasProdutosPorTipo();

        public async Task<ProdutoModel> ConsulteProdutoPorId(int id) =>
            await new ConsultasProduto(_configuration).ConsulteProdutoPorId(id);

        public async Task<List<ProdutoModel>> ConsulteProdutos() =>
            await new ConsultasProduto(_configuration).ConsulteProdutos();

        public async Task EditeProduto(ProdutoModel produto, int id) =>
            await new ConsultasProduto(_configuration).EditeProduto(produto, id);


        public async Task RemovaProduto(int id) =>
            await new ConsultasProduto(_configuration).RemovaProduto(id);
    }
}