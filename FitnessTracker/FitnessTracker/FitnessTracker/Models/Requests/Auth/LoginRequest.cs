﻿// FitnessTracker.API/Models/Requests/Auth/LoginRequest.cs
namespace FitnessTracker.API.Models.Requests.Auth
{
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
