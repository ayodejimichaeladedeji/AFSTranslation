using AFSTranslator.Services;
using AFSTranslator.Repository;
using Microsoft.Data.SqlClient;
using AFSTranslator.Interfaces.Services;
using AFSTranslator.Interfaces.Repository;
using AFSTranslator.Interfaces.Factory;
using AFSTranslator.Factory;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication("MyCookieAuth")
    .AddCookie("MyCookieAuth", options =>
    {
        options.Cookie.Name = "AFSTranslatorAuth";
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/Login";
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
    });

// builder.Services.AddAuthentication("Bearer")
//     .AddJwtBearer(options =>
//     {
//         options.Authority = builder.Configuration["Jwt:Issuer"];
//         options.Audience = builder.Configuration["Jwt:Audience"];
//         options.RequireHttpsMetadata = false;
//         options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
//             {
//                 ValidateIssuer = true,
//                 ValidateAudience = true,
//                 ValidateLifetime = true,
//                 ClockSkew = TimeSpan.Zero
//             };
//     });

builder.Services.AddControllersWithViews();

builder.Services.AddScoped((s) => new SqlConnection(builder.Configuration.GetConnectionString("AFSConnectionString")));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITokenService, JWTService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IRestService, RestService>();
builder.Services.AddTransient<ITranslatorService, FunTranslationService>();
builder.Services.AddScoped<ITranslatorFactory, TranslatorFactory>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseStatusCodePages(context =>
{
    if (context.HttpContext.Response.StatusCode == 401)
    {
        context.HttpContext.Response.Redirect("/Auth/Login");
    }
    return Task.CompletedTask;
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/");

app.Run();
