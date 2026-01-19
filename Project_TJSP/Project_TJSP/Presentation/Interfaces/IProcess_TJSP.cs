using Presentation.DTO;

namespace Presentation.Interfaces
{
    public interface IProcess_TJSP
    {
        Task OpenWebsite();
        Task ConsultProcess(ConsultDTO processnumber);
    }
}
