using dynamic_twist_api.Services;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.OpenApi.Models;
using Westwind.AspNetCore.Markdown;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions()
{
    Args = args,
    WebRootPath = "wwwroot"
});

// Add services to the container.

builder.Services.AddMarkdown();

builder.Services.AddControllers()
    .AddApplicationPart(typeof(MarkdownPageProcessorMiddleware).Assembly);

builder.Services.AddScoped<IFileService, FileService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => 
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Dynamic Twist API",
        Version = "v1"
    });
});

builder.Services.AddCors();

var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.InjectStylesheet("https://cdnjs.cloudflare.com/ajax/libs/swagger-ui/4.1.3/swagger-ui.css");
        options.InjectJavascript("https://cdnjs.cloudflare.com/ajax/libs/swagger-ui/4.1.3/swagger-ui-bundle.js", "text/javascript");
        options.InjectJavascript("https://cdnjs.cloudflare.com/ajax/libs/swagger-ui/4.1.3/swagger-ui-standalone-preset.js", "text/javascript");
        options.SwaggerEndpoint($"v1/swagger.json", "Dynamic Twist API");
    });
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseMarkdown();
app.UseStaticFiles();
app.UseRouting();

app.UseCors(build =>
    build.WithOrigins(builder.Configuration["CORS:AllowedOrigins"])
         .WithHeaders(builder.Configuration["CORS:AllowedHeaders"])
         .WithMethods(builder.Configuration["CORS:AllowedMethods"])
);

app.UseAuthorization();

app.MapControllers();

app.Run();
