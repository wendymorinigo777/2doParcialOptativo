using OptativoSegundoParcial.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;
namespace OptativoSegundoParcial.Models
{
    public class Movimientos
    {
        public int id_Movimiento { get; set; }
        public int id_Cuenta { get; set; }
        public DateTime Fecha_Movimiento { get; set; }
        public string Tipo_de_Movimiento { get; set; }
        public decimal Saldo_Anterior { get; set; }
        public decimal Saldo_Actual { get; set; }
        public decimal Monto_Movimiento { get; set; }
        public decimal Cuenta_Origen { get; set; }
        public decimal Cuenta_Destino { get; set; }
        public decimal Canal { get; set; }

        // Relación con la tabla Cuentas
        public Cuentas Cuentas { get; set; }

    }


public static class MovimientosEndpoints
{
	public static void MapMovimientosEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Movimientos").WithTags(nameof(Movimientos));

        group.MapGet("/", async (AppBDDContext db) =>
        {
            return await db.Movimientos.ToListAsync();
        })
        .WithName("GetAllMovimientos")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Movimientos>, NotFound>> (int id_Movimiento, AppBDDContext db) =>
        {
            return await db.Movimientos.AsNoTracking()
                .FirstOrDefaultAsync(model => model.id_Movimiento == id_Movimiento)
                is Movimientos model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetMovimientosById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id_Movimiento, Movimientos movimientos, AppBDDContext db) =>
        {
            var affected = await db.Movimientos
                .Where(model => model.id_Movimiento == id_Movimiento)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.id_Movimiento, movimientos.id_Movimiento)
                  .SetProperty(m => m.id_Cuenta, movimientos.id_Cuenta)
                  .SetProperty(m => m.Fecha_Movimiento, movimientos.Fecha_Movimiento)
                  .SetProperty(m => m.Tipo_de_Movimiento, movimientos.Tipo_de_Movimiento)
                  .SetProperty(m => m.Saldo_Anterior, movimientos.Saldo_Anterior)
                  .SetProperty(m => m.Saldo_Actual, movimientos.Saldo_Actual)
                  .SetProperty(m => m.Monto_Movimiento, movimientos.Monto_Movimiento)
                  .SetProperty(m => m.Cuenta_Origen, movimientos.Cuenta_Origen)
                  .SetProperty(m => m.Cuenta_Destino, movimientos.Cuenta_Destino)
                  .SetProperty(m => m.Canal, movimientos.Canal)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateMovimientos")
        .WithOpenApi();

        group.MapPost("/", async (Movimientos movimientos, AppBDDContext db) =>
        {
            db.Movimientos.Add(movimientos);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Movimientos/{movimientos.id_Movimiento}",movimientos);
        })
        .WithName("CreateMovimientos")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id_Movimiento, AppBDDContext db) =>
        {
            var affected = await db.Movimientos
                .Where(model => model.id_Movimiento == id_Movimiento)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteMovimientos")
        .WithOpenApi();
    }
}}
