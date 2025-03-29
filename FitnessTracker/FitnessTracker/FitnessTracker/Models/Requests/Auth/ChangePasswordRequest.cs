
// FitnessTracker.API/Models/Requests/Auth/ChangePasswordRequest.cs
namespace FitnessTracker.API.Models.Requests.Auth
{
    public class ChangePasswordRequest
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
