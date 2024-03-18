using ExHangFireWebApi.Enum;
using System.ComponentModel.DataAnnotations;

namespace ExHangFireWebApi.Model
{
    public class Log
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Descricao { get; set; }
        public TipoLog TipoLog { get; set; }
    }
}
