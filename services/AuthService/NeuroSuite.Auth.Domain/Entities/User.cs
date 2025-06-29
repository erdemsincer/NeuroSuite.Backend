﻿namespace NeuroSuite.Auth.Domain.Entities;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;

    public string Role { get; set; } = "User"; // Varsayılan rol

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
