using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Presentation.Interfaces;
using System.Diagnostics;
using Presentation.DTO;
using System.Net;

namespace Presentation.Services
{
    public class Process_TJSP : IProcess_TJSP
    {
        public async Task OpenWebsite()
        {
            string url = "https://esaj.tjsp.jus.br/cpopg/open.do";

            Console.Write("Type 1 to open the browser or 2 to save HTML: ");
            string number = Console.ReadLine();

            if (number == "1")
            {
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = url,
                        UseShellExecute = true
                    });

                    Console.WriteLine("Browser opened successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error opening browser: {ex.Message}");
                }
            }
            else
            {
                string filePath = "GetWebsite.html";

                var handler = new HttpClientHandler();
                handler.UseCookies = true;
                handler.CookieContainer = new CookieContainer();

                // Add cookies
                handler.CookieContainer.Add(
                    new Uri(url),
                    new Cookie("JSESSIONID", "69AA991B4A20197BD8D374DD57F3D928.cpopg2")
                );
                handler.CookieContainer.Add(
                    new Uri(url),
                    new Cookie("K-JSESSIONID-knbbofpc", "FEB2F2C50EC1A46338244BA9DF3C2BD9")
                );

                using var client = new HttpClient(handler);

                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode(); 

                    string returnHtml = await response.Content.ReadAsStringAsync();

                    Console.WriteLine(returnHtml);
                    File.WriteAllText(filePath, returnHtml);

                    Console.WriteLine("GET request completed successfully!");
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"HTTP error: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        public async Task ConsultProcess(ConsultDTO processnumber)
        {
            string url = "https://esaj.tjsp.jus.br/cpopg/show.do?processo.codigo=FM0010YXH0000&processo.foro=562&processo.numero=";

            if (ValidateNumber(processnumber.ProcessNumber) == false)
            {
                throw new ValidationException("Process number is invalid.");
            }

            string filePath = "GetConsultProcessWebsite.html";

            var handler = new HttpClientHandler();
            handler.UseCookies = true;
            handler.CookieContainer = new CookieContainer();

            // Add cookies
            handler.CookieContainer.Add(
                new Uri(url),
                new Cookie("JSESSIONID", "69AA991B4A20197BD8D374DD57F3D928.cpopg2")
            );
            handler.CookieContainer.Add(
                new Uri(url),
                new Cookie("K-JSESSIONID-knbbofpc", "FEB2F2C50EC1A46338244BA9DF3C2BD9")
            );

            using var client = new HttpClient(handler);

            try
            {
                HttpResponseMessage response = await client.GetAsync(url + processnumber.ProcessNumber);
                response.EnsureSuccessStatusCode();

                string returnHtml = await response.Content.ReadAsStringAsync();

                Console.WriteLine(returnHtml);
                File.WriteAllText(filePath, returnHtml);

                Console.WriteLine("GET request completed successfully!");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public bool ValidateNumber(string processnumber)
        {
            bool isvalid = true;

            if (string.IsNullOrWhiteSpace(processnumber))
            {
                isvalid = false;
            }
                
            var regex = new Regex(@"^\d{7}-\d{2}\.\d{4}\.\d\.\d{2}\.\d{4}$");

            if (!regex.IsMatch(processnumber))
            {
                isvalid = false;
            }

            return isvalid;
        }
    }
}
