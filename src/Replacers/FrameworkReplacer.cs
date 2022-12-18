using System.Xml.Linq;
using VersionChanger.Extensions;
using VersionChanger.Models;

namespace VersionChanger.Replacers;

public class FrameworkReplacer : IReplacer
{
    private readonly Settings _settings;

    public FrameworkReplacer(Settings settings)
    {
        _settings = settings;
    }

    public bool Replace(XNode project, XNode package = default)
    {
        var result = ReplaceProject(project);
        if (package != null)
            result |= ReplacePackage(package);

        return result;
    }

    private bool ReplaceProject(XNode xmlFile)
    {
        return _settings.Packages.Aggregate(false,
            (current, pkg) => current | xmlFile.TryReplaceInFrameworkProject(pkg));
    }

    private bool ReplacePackage(XNode xmlFile)
    {
        return _settings.Packages.Aggregate(false,
            (current, pkg) => current | xmlFile.TryReplaceInPackageConfig(pkg));
    }
}