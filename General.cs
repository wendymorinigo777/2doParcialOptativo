Nombre: AppDbContext.cs
using Microsoft.EntityFrameworkCore;

namespace OPTATIVOIII3ERPARCIAL.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Ciudad> Ciudades { get; set; }
        public DbSet<Persona> Personas { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Cuentas> Cuentas { get; set; }
        public DbSet<Movimientos> Movimientos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ciudad>()
                .HasKey(c => c.idCiudad);

            modelBuilder.Entity<Persona>()
                .HasKey(p => p.idPersona);

            modelBuilder.Entity<Cliente>()
                .HasKey(c => c.idCliente);

            modelBuilder.Entity<Cuentas>()
                .HasKey(c => c.idCuenta);

            modelBuilder.Entity<Movimientos>()
                .HasKey(m => m.idMovimiento);
        }
    }

}
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

Nombre: Ciudad.cs
using OPTATIVOIII3ERPARCIAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;
namespace OPTATIVOIII3ERPARCIAL.Models
{
    public class Ciudad
    {
        public int idCiudad { get; set; }
        public string CiudadNombre { get; set; }
        public string Departamento { get; set; }
        public int PostalCode { get; set; }

    }


public static class CiudadEndpoints
{
	public static void MapCiudadEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Ciudad").WithTags(nameof(Ciudad));

        group.MapGet("/", async (AppDbContext db) =>
        {
            return await db.Ciudades.ToListAsync();
        })
        .WithName("GetAllCiudads")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Ciudad>, NotFound>> (int idciudad, AppDbContext db) =>
        {
            return await db.Ciudades.AsNoTracking()
                .FirstOrDefaultAsync(model => model.idCiudad == idciudad)
                is Ciudad model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetCiudadById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int idciudad, Ciudad ciudad, AppDbContext db) =>
        {
            var affected = await db.Ciudades
                .Where(model => model.idCiudad == idciudad)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.idCiudad, ciudad.idCiudad)
                  .SetProperty(m => m.CiudadNombre, ciudad.CiudadNombre)
                  .SetProperty(m => m.Departamento, ciudad.Departamento)
                  .SetProperty(m => m.PostalCode, ciudad.PostalCode)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateCiudad")
        .WithOpenApi();

        group.MapPost("/", async (Ciudad ciudad, AppDbContext db) =>
        {
            db.Ciudades.Add(ciudad);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Ciudad/{ciudad.idCiudad}",ciudad);
        })
        .WithName("CreateCiudad")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int idciudad, AppDbContext db) =>
        {
            var affected = await db.Ciudades
                .Where(model => model.idCiudad == idciudad)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteCiudad")
        .WithOpenApi();
    }
}}


/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

Nombre: Cliente.cs
using OPTATIVOIII3ERPARCIAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;
namespace OPTATIVOIII3ERPARCIAL.Models
{
    public class Cliente
    {
        public int idCliente { get; set; }
        public int idPersona { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string Calificacion { get; set; }
        public string Estado { get; set; }

        // Relación con la tabla Persona
        public Persona Persona { get; set; }

    }


public static class ClienteEndpoints
{
	public static void MapClienteEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Cliente").WithTags(nameof(Cliente));

        group.MapGet("/", async (AppDbContext db) =>
        {
            return await db.Clientes.ToListAsync();
        })
        .WithName("GetAllClientes")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Cliente>, NotFound>> (int idcliente, AppDbContext db) =>
        {
            return await db.Clientes.AsNoTracking()
                .FirstOrDefaultAsync(model => model.idCliente == idcliente)
                is Cliente model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetClienteById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int idcliente, Cliente cliente, AppDbContext db) =>
        {
            var affected = await db.Clientes
                .Where(model => model.idCliente == idcliente)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.idCliente, cliente.idCliente)
                  .SetProperty(m => m.idPersona, cliente.idPersona)
                  .SetProperty(m => m.FechaIngreso, cliente.FechaIngreso)
                  .SetProperty(m => m.Calificacion, cliente.Calificacion)
                  .SetProperty(m => m.Estado, cliente.Estado)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateCliente")
        .WithOpenApi();

        group.MapPost("/", async (Cliente cliente, AppDbContext db) =>
        {
            db.Clientes.Add(cliente);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Cliente/{cliente.idCliente}",cliente);
        })
        .WithName("CreateCliente")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int idcliente, AppDbContext db) =>
        {
            var affected = await db.Clientes
                .Where(model => model.idCliente == idcliente)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteCliente")
        .WithOpenApi();
    }
}}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



Nombre: Cuentas.cs
using OPTATIVOIII3ERPARCIAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;
namespace OPTATIVOIII3ERPARCIAL.Models
{
    public class Cuentas
    {
        public int idCuenta { get; set; }
        public int idCliente { get; set; }
        public string NroCuenta { get; set; }
        public DateTime FechaAlta { get; set; }
        public string TipoCuenta { get; set; }
        public string Estado { get; set; }
        public decimal Saldo { get; set; }
        public string Nro_Contrato { get; set; }
        public decimal CostoMantenimiento { get; set; }
        public string PromedioAcreditacion { get; set; }
        public string Moneda { get; set; }

        // Relación con la tabla Cliente
        public Cliente Cliente { get; set; }

    }


public static class CuentasEndpoints
{
	public static void MapCuentasEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Cuentas").WithTags(nameof(Cuentas));

        group.MapGet("/", async (AppDbContext db) =>
        {
            return await db.Cuentas.ToListAsync();
        })
        .WithName("GetAllCuentas")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Cuentas>, NotFound>> (int idcuenta, AppDbContext db) =>
        {
            return await db.Cuentas.AsNoTracking()
                .FirstOrDefaultAsync(model => model.idCuenta == idcuenta)
                is Cuentas model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetCuentasById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int idcuenta, Cuentas cuentas, AppDbContext db) =>
        {
            var affected = await db.Cuentas
                .Where(model => model.idCuenta == idcuenta)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.idCuenta, cuentas.idCuenta)
                  .SetProperty(m => m.idCliente, cuentas.idCliente)
                  .SetProperty(m => m.NroCuenta, cuentas.NroCuenta)
                  .SetProperty(m => m.FechaAlta, cuentas.FechaAlta)
                  .SetProperty(m => m.TipoCuenta, cuentas.TipoCuenta)
                  .SetProperty(m => m.Estado, cuentas.Estado)
                  .SetProperty(m => m.Saldo, cuentas.Saldo)
                  .SetProperty(m => m.Nro_Contrato, cuentas.Nro_Contrato)
                  .SetProperty(m => m.CostoMantenimiento, cuentas.CostoMantenimiento)
                  .SetProperty(m => m.PromedioAcreditacion, cuentas.PromedioAcreditacion)
                  .SetProperty(m => m.Moneda, cuentas.Moneda)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateCuentas")
        .WithOpenApi();

        group.MapPost("/", async (Cuentas cuentas, AppDbContext db) =>
        {
            db.Cuentas.Add(cuentas);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Cuentas/{cuentas.idCuenta}",cuentas);
        })
        .WithName("CreateCuentas")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int idcuenta, AppDbContext db) =>
        {
            var affected = await db.Cuentas
                .Where(model => model.idCuenta == idcuenta)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteCuentas")
        .WithOpenApi();
    }
}}



////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


Nombre: Movimientos.cs
using OPTATIVOIII3ERPARCIAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;
namespace OPTATIVOIII3ERPARCIAL.Models
{
    public class Movimientos
    {
        public int idMovimiento { get; set; }
        public int idCuenta { get; set; }
        public DateTime Fecha_Movimiento { get; set; }
        public string TipoMovimiento { get; set; }
        public decimal SaldoAnterior { get; set; }
        public decimal SaldoActual { get; set; }
        public decimal MontoMovimiento { get; set; }
        public decimal CuentaOrigen { get; set; }
        public decimal CuentaDestino { get; set; }
        public decimal Canal { get; set; }

        // Relación con la tabla Cuentas
        public Cuentas Cuentas { get; set; }

    }


public static class MovimientosEndpoints
{
	public static void MapMovimientosEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Movimientos").WithTags(nameof(Movimientos));

        group.MapGet("/", async (AppDbContext db) =>
        {
            return await db.Movimientos.ToListAsync();
        })
        .WithName("GetAllMovimientos")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Movimientos>, NotFound>> (int idmovimiento, AppDbContext db) =>
        {
            return await db.Movimientos.AsNoTracking()
                .FirstOrDefaultAsync(model => model.idMovimiento == idmovimiento)
                is Movimientos model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetMovimientosById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int idmovimiento, Movimientos movimientos, AppDbContext db) =>
        {
            var affected = await db.Movimientos
                .Where(model => model.idMovimiento == idmovimiento)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.idMovimiento, movimientos.idMovimiento)
                  .SetProperty(m => m.idCuenta, movimientos.idCuenta)
                  .SetProperty(m => m.Fecha_Movimiento, movimientos.Fecha_Movimiento)
                  .SetProperty(m => m.TipoMovimiento, movimientos.TipoMovimiento)
                  .SetProperty(m => m.SaldoAnterior, movimientos.SaldoAnterior)
                  .SetProperty(m => m.SaldoActual, movimientos.SaldoActual)
                  .SetProperty(m => m.MontoMovimiento, movimientos.MontoMovimiento)
                  .SetProperty(m => m.CuentaOrigen, movimientos.CuentaOrigen)
                  .SetProperty(m => m.CuentaDestino, movimientos.CuentaDestino)
                  .SetProperty(m => m.Canal, movimientos.Canal)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateMovimientos")
        .WithOpenApi();

        group.MapPost("/", async (Movimientos movimientos, AppDbContext db) =>
        {
            db.Movimientos.Add(movimientos);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Movimientos/{movimientos.idMovimiento}",movimientos);
        })
        .WithName("CreateMovimientos")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int idmovimiento, AppDbContext db) =>
        {
            var affected = await db.Movimientos
                .Where(model => model.idMovimiento == idmovimiento)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteMovimientos")
        .WithOpenApi();
    }
}}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



Nombre: Persona.cs
using OPTATIVOIII3ERPARCIAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;
namespace OPTATIVOIII3ERPARCIAL.Models
{
    public class Persona
    {
        public int idPersona { get; set; }
        public int idCiudad { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string TipoDocumento { get; set; }
        public string NroDocumento { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Estado { get; set; }

        // Relación con la tabla Ciudad
        public Ciudad Ciudad { get; set; }

    }


public static class PersonaEndpoints
{
	public static void MapPersonaEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Persona").WithTags(nameof(Persona));

        group.MapGet("/", async (AppDbContext db) =>
        {
            return await db.Personas.ToListAsync();
        })
        .WithName("GetAllPersonas")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Persona>, NotFound>> (int idpersona, AppDbContext db) =>
        {
            return await db.Personas.AsNoTracking()
                .FirstOrDefaultAsync(model => model.idPersona == idpersona)
                is Persona model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetPersonaById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int idpersona, Persona persona, AppDbContext db) =>
        {
            var affected = await db.Personas
                .Where(model => model.idPersona == idpersona)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.idPersona, persona.idPersona)
                  .SetProperty(m => m.idCiudad, persona.idCiudad)
                  .SetProperty(m => m.Nombre, persona.Nombre)
                  .SetProperty(m => m.Apellido, persona.Apellido)
                  .SetProperty(m => m.TipoDocumento, persona.TipoDocumento)
                  .SetProperty(m => m.NroDocumento, persona.NroDocumento)
                  .SetProperty(m => m.Direccion, persona.Direccion)
                  .SetProperty(m => m.Telefono, persona.Telefono)
                  .SetProperty(m => m.Email, persona.Email)
                  .SetProperty(m => m.Estado, persona.Estado)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdatePersona")
        .WithOpenApi();

        group.MapPost("/", async (Persona persona, AppDbContext db) =>
        {
            db.Personas.Add(persona);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Persona/{persona.idPersona}",persona);
        })
        .WithName("CreatePersona")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int idpersona, AppDbContext db) =>
        {
            var affected = await db.Personas
                .Where(model => model.idPersona == idpersona)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeletePersona")
        .WithOpenApi();
    }
}}


////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


Nombre: appsettings.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=.\\SQLEXPRESS;Initial Catalog=OPTATIVO;Integrated Security=True"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

Nombre: Program.cs
using Microsoft.EntityFrameworkCore;
using OPTATIVOIII3ERPARCIAL.Models;
using OPTATIVOIII3ERPARCIAL.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapCiudadEndpoints();

app.MapClienteEndpoints();

app.MapCuentasEndpoints();

app.MapMovimientosEndpoints();

app.Run();





