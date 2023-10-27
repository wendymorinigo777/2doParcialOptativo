using OptativoSegundoParcial.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;
namespace OptativoSegundoParcial.Models
{
    public class Cliente
    {
        public int id_Cliente { get; set; }
        public int id_Persona { get; set; }
        public DateTime Fecha_Ingresada { get; set; }
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

        group.MapGet("/", async (AppBDDContext db) =>
        {
            return await db.Clientes.ToListAsync();
        })
        .WithName("GetAllClientes")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Cliente>, NotFound>> (int id_Cliente, AppBDDContext db) =>
        {
            return await db.Clientes.AsNoTracking()
                .FirstOrDefaultAsync(model => model.id_Cliente == id_Cliente)
                is Cliente model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetClienteById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id_Cliente, Cliente cliente, AppBDDContext db) =>
        {
            var affected = await db.Clientes
                .Where(model => model.id_Cliente == id_Cliente)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.id_Cliente, cliente.id_Cliente)
                  .SetProperty(m => m.id_Persona, cliente.id_Persona)
                  .SetProperty(m => m.Fecha_Ingresada, cliente.Fecha_Ingresada)
                  .SetProperty(m => m.Calificacion, cliente.Calificacion)
                  .SetProperty(m => m.Estado, cliente.Estado)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateCliente")
        .WithOpenApi();

        group.MapPost("/", async (Cliente cliente, AppBDDContext db) =>
        {
            db.Clientes.Add(cliente);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Cliente/{cliente.id_Cliente}",cliente);
        })
        .WithName("CreateCliente")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id_Cliente, AppBDDContext db) =>
        {
            var affected = await db.Clientes
                .Where(model => model.id_Cliente == id_Cliente)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteCliente")
        .WithOpenApi();
    }
}}

