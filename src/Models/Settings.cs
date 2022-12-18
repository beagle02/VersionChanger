namespace VersionChanger.Models;

public class Settings
{
    /// <summary>
    /// Путь поиска проектных файлов
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// Список пакетов на изменение
    /// </summary>
    public Package[] Packages { get; set; }

    public override string ToString()
    {
        return Packages.Aggregate("Configuration:\n",
            (current, pkg) =>
            {
                var msg1 = pkg.IsFrameworkProject ? $", '{pkg.FileVersion}'" : string.Empty;
                var msg2 = pkg.IsFrameworkProject && !string.IsNullOrWhiteSpace(pkg.IgnoreFileVersion)
                    ? $", '{pkg.IgnoreFileVersion}'"
                    : string.Empty;
                return current + $"  '{pkg.StartsWithPattern}', '{pkg.Version}'{msg1}{msg2}\n";
            });
    }
}