
Nombre: Persona.cs
using OptativoSegundoParcial.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;
namespace OptativoSegundoParcial.Models
{
    public class Persona
    {
        public int id_Persona { get; set; }
        public int id_Ciudad { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Tipo_de_Documento { get; set; }
        public string Nro_Documento { get; set; }
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

        group.MapGet("/", async (AppBDDContext db) =>
        {
            return await db.Personas.ToListAsync();
        })
        .WithName("GetAllPersonas")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Persona>, NotFound>> (int id_Persona, AppBDDContext db) =>
        {
            return await db.Personas.AsNoTracking()
                .FirstOrDefaultAsync(model => model.id_Persona == id_Persona)
                is Persona model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetPersonaById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id_Persona, Persona persona, AppBDDContext db) =>
        {
            var affected = await db.Personas
                .Where(model => model.id_Persona == id_Persona)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.id_Persona, persona.id_Persona)
                  .SetProperty(m => m.id_Ciudad, persona.id_Ciudad)
                  .SetProperty(m => m.Nombre, persona.Nombre)
                  .SetProperty(m => m.Apellido, persona.Apellido)
                  .SetProperty(m => m.Tipo_de_Documento, persona.Tipo_de_Documento)
                  .SetProperty(m => m.Nro_Documento, persona.Nro_Documento)
                  .SetProperty(m => m.Direccion, persona.Direccion)
                  .SetProperty(m => m.Telefono, persona.Telefono)
                  .SetProperty(m => m.Email, persona.Email)
                  .SetProperty(m => m.Estado, persona.Estado)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdatePersona")
        .WithOpenApi();

        group.MapPost("/", async (Persona persona, AppBDDContext db) =>
        {
            db.Personas.Add(persona);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Persona/{persona.id_Persona}",persona);
        })
        .WithName("CreatePersona")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id_Persona, AppBDDContext db) =>
        {
            var affected = await db.Personas
                .Where(model => model.id_Persona == id_Persona)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeletePersona")
        .WithOpenApi();
    }
}}
