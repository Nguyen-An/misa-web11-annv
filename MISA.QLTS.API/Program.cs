using MISA.QLTS.BL;
using MISA.QLTS.BL.AssetCategoryBL;
using MISA.QLTS.DL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped(typeof(IBaseDL<>), typeof(BaseDL<>));
builder.Services.AddScoped(typeof(IBaseBL<>), typeof(BaseBL<>));

builder.Services.AddScoped<IAssetDL, AssetDL>();
builder.Services.AddScoped<IAssetBL, AssetBL>();

builder.Services.AddScoped<IAssetCategoryDL, AssetCategoryDL>();
builder.Services.AddScoped<IAssetCategoryBL, AssetCategoryBL>();

builder.Services.AddScoped<IDepartmentDL, DepartmentDL>(); 
builder.Services.AddScoped<IDepartmentBL, DepartmentBL>();



builder.Services.AddControllers().ConfigureApiBehaviorOptions(options => 
    options.SuppressModelStateInvalidFilter = true
);

DatabaseContext.ConnectionString = builder.Configuration.GetConnectionString("MySql");

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(p => p.AddPolicy("MyCors", build =>
{
    //build.WithOrigins("http://localhost:8080");
    build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.UseCors("MyCors");

app.MapControllers();
    
app.Run();
