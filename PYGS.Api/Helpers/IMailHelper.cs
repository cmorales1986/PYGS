using PYGS.Shared.Responses;

namespace PYGS.Api.Helpers
{
    public interface IMailHelper
    {
        Response SendMail(string toName, string toEmail, string subject, string body);
    }
}
