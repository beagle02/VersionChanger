namespace VersionChanger.Models;

public class Package
{
    /// <summary>
    /// Шаблон поиска пакета по правилу "начинается с"
    /// </summary>
    public string StartsWithPattern { get; set; }

    /// <summary>
    /// Новая версия пакета
    /// </summary>
    public string Version { get; set; }

    /// <summary>
    /// Новая версия файла (необходима для проектов .Net Framework)
    /// </summary>
    public string FileVersion { get; set; }

    /// <summary>
    /// Версия файла (опционально), которая не будет изменяться (для проектов .Net Framework)
    /// </summary>
    public string IgnoreFileVersion { get; set; }

    /// <summary>
    /// Признак описания пакета для .Net Framework проекта
    /// </summary>
    public bool IsFrameworkProject => !string.IsNullOrWhiteSpace(FileVersion);
}