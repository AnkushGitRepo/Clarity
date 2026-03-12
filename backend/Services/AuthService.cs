using Clarity.Api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using BCrypt.Net;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Clarity.Api.Services;

public class AuthService
{
    private readonly IMongoCollection<User> _usersCollection;
    private readonly IConfiguration _configuration;

    public AuthService(IOptions<MongoDBSettings> mongoDBSettings, IConfiguration configuration)
    {
        var mongoClient = new MongoClient(mongoDBSettings.Value.ConnectionURI);
        var mongoDatabase = mongoClient.GetDatabase(mongoDBSettings.Value.DatabaseName);
        _usersCollection = mongoDatabase.GetCollection<User>(mongoDBSettings.Value.UsersCollectionName);
        _configuration = configuration;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _usersCollection.Find(u => u.Email == email).FirstOrDefaultAsync();
    }

    public async Task<User> RegisterUserAsync(string username, string email, string password)
    {
        var existingUser = await GetUserByEmailAsync(email);
        if (existingUser != null)
        {
            throw new Exception("Email already in use");
        }

        var newUser = new User
        {
            Username = username,
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            CreatedAt = DateTime.UtcNow
        };

        await _usersCollection.InsertOneAsync(newUser);
        return newUser;
    }

    public async Task<string?> LoginAsync(string email, string password)
    {
        var user = await GetUserByEmailAsync(email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            return null; // Invalid credentials
        }

        return GenerateJwtToken(user);
    }

    private string GenerateJwtToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id!),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
