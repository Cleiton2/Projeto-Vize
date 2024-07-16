using Projeto_Vize.Models;

namespace Projeto_Vize.Repositorio.Interfaces
{
    public interface IProdutoRepositorio
    {
        Task<List<ProdutoModel>> ConsulteProdutos();

        Task<ProdutoModel> ConsulteProdutoPorId(int id);

        Task AdicioneProduto(ProdutoModel produto);

        Task<bool> EhIdCadastrado(int id);

        Task EditeProduto(ProdutoModel produto, int id);

        Task RemovaProduto(int id);

        Task<EstatisticasPorTipoModel> ConsulteEstatisticasProdutosPorTipo();
    }
}