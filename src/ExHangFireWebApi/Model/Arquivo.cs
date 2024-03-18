using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExHangFireWebApi.Model
{
    public class Arquivo
    {
        [Key]
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string MimeType { get; set; }
        public byte[] Data { get; set; }
        public DateTime DataCadastro { get; set; }

        public Arquivo()
        {
                
        }

        public Arquivo(string fileName, string mimeType, byte[] data)
        {
            Id = Guid.NewGuid();
            FileName = fileName;
            MimeType = mimeType;
            Data = data;

            DataCadastro = DateTime.Now;
        }
    }
}
