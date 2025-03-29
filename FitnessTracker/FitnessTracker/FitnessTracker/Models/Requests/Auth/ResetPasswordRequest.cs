// FitnessTracker.API/Models/Requests/Auth/ResetPasswordRequest.cs
namespace FitnessTracker.API.Models.Requests.Auth
{
    public class ResetPasswordRequest
    {
        public int UserId { get; set; }
        public string NewPassword { get; set; }
    }
}