var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "SetPasswordPage");

app.UseDefaultFiles(); 
app.UseStaticFiles(); 

app.Run();
