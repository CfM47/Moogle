using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MoogleEngine;
using System.Runtime.CompilerServices;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

//Inyectar el objeto que tiene el precalculo a mi servidor
//es posible que haya que cambiar la direccion de la base de datos
string databasePath = "../Content";
builder.Services.AddSingleton(s => BuildTfIdfDirectory(databasePath));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}



app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

static TfIdfDirectory BuildTfIdfDirectory(string databasePath)
{
    //este metodo decide como construir el objeto tfIdf
    var parent = Directory.GetParent(databasePath) ?? throw new DirectoryNotFoundException("No se encontro el directorio de la base de datos");
    string mgl = Path.Combine(parent.FullName, "DatabaseInfo.json");
    TfIdfDirectory tf;
    if (File.Exists(mgl) && new DirectoryInfo(mgl).LastWriteTime > new DirectoryInfo(databasePath).LastWriteTime)
    {
        string jsonString = File.ReadAllText(mgl);
        return tf = JsonSerializer.Deserialize<TfIdfDirectory>(jsonString) ?? throw new Exception("No se pudo deserializar el archivo de la base de datos");

    }
    return tf = new TfIdfDirectory(databasePath);
}




