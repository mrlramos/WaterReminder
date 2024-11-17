using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using WaterReminder.Auth.Interfaces;
using WaterReminder.Auth.Services;

var builder = WebApplication.CreateBuilder(args);
// Adicionar serviços ao contêiner.

builder.Services.AddScoped<ITokenService, TokenService>();

// Configuração de CORS
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

// Configuração de Autenticação JWT
var keyJWT = builder.Configuration.GetValue<string>("KeyJWT");
if (string.IsNullOrEmpty(keyJWT))
{
    throw new Exception("KeyJWT não configurado corretamente");
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

// (Opcional) Adicionar Swagger para documentação de API
builder.Services.AddEndpointsApiExplorer();


var app = builder.Build();
// Configurar o pipeline de solicitação HTTP.

// Redirecionamento HTTPS
app.UseHttpsRedirection();

// Roteamento
app.UseRouting();

// Aplicar política de CORS
app.UseCors("AllowAll");

// Autenticação e Autorização
app.UseAuthentication();
app.UseAuthorization();

// Mapeamento de Controllers
app.MapControllers();

// Executar a aplicação
app.Run();
