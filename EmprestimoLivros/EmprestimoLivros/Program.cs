using EmprestimoLivros.Data;
using EmprestimoLivros.Services.LoginService;
using EmprestimoLivros.Services.SenhaService;
using EmprestimoLivros.Services.SessaoService;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//adicionando o dbcontext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//registra o HttpContextAccessor como um singleton e permite que outras partes da aplica��o possa usar o HttpContext atrav�s da interface IHttpContextAccessor
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


builder.Services.AddScoped<ILoginInterface, LoginService>();
builder.Services.AddScoped<ISenhaInterface, SenhaService>();
builder.Services.AddScoped<ISessaoInterface, SessaoService>();

//builder.Services.AddSession, A sess�o permite armazenar dados especificos do usu�rio entre as solicita��es HTTP
builder.Services.AddSession(options =>
{
    //define a propriedade HttpOnly do cookie da sess�o como true, isso ajuda para prote��o
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

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

app.UseAuthorization();

//adicioanndo a sessao,permitindo que voce armazene e recupre dados de sess�o entre solicita��es HTTP
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}/{id?}");//modificando a tela principal para Login

app.Run();
