using Dental_clinic_WebApi.Data;
using Dental_clinic_WebApi.Module;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;

public class Program 
{
    public static void Main(string[] args) 
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<PatientDb>();
        builder.Services.AddDbContext<SpecializationDoctorDb>();
        builder.Services.AddDbContext<DoctorsDb>();
        builder.Services.AddDbContext<RecordOnReceivingDb>();
        builder.Services.AddDbContext<DestinationDb>();
        builder.Services.AddDbContext<ServicesDb>();
        builder.Services.AddDbContext<ListOfDiseasesDb>();
        builder.Services.AddDbContext<PatientCardDb>();
        builder.Services.AddDbContext<ReceivingTypeDb>();
        /*(opt =>
        {
            opt.UseSqlServer(builder.Configuration.GetConnectionString("Dental_clinic"));
        });*/

        //builder.Services.AddScoped<IPatientRepository, PatientRepository>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            /*using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<PatientsDb>();
            db.Database.EnsureCreated();*/
        }

        app.MapEntityEndpoints<Patients, PatientDb>("/patients"); 
        app.MapEntityEndpoints<Doctors, DoctorsDb>("/doctors");
        app.MapEntityEndpoints<Specialization_doctor, SpecializationDoctorDb>("/specializ_doctor");
        app.MapEntityEndpoints<Records_on_receiving, RecordOnReceivingDb>("/recordonreceiv");
        app.MapEntityEndpoints<Destination, DestinationDb>("/destination");
        app.MapEntityEndpoints<Services, ServicesDb>("/services");
        app.MapEntityEndpoints<List_of_diseases, ListOfDiseasesDb>("/listdiseases");
        app.MapEntityEndpoints<Patient_card, PatientCardDb>("/patientcard");
        app.MapEntityEndpoints<Receiving_type, ReceivingTypeDb>("/receivingtype");
        app.Run();
    }
}

public static class EntityEndpoints 
{
    public static void MapEntityEndpoints<TEntity, TDbContext>(this WebApplication app, string routePrefix)
        where TEntity : class, IEntity
        where TDbContext : DbContext
    {
        app.MapGet(routePrefix, async (TDbContext dbContext) =>
        {
            var entities = await dbContext.Set<TEntity>().ToListAsync();
            return entities;
        });

        app.MapGet($"{routePrefix}/{{id}}", async (int id, TDbContext dbContext) =>
            {
                var entity = await dbContext.Set<TEntity>().FindAsync(id);
                if(entity == null) return Results.NotFound();
                return Results.Ok(entity);
            });

        app.MapPost(routePrefix, async (TDbContext dbContext, TEntity entity) =>
        {
            dbContext.Set<TEntity>().Add(entity);
            await dbContext.SaveChangesAsync();
            return Results.Created($"/{routePrefix}/{entity.Id}", entity);
        });

        app.MapPut($"{routePrefix}/{{id}}", async (int id, TDbContext dbContext, TEntity entit) =>
        {
            var existingEntity = await dbContext.Set<TEntity>().FindAsync(id);
            if (existingEntity == null) return Results.NotFound();
            dbContext.Entry(existingEntity).CurrentValues.SetValues(existingEntity);
            await dbContext.SaveChangesAsync();
            return Results.Ok(entit);
        });

        app.MapDelete($"{routePrefix}/{{id}}", async (int id, TDbContext dbContext) =>
        {
            if (await dbContext.Set<TEntity>().FindAsync(id) is TEntity entity) 
            {
                dbContext.Set<TEntity>().Remove(entity);
                await dbContext.SaveChangesAsync();
                return Results.NoContent();
            }
            return Results.NotFound();
        });

        /*
            app.MapGet("/hotels/search/name/{query}",
            async (string query, IPatientRepository repository) =>
                await repository.GetPatientsAsync(query) is IEnumerable<Patients> patients
                    ? Results.Ok(patients)
                    : Results.NotFound(Array.Empty<Patients>()))
                    .Produces<List<Patients>>(StatusCodes.Status200OK)
                    .Produces(StatusCodes.Status404NotFound)
                    .WithName("SearchPatients")
                    .WithTags("Getters")
                    .ExcludeFromDescription();
        */

        app.UseHttpsRedirection();

    }
}

