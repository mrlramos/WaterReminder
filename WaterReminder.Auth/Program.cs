using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using WaterReminder.Auth.Interfaces;
using WaterReminder.Auth.Services;

var builder = WebApplication.CreateBuilder(args);
// Adicionar servi�os ao cont�iner.

builder.Services.AddScoped<ITokenService, TokenService>();

// Configura��o de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Adicionar Controllers
builder.Services.AddControllers();

// Configura��o de Autentica��o JWT
var keyJWT = builder.Configuration.GetValue<string>("KeyJWT");
if (string.IsNullOrEmpty(keyJWT))
{
    throw new Exception("KeyJWT n�o configurado corretamente");
}

var key = Encoding.ASCII.GetBytes(keyJWT);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

// (Opcional) Adicionar Swagger para documenta��o de API
builder.Services.AddEndpointsApiExplorer();


var app = builder.Build();
// Configurar o pipeline de solicita��o HTTP.

// Redirecionamento HTTPS
app.UseHttpsRedirection();

// Roteamento
app.UseRouting();

// Aplicar pol�tica de CORS
app.UseCors("AllowAll");

// Autentica��o e Autoriza��o
app.UseAuthentication();
app.UseAuthorization();

// Mapeamento de Controllers
app.MapControllers();

// Executar a aplica��o
app.Run();
