using JustTouch_ApiServices.Extentions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddJustTouchAuthentication(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddJustTouchServices(builder.Configuration);


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
