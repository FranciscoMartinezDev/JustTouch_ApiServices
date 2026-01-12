using JustTouch_ApiServices.Extentions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddJustTouchAuthentication(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddJustTouchServices(builder.Configuration);

builder.Services.AddJTCors(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseCors("Allowed");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
