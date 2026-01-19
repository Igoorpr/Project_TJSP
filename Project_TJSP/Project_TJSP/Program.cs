// See https://aka.ms/new-console-template for more information
using Presentation.Controllers;
using Presentation.Interfaces;
using Presentation.Services;

namespace Project_TJSP._2
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            IProcess_TJSP processAppService = new Process_TJSP(); 

            Main mainController = new Main(processAppService);

            await mainController.MainProcess();
        }
    }
}
