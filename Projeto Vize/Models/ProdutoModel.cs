using System.ComponentModel.DataAnnotations;
using Projeto_Vize.Enum;

namespace Projeto_Vize.Models
{
    public class ProdutoModel
    {
        [Key]
        public int Id { get; set; }

        public string? Nome { get; set; }

        public EnumTipo Tipo { get; set; }

        public float? ValorUnidadeProduto { get; set; }
    }
}