using ExHangFireWebApi.Context;
using ExHangFireWebApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace ExHangFireWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public FilesController(ApplicationContext context)
        {
            _context = context;
        }

        //Salva o arquivo de importação

        [HttpPost("EnfileirarArquivos")]
        public async Task<IActionResult> EnfilerarParaImportacao(IFormFile file)
        {
            byte[] data = null;

            using(var ms = new MemoryStream())
            {
               file.OpenReadStream().CopyTo(ms);

                data = ms.ToArray();
            }

            var arquivo = new Arquivo(file.Name, file.ContentType, data);


            _context.Arquivos.Add(arquivo);
            await _context.SaveChangesAsync();

            return Ok("Arquivo enfileirado com sucesso!");
        }
    }
}
