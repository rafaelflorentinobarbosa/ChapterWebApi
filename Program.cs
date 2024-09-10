using Chapter.WebApi.Contexts;
using Chapter.WebApi.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;

var builder = WebApplication.CreateBuilder(args);

// Adiciona o contexto do banco de dados
builder.Services.AddScoped<ChapterContext, ChapterContext>();

// Adiciona os controladores
builder.Services.AddControllers();

// Configura o Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ChapterApi",
        Version = "v1"
    });
});

// Configura a autenticação JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "JwtBearer";
    options.DefaultChallengeScheme = "JwtBearer";
})
.AddJwtBearer("JwtBearer", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("chapterapi-chaveautenticacao")),
        ClockSkew = TimeSpan.FromMinutes(30),
        ValidIssuer = "chapterapi.webapi",
        ValidAudience = "chapterapi.webapi"
    };
});

// Adiciona os repositórios
builder.Services.AddTransient<LivroRepository, LivroRepository>();
builder.Services.AddTransient<UsuarioRepository, UsuarioRepository>();

var app = builder.Build();

// Configura o middleware de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Ativa o middleware para uso do Swagger
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ChapterApi v1"));

// Configura o middleware para redirecionamento HTTPS e arquivos estáticos
app.UseHttpsRedirection();
app.UseStaticFiles();

// Configura o roteamento
app.UseRouting();

// Habilita a autenticação e autorização
app.UseAuthentication();
app.UseAuthorization();

// Registra os endpoints usando rotas em nível superior
app.MapControllers();

app.Run();
