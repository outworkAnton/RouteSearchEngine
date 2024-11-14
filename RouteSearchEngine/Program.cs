using Microsoft.Extensions.DependencyInjection.Extensions;
using RouteSearchEngine.Abstractions;
using RouteSearchEngine.Repositories;
using RouteSearchEngine.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers().AddControllersAsServices();
builder.Services.AddTransient<ISearchService, SearchService>();
builder.Services.AddSingleton<ICacheService, CacheService>();
builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<ISearchRepository, ProviderOneSearchRepository>());
builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<ISearchRepository, ProviderTwoSearchRepository>());
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();