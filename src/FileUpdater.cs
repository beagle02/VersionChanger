using System.Xml.Linq;
using VersionChanger.Extensions;
using VersionChanger.Models;
using VersionChanger.Replacers;

namespace VersionChanger;

public class FileUpdater
{
    private readonly bool _justShow;
    private readonly IReplacer _coreReplacer;
    private readonly IReplacer _frameworkReplacer;
    private readonly Action<string> _onVersionReplaced;

    public FileUpdater(Settings settings, Action<string> versionReplacedFunc, bool justShow)
    {
        _onVersionReplaced = versionReplacedFunc;
        _justShow = justShow;
        _coreReplacer = new CoreReplacer(settings);
        _frameworkReplacer = new FrameworkReplacer(settings);
    }

    public void Update(string filename)
    {
        string packageName = null;
        XDocument xmlPackageFile = default;
        bool handled;

        var xmlProjectFile = XDocument.Load(filename);

        if (xmlProjectFile.IsFrameworkProject())
        {
            packageName = GetPackageConfigFilename(filename);
            if (packageName != null)
                xmlPackageFile = XDocument.Load(packageName);

            handled = _frameworkReplacer.Replace(xmlProjectFile, xmlPackageFile);
        }
        else
            handled = _coreReplacer.Replace(xmlProjectFile);

        if (!handled)
            return;

        if (_justShow)
            Display(filename, xmlProjectFile, packageName, xmlPackageFile);
        else
            Save(filename, xmlProjectFile, packageName, xmlPackageFile);
    }

    private static string GetPackageConfigFilename(string projectFilename)
    {
        var path = Path.GetDirectoryName(projectFilename);
        if (!Directory.Exists(path))
            return null;

        var packageName = Path.Combine(path, "packages.config");
        return File.Exists(packageName) ? packageName : null;
    }

    private void Display(string filename, XNode xmlProjectFile, string packageName, XNode xmlPackageFile)
    {
        if (_onVersionReplaced == null)
            return;

        _onVersionReplaced.Invoke(filename);
        _onVersionReplaced.Invoke(xmlProjectFile.ToString());

        if (string.IsNullOrWhiteSpace(packageName))
            return;

        _onVersionReplaced.Invoke(packageName);
        _onVersionReplaced.Invoke(xmlPackageFile?.ToString());
    }

    private void Save(string filename, XDocument xmlProjectFile, string packageName, XDocument xmlPackageFile)
    {
        if (_onVersionReplaced == null)
            return;

        xmlProjectFile.Save(filename, SaveOptions.None);
        _onVersionReplaced.Invoke(filename);

        if (string.IsNullOrWhiteSpace(packageName))
            return;

        xmlPackageFile?.Save(packageName, SaveOptions.None);
        _onVersionReplaced.Invoke(packageName);
    }
}