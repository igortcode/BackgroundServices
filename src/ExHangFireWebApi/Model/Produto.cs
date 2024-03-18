using System.ComponentModel.DataAnnotations;

namespace ExHangFireWebApi.Model
{
    public class Produto
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        public decimal Preco { get; set; }
        [Required]
        public int Quantidade { get; set; }


    }
}
