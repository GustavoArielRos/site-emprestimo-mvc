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

//registra o HttpContextAccessor como um singleton e permite que outras partes da aplicação possa usar o HttpContext através da interface IHttpContextAccessor
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


builder.Services.AddScoped<ILoginInterface, LoginService>();
builder.Services.AddScoped<ISenhaInterface, SenhaService>();
builder.Services.AddScoped<ISessaoInterface, SessaoService>();

//builder.Services.AddSession, A sessão permite armazenar dados especificos do usuário entre as solicitações HTTP
builder.Services.AddSession(options =>
{
    //define a propriedade HttpOnly do cookie da sessão como true, isso ajuda para proteção
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

//adicioanndo a sessao,permitindo que voce armazene e recupre dados de sessão entre solicitações HTTP
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}/{id?}");//modificando a tela principal para Login

app.Run();
