using FitFlow.Application.Clients;
using FitFlow.Application.Memberships;
using FitFlow.Application.Sections;
using FitFlow.Application.Trainers;
using FitFlow.Application.Visits;
using FitFlow.Application.Payments;
using FitFlow.Application.TrainingSessions;
using FitFlow.Infrastructure.Persistence;
using FitFlow.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace FitFlow.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<FitFlowDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IClientService, ClientService>();
        services.AddScoped<ITrainerService, TrainerService>();
        services.AddScoped<ISectionService, SectionService>();
        services.AddScoped<IMembershipService, MembershipService>();
        services.AddScoped<IVisitService, VisitService>();
        services.AddScoped<ITrainingSessionService, TrainingSessionService>();
        services.AddScoped<IPaymentService, PaymentService>();
        return services;
    }
}