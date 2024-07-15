using Microsoft.AspNetCore.Mvc;
using Projeto_Vize.Models;
using Projeto_Vize.Repositorio;

namespace Projeto_Vize.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController(IConfiguration configuration) : ControllerBase
    {
        private readonly IConfiguration _config = configuration;

        [HttpGet]
        [Route("ObtenhaProdutos")]
        public async Task<ActionResult<List<ProdutoModel>>> ObtenhaProdutos()
        {
            List<ProdutoModel> produtos = await new ProdutoRepositorio(_config).ConsulteProdutos();

            return produtos.Count == 0 ? BadRequest("Nenhum produto encontrado") : produtos;
        }
           

        [HttpGet]
        [Route("ObtenhaProdutoPorId/{id}")]
        public async Task<ActionResult<ProdutoModel>> ObtenhaProdutoPorId(int id)
        {
            ProdutoModel produto = await new ProdutoRepositorio(_config).ConsulteProdutoPorId(id);

            return produto.Id >= 0 ?  produto : BadRequest("Produto não Cadastrado");
        }
            

        [HttpGet]
        [Route("ConsulteEstatisticasProdutos")]
        public async Task<ActionResult<EstatisticasPorTipoModel>> ConsulteEstatisticasProdutos() =>
            await new ProdutoRepositorio(_config).ConsulteEstatisticasProdutosPorTipo();

        [HttpPost]
        [Route("AdicioneProduto")]
        public async Task<ActionResult<ProdutoModel>> AdicioneProduto([FromBody] ProdutoModel produto)
        {
            if (produto.Nome is null || (int)produto.Tipo is not 0 or 1)
            {
                return BadRequest("Nome e Tipo do prodduto são obrigatórios!");
            }

            bool ehIdCadastrado = await new ProdutoRepositorio(_config).EhIdCadastrado(produto.Id);

            if(ehIdCadastrado)
            {
                return BadRequest("Id já utilizado!");
            }

            await new ProdutoRepositorio(_config).AdicioneProduto(produto);

            return produto;
        }

        [HttpPost]
        [Route("EditeProduto/{id}")]
        public async Task<ActionResult<ProdutoModel>> EditeProduto([FromBody] ProdutoModel produto, int id)
        {
            if (id <= 0)
            {
                return BadRequest("Informe um id válido");
            }

            await new ProdutoRepositorio(_config).EditeProduto(produto, id);

            return produto;
        }

        [HttpDelete]
        [Route("RemovaProduto/{id}")]
        public async Task RemovaProduto(int id)
        {
            if (id > 0)
            {
                await new ProdutoRepositorio(_config).RemovaProduto(id);
            }
        }
    }
}