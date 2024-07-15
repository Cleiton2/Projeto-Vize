using Npgsql;
using Projeto_Vize.Enum;
using Projeto_Vize.Models;
using Projeto_Vize.Repositorio.Interfaces;
using System.Data;

namespace Projeto_Vize.Repositorio.Consultas
{
    public class ConsultasProduto(IConfiguration configuration) : IConsulta
    {
        private readonly IConfiguration _configuration = configuration;

        public async Task AdicioneProduto(ProdutoModel produtoModel)
        {
            string stringProdutoId = produtoModel.Id > 0 ? "@produtoModel.Id" : "(SELECT MAX(produto_id) + 1 FROM produto)";

            await using NpgsqlConnection conn = new(_configuration.GetConnectionString("PROJETOVIZE"));
            await using NpgsqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @$"INSERT INTO produto (produto_id, produto_nome, produto_tipo, produto_valor)
                                VALUES ({stringProdutoId}, @produto_nome, @produto_tipo, @produto_valor)";

            cmd.Parameters.AddWithValue("produto_id",produtoModel.Id);
            cmd.Parameters.AddWithValue("produto_nome", produtoModel.Nome!);
            cmd.Parameters.AddWithValue("produto_tipo", (int)produtoModel.Tipo);
            cmd.Parameters.AddWithValue("produto_valor", produtoModel.ValorUnidadeProduto!);

            conn.Open();

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<bool> EhIdCadastrado(int id)
        {
            ProdutoModel produto = await ConsulteProdutoPorId(id);

            return produto.Id > 0;
        }

        public async Task EditeProduto(ProdutoModel produtoModel, int id)
        {
            await using NpgsqlConnection conn = new(_configuration.GetConnectionString("PROJETOVIZE"));
            await using NpgsqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"UPDATE produto 
                                SET produto_nome = @produto_nome, produto_tipo = @produto_tipo, produto_valor = @produto_valor
                                WHERE produto_id = @produto_id";

            cmd.Parameters.AddWithValue("produto_nome", produtoModel.Nome!);
            cmd.Parameters.AddWithValue("produto_tipo", produtoModel.Tipo);
            cmd.Parameters.AddWithValue("produto_valor", produtoModel.ValorUnidadeProduto!);
            cmd.Parameters.AddWithValue("produto_id", id);

            conn.Open();

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task RemovaProduto(int id)
        {
            await using NpgsqlConnection conn = new(_configuration.GetConnectionString("PROJETOVIZE"));
            await using NpgsqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"DELETE FROM produto WHERE produto_id = @produto_id";

            cmd.Parameters.AddWithValue("produto_id", id);

            conn.Open();

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<List<ProdutoModel>> ConsulteProdutos()
        {
            await using NpgsqlConnection conn = new(_configuration.GetConnectionString("PROJETOVIZE"));
            await using NpgsqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT produto_id, produto_nome, produto_tipo, produto_valor FROM produto";

            await conn.OpenAsync();

            NpgsqlDataReader dr = await cmd.ExecuteReaderAsync();

            List<ProdutoModel> produtos = [];

            while (dr.Read()) 
            { 
                produtos.Add(MonteDadosCarregamento(dr));
            }

            return produtos;
        }

        public async Task<ProdutoModel> ConsulteProdutoPorId(int id)
        {
            await using NpgsqlConnection conn = new(_configuration.GetConnectionString("PROJETOVIZE"));
            await using NpgsqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT produto_id, produto_nome, produto_tipo, produto_valor 
                                FROM produto WHERE produto_id=@produto_id";

            cmd.Parameters.AddWithValue("produto_id", id);

            await conn.OpenAsync();

            NpgsqlDataReader dr = await cmd.ExecuteReaderAsync();

            ProdutoModel produto = new();

            while (dr.Read())
            {
                produto = MonteDadosCarregamento(dr);
            }


            return produto;
        }

        public async Task<EstatisticasPorTipoModel> ConsulteEstatisticasProdutosPorTipo()
        {
            List<ProdutoModel> produtos = await ConsulteProdutos();

            int quantidadeMaterial = produtos.Count(c => c.Tipo == EnumTipo.Material);
            int quantidadeServico = produtos.Count(c => c.Tipo == EnumTipo.Servico);
            float? valorTotalMateriais = produtos.Where(c => c.Tipo == EnumTipo.Material).Select(c => c.ValorUnidadeProduto).Sum();
            float? valorTotalServicos = produtos.Where(c => c.Tipo == EnumTipo.Servico).Select(c => c.ValorUnidadeProduto).Sum();

            EstatisticasPorTipoModel estatisticas = new()
            {
                QuantidadeMaterial = quantidadeMaterial,
                QuantidadeSerico = quantidadeServico,
                MediaValorMaterial = valorTotalMateriais / quantidadeMaterial,
                MediaValorSerico = valorTotalServicos / quantidadeServico
            };

            return estatisticas;
        }

        private static ProdutoModel MonteDadosCarregamento(NpgsqlDataReader dr)
        {
            ProdutoModel produtoModel = new()
            {
                Id = dr.GetInt32("produto_id"),
                Nome = dr.GetString("produto_nome"),
                Tipo =  (EnumTipo)dr.GetInt32("produto_tipo")
                ,
                ValorUnidadeProduto = dr.GetFloat("produto_valor")
            };

            return produtoModel;
        }
    }
}