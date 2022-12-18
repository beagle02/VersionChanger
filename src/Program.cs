using System.Reflection;
using Microsoft.Extensions.Configuration;
using VersionChanger;
using VersionChanger.Models;
using VersionChanger.Validators;

const string fileMask = "*.csproj";

var justShowResult = args.Contains("--show");

if (!args.Contains("replace"))
{
    Console.WriteLine("Usage: VersionChanger.exe replace [--show]");
    return;
}

var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
if (!Directory.Exists(basePath))
{
    Console.WriteLine($"Path '{basePath}' not found.");
    return;
}

var builder = new ConfigurationBuilder()
    .SetBasePath(basePath)
    .AddJsonFile("appsettings.json", optional: false);

var config = builder.Build();
var settings = config.GetRequiredSection(nameof(Settings)).Get<Settings>();

var validator = new SettingsValidator();
var result = validator.Validate(settings);
if (!result.IsValid)
{
    Console.WriteLine($"Settings validation error:\n{result}");
    return;
}

Console.WriteLine(settings);
Console.WriteLine($"Looking for files {fileMask} ...");
var files = Directory.GetFiles(settings.Path, fileMask, SearchOption.AllDirectories);

var updater = new FileUpdater(settings, Console.WriteLine, justShowResult);

foreach (var file in files)
    updater.Update(file);
