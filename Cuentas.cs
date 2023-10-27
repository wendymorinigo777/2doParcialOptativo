using OptativoSegundoParcial.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;
namespace OptativoSegundoParcial.Models
{
    public class Cuentas
    {
        public int id_Cuenta { get; set; }
        public int id_Cliente { get; set; }
        public string Nro_de_Cuenta { get; set; }
        public DateTime Fecha_de_Alta { get; set; }
        public string Tipo_de_Cuenta { get; set; }
        public string Estado { get; set; }
        public decimal Saldo { get; set; }
        public string Nro_Contrato { get; set; }
        public decimal Costo_de_Mantenimiento { get; set; }
        public string Promedio_de_Acreditacion { get; set; }
        public string Moneda { get; set; }

        // Relación con la tabla Cliente
        public Cliente Cliente { get; set; }

    }


public static class CuentasEndpoints
{
	public static void MapCuentasEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Cuentas").WithTags(nameof(Cuentas));

        group.MapGet("/", async (AppBDDContext db) =>
        {
            return await db.Cuentas.ToListAsync();
        })
        .WithName("GetAllCuentas")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Cuentas>, NotFound>> (int id_Cuenta, AppBDDContext db) =>
        {
            return await db.Cuentas.AsNoTracking()
                .FirstOrDefaultAsync(model => model.id_Cuenta == id_Cuenta)
                is Cuentas model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetCuentasById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id_Cuenta, Cuentas cuentas, AppBDDContext db) =>
        {
            var affected = await db.Cuentas
                .Where(model => model.id_Cuenta == id_Cuenta)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.id_Cuenta, cuentas.id_Cuenta)
                  .SetProperty(m => m.id_Cliente, cuentas.id_Cliente)
                  .SetProperty(m => m.Nro_de_Cuenta, cuentas.Nro_de_Cuenta)
                  .SetProperty(m => m.Fecha_de_Alta, cuentas.Fecha_de_Alta)
                  .SetProperty(m => m.Tipo_de_Cuenta, cuentas.Tipo_de_Cuenta)
                  .SetProperty(m => m.Estado, cuentas.Estado)
                  .SetProperty(m => m.Saldo, cuentas.Saldo)
                  .SetProperty(m => m.Nro_Contrato, cuentas.Nro_Contrato)
                  .SetProperty(m => m.Costo_de_Mantenimiento, cuentas.Costo_de_Mantenimiento)
                  .SetProperty(m => m.Promedio_de_Acreditacion, cuentas.Promedio_de_Acreditacion)
                  .SetProperty(m => m.Moneda, cuentas.Moneda)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateCuentas")
        .WithOpenApi();

        group.MapPost("/", async (Cuentas cuentas, AppBDDContext db) =>
        {
            db.Cuentas.Add(cuentas);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Cuentas/{cuentas.id_Cuenta}",cuentas);
        })
        .WithName("CreateCuentas")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id_Cuenta, AppBDDContext db) =>
        {
            var affected = await db.Cuentas
                .Where(model => model.id_Cuenta == id_Cuenta)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteCuentas")
        .WithOpenApi();
    }
}}

