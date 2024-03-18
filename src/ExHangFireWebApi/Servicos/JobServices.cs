using Hangfire;

namespace ExHangFireWebApi.Servicos
{
    public class JobServices : IJobServices
    {
        private readonly IImportarProdutoServices _importarProdutoServices;

        public JobServices(IImportarProdutoServices importarProdutoServices)
        {
            _importarProdutoServices = importarProdutoServices;
        }

        //Cria o RecurringJob com intervalo de 1 min
        public void ProcessarArquivoProduto()
        {
            RecurringJob.AddOrUpdate("ImportaArquivos", () => _importarProdutoServices.ProcessaArquivo(), Cron.MinuteInterval(1));
        }
    }
}
