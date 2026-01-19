using System.ComponentModel.DataAnnotations;
using Presentation.Interfaces;
using Presentation.DTO;

namespace Presentation.Controllers
{
    public class Main
    {
        private readonly IProcess_TJSP _processAppService;

        public Main(IProcess_TJSP processAppService)
        {
            (_processAppService) = (processAppService);
        }

        public async Task MainProcess()
        {

                string Options;
                do
                {
                    Console.Write("0- Stop\n");
                    Console.Write("1- Website\n");
                    Console.Write("2- Consult Process\n");
                    Options = Console.ReadLine();

                    switch (Options)
                    {
                        case "0":                  
                            break;

                        case "1":
                            await _processAppService.OpenWebsite();
                        break;

                        case "2":
                            try
                            {
                                Console.Write("Enter process number: ");
                                string processNumber = Console.ReadLine();

                                var consult = new ConsultDTO
                                {
                                    ProcessNumber = processNumber
                                };

                                await _processAppService.ConsultProcess(consult);
                            }
                            catch (ValidationException ex)
                            {
                                Console.Write(ex.Message + ("(400)\n"));
                            }
                            catch (Exception ex)
                            {
                                Console.Write("Internal Server Error(500).\n");
                            }
                        break;

                        default:
                            Console.Write("ERROR: Invalid option.\n");
                        break;
                    }

                Console.Write("\n");
            } while (Options != "0");
        }
    }
}

