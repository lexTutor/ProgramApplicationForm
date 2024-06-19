using Application.Infrastructure.Data;
using Application.Infrastructure.Filters;
using Application.Infrastructure.Logger;
using Application.Infrastructure.Services;
using Application.Infrastructure.Services.QuestionTypeManagers;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System.Reflection;
using static Application.Core.Enums;
using ILogger = Serilog.ILogger;

namespace Application.Infrastructure;

public static class InfrastructureInjection
{
    public static void AddInfrastructure(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton((serviceProvider) =>
        {
            return new CosmosClient(builder.Configuration.GetConnectionString("CosmosDbConnectionString"));
        });

        builder.Services.AddScoped(typeof(ICosmosDbRepository<>), typeof(CosmosDbRepository<>));
        builder.Services.AddScoped(typeof(ILogManager<>), typeof(LogManager<>));
        builder.Services.AddScoped(typeof(ValidationFilter<>));

        builder.Services.AddScoped<IProgramFormService, ProgramFormService>();
        builder.Services.AddKeyedSingleton<IQuestionTypeManager, MultiChoiceQuestionManager>(QuestionType.MultipleChoice);
        builder.Services.AddKeyedSingleton<IQuestionTypeManager, DropdownQuestionManager>(QuestionType.Dropdown);
        builder.Services.AddKeyedSingleton<IQuestionTypeManager, DateQuestionManager>(QuestionType.Date);
        builder.Services.AddKeyedSingleton<IQuestionTypeManager, NumberQuestionManager>(QuestionType.Number);
        builder.Services.AddKeyedSingleton<IQuestionTypeManager, ParagraphQuestionManager>(QuestionType.Paragraph);
        builder.Services.AddKeyedSingleton<IQuestionTypeManager, YesNoQuestionManager>(QuestionType.YesNo);
        builder.Services.AddSingleton<Func<QuestionType, IQuestionTypeManager>>(serviceProvider => key =>
        {
            return serviceProvider.GetKeyedService<IQuestionTypeManager>(key) ?? throw new ArgumentNullException(key.ToString());
        });

        builder.Services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public static void AddApplicationLogging(this ILoggingBuilder loggingBuilder)
    {
        loggingBuilder.ClearProviders();

        // Create the logger configuration
        ILogger logger = new LoggerConfiguration()
            .WriteTo.File("app-logs", rollingInterval: RollingInterval.Day)
            .MinimumLevel.Override("warning", LogEventLevel.Information)
            .CreateLogger();

        Log.Logger = logger;
        loggingBuilder.AddSerilog();
    }
}
