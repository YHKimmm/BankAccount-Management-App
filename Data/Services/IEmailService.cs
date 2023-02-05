using ASP.Net_MVC_Assignment.Models;
using SendGrid;

namespace ASP.Net_MVC_Assignment.Data.Services
{
    public interface IEmailService
    {
        Task<Response> SendSingleEmail(ComposeEmailModel payload);
    }

}
