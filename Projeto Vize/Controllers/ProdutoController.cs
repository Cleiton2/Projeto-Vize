using Microsoft.AspNetCore.Mvc;
using Projeto_Vize.Models;
using Projeto_Vize.Repositorio.Interfaces;

namespace Projeto_Vize.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController(IProdutoRepositorio _repositorioProduto) : ControllerBase
    {
        private readonly IProdutoRepositorio _repositorioProduto = _repositorioProduto;

        [HttpGet]
        [Route("ObtenhaProdutos")]
        public async Task<ActionResult<List<ProdutoModel>>> ObtenhaProdutos()
        {
            List<ProdutoModel> produtos = await _repositorioProduto.ConsulteProdutos();

            return produtos.Count == 0 ? BadRequest("Nenhum produto encontrado") : produtos;
        }
           

        [HttpGet]
        [Route("ObtenhaProdutoPorId/{id}")]
        public async Task<ActionResult<ProdutoModel>> ObtenhaProdutoPorId(int id)
        {
            ProdutoModel produto = await _repositorioProduto.ConsulteProdutoPorId(id);

            return produto.Id != 0 ?  produto : BadRequest("Produto não Cadastrado");
        }
            

        [HttpGet]
        [Route("ConsulteEstatisticasProdutos")]
        public async Task<ActionResult<EstatisticasPorTipoModel>> ConsulteEstatisticasProdutos() =>
            await _repositorioProduto.ConsulteEstatisticasProdutosPorTipo();

        [HttpPost]
        [Route("AdicioneProduto")]
        public async Task<ActionResult<ProdutoModel>> AdicioneProduto([FromBody] ProdutoModel produto)
        {
            if (produto.Nome is null || (int)produto.Tipo is not 0 or 1)
            {
                return BadRequest("Nome e Tipo do prodduto são obrigatórios!");
            }

            bool ehIdCadastrado = await _repositorioProduto.EhIdCadastrado(produto.Id);

            if(ehIdCadastrado)
            {
                return BadRequest("Id já utilizado!");
            }

            await _repositorioProduto.AdicioneProduto(produto);

            return produto;
        }

        [HttpPost]
        [Route("EditeProduto/{id}")]
        public async Task<ActionResult> EditeProduto([FromBody] ProdutoModel produto, int id)
        {
            if (id <= 0)
            {
                return BadRequest("Informe um id válido");
            }

            await _repositorioProduto.EditeProduto(produto, id);

            return Ok();
        }

        [HttpDelete]
        [Route("RemovaProduto/{id}")]
        public async Task<ActionResult> RemovaProduto(int id)
        {
            bool ehIdCadastrado = await _repositorioProduto.EhIdCadastrado(id);

            if (ehIdCadastrado)
            {
                return BadRequest("Código de produto não cadastrado!");
            }

            await _repositorioProduto.RemovaProduto(id);

            return Ok();
        }
    }
}