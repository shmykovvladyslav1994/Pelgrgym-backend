using backend.Data;
using backend.Models;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly AppDbContext _db;

    public AuthController(IConfiguration config, AppDbContext db)
    {
        _config = config;
        _db = db;
    }

    [HttpGet("me")]
    [Authorize]
    public IActionResult Me()
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        var name = User.FindFirst(ClaimTypes.Name)?.Value;

        return Ok(new
        {
            email,
            name
        });
    }

    [HttpGet("ping-google")]
    public async Task<IActionResult> PingGoogle()
    {
        using var client = new HttpClient();

        var urls = new[]
        {
        "https://www.googleapis.com/oauth2/v3/certs",
        "https://www.googleapis.com/oauth2/v1/certs",
        "https://oauth2.googleapis.com/certs"
    };

        var results = new List<object>();

        foreach (var url in urls)
        {
            try
            {
                var res = await client.GetAsync(url);

                results.Add(new
                {
                    url,
                    status = (int)res.StatusCode,
                    body = await res.Content.ReadAsStringAsync()
                });
            }
            catch (Exception ex)
            {
                results.Add(new
                {
                    url,
                    error = ex.Message
                });
            }
        }

        return Ok(results);
    }

    [HttpPost("google")]
    public async Task<IActionResult> GoogleLogin([FromBody] GoogleDto dto)
    {
        try
        {
            // Проверяем токен у Google
            var payload = await GoogleJsonWebSignature.ValidateAsync(dto.Token);

            var email = payload.Email;
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                user = new User
                {
                    Email = email,
                    Name = payload.Name
                };

                _db.Users.Add(user);
                await _db.SaveChangesAsync();
            }


            // Генерируем JWT для фронта
            var jwtSecret = _config["Jwt:Secret"];
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];
            var key = Encoding.UTF8.GetBytes(jwtSecret);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim("userId", user.Id.ToString()),
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Name, payload.Name ?? "")
                ]),
                Expires = DateTime.UtcNow.AddMonths(2),

                Issuer = issuer,
                Audience = audience,

                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);

            return Ok(new { user, token = jwt });
        }
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
    return BadRequest(ex.Message);
}
    }
}
