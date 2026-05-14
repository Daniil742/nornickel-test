using Microsoft.OpenApi;
using Nornickel.Api.WebApplicationBuildersExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "HR System API", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header. Введите токен.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    //options.AddSecurityRequirement(new OpenApiSecurityRequirement
    //{
    //    {
    //        new OpenApiSecurityScheme
    //        {
    //            Reference = new OpenApiReference
    //            {
    //                Type = ReferenceType.SecurityScheme,
    //                Id = "Bearer"
    //            }
    //        },
    //        Array.Empty<string>()
    //    }
    //});
});
builder.Services.AddServiceRegister(builder.Configuration);

builder.Services.AddCors(o => o.AddPolicy("RequestPolicy", config =>
{
    config
        .WithOrigins("http://localhost:50689")
        //.WithMethods("GET", "POST", "PUT", "DELETE")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
}));

builder.Services.AddHealthChecks();
builder.Services.AddDbContext(builder.Configuration);

var app = builder.Build();

app.MapHealthChecks("/healthz").RequireHost("localhost");

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler();
    app.UseHsts();
}

app.DbMigrateHrSystem();

app.UseStaticFiles();
app.UseDefaultFiles();

//app.UseHttpsRedirection();
app.UseCors("RequestPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints();

app.Run();
