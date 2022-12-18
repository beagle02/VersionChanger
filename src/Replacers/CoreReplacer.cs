using System.Xml.Linq;
using VersionChanger.Extensions;
using VersionChanger.Models;

namespace VersionChanger.Replacers;

public class CoreReplacer : IReplacer
{
    private readonly Settings _settings;

    public CoreReplacer(Settings settings)
    {
        _settings = settings;
    }

    public bool Replace(XNode project, XNode package = default)
    {
        return _settings.Packages.Aggregate(false,
            (current, pkg) => current | project.TryReplaceInProject(pkg));
    }
}