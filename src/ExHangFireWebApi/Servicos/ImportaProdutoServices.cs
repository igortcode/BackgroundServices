
using ExcelDataReader;
using ExHangFireWebApi.Context;
using ExHangFireWebApi.Model;
using System.Data;
using System.Transactions;

namespace ExHangFireWebApi.Servicos
{
    public class ImportaProdutoServices : IImportarProdutoServices
    {
        private readonly ApplicationContext _context;

        public ImportaProdutoServices(ApplicationContext context)
        {
            _context = context;
        }


        //Processa um arquivo por vez
        public void ProcessaArquivo()
        {
            if (_context.Arquivos.Any())
            {
                var arquivo =  _context.Arquivos.OrderBy(a => a.DataCadastro).FirstOrDefault();

                try
                {
                    DataSet dataSet = GetDataSet(arquivo.Data);

                    var produtos = CastToProdutos(dataSet);

                    using (var transaction = new TransactionScope())
                    {
                        _context.Produtos.AddRange(produtos);
                         _context.SaveChanges();

                        _context.Logs.Add(new Log { Descricao = $"Arquivo: {arquivo.FileName} processado.", TipoLog = Enum.TipoLog.Sucesso });
                         _context.SaveChanges();

                        _context.Arquivos.Remove(arquivo);
                        _context.SaveChanges();

                        transaction.Complete();
                    }
                }
                catch (Exception ex)
                {
                    _context.Logs.Add(new Log { Descricao = $"Erro ao processar o arquivo {arquivo.FileName}. Erro:"+ex.Message, TipoLog = Enum.TipoLog.Erro });
                    _context.SaveChanges();
                }             
            }
        }

        //Lê um arquivo Excel
        private DataSet GetDataSet(byte[] data)
        {
            var ms = new MemoryStream(data);

            var excelReader = ExcelReaderFactory.CreateOpenXmlReader(ms);

            DataSet result = excelReader.AsDataSet(new ExcelDataSetConfiguration
            {
                ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                {
                    UseHeaderRow = true
                }
            });

            return result;
        }

        private IList<Produto> CastToProdutos(DataSet result)
        {
            var produtos = result.Tables[0].AsEnumerable()
                    .Select(a => new Produto
                    {
                        Nome = a.Field<string>("Nome"),
                        Preco = Convert.ToDecimal(a.Field<double>("Preco")),
                        Quantidade = (int)a.Field<double>("Quantidade")
                    }).ToList();

            return produtos;
        }
    }
}
