namespace UmbracoDemo.Services
{
    public interface IEmailService
    {
        void SendEmail(string from, string to, string title, string message);
    }
}