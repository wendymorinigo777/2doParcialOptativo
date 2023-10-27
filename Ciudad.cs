using OptativoSegundoParcial.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;
namespace OptativoSegundoParcial.Models
{
    public class Ciudad
    {
        public int id_Ciudad { get; set; }
        public string Ciudad_Nombre { get; set; }
        public string Departamento { get; set; }
        public int Codigo_Postal { get; set; }

    }


public static class CiudadEndpoints
{
	public static void MapCiudadEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Ciudad").WithTags(nameof(Ciudad));

        group.MapGet("/", async (AppBDDContext db) =>
        {
            return await db.Ciudades.ToListAsync();
        })
        .WithName("GetAllCiudads")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Ciudad>, NotFound>> (int id_Ciudad, AppBDDContext db) =>
        {
            return await db.Ciudades.AsNoTracking()
                .FirstOrDefaultAsync(model => model.id_Ciudad == id_Ciudad)
                is Ciudad model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetCiudadById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id_Ciudad, Ciudad ciudad, AppBDDContext db) =>
        {
            var affected = await db.Ciudades
                .Where(model => model.id_Ciudad == id_Ciudad)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.id_Ciudad, ciudad.id_Ciudad)
                  .SetProperty(m => m.Ciudad_Nombre, ciudad.Ciudad_Nombre)
                  .SetProperty(m => m.Departamento, ciudad.Departamento)
                  .SetProperty(m => m.Codigo_Postal, ciudad.Codigo_Postal)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateCiudad")
        .WithOpenApi();

        group.MapPost("/", async (Ciudad ciudad, AppBDDContext db) =>
        {
            db.Ciudades.Add(ciudad);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Ciudad/{ciudad.id_Ciudad}",ciudad);
        })
        .WithName("CreateCiudad")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id_Ciudad, AppBDDContext db) =>
        {
            var affected = await db.Ciudades
                .Where(model => model.id_Ciudad == id_Ciudad)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteCiudad")
        .WithOpenApi();
    }
}}

