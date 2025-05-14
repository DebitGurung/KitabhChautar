namespace KitabhChautari.Services
{
    public interface IEmailService
    {
        Task SendStaffCredentialsAsync(string email, string password);
    }
}