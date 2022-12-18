using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using VersionChanger.Models;

namespace VersionChanger.Extensions;

public static class XNodeExtensions
{
    private static IXmlNamespaceResolver CreateNamespaceResolver()
    {
        var manager = new XmlNamespaceManager(new NameTable());
        manager.AddNamespace("ns", "http://schemas.microsoft.com/developer/msbuild/2003");
        return manager;
    }

    /// <summary>
    /// Проверить, является ли проект .Net Framework проектом
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public static bool IsFrameworkProject(this XNode node)
    {
        var manager = CreateNamespaceResolver();
        return node.XPathSelectElements("/ns:Project[@ToolsVersion]", manager).Any();
    }

    /// <summary>
    /// Изменить версию пакета в .Net Core проекте
    /// </summary>
    /// <param name="xmlNode"></param>
    /// <param name="pkg"></param>
    /// <returns></returns>
    public static bool TryReplaceInProject(this XNode xmlNode, Package pkg)
    {
        var result = false;

        var query = from c in xmlNode.XPathSelectElements("/Project/ItemGroup/PackageReference")
            where c.Attribute("Include")!.Value.StartsWith(pkg.StartsWithPattern)
            select c;

        foreach (var elem in query)
        {
            var attr = elem.Attribute("Version");
            if (attr == null) continue;

            attr.Value = pkg.Version;
            result = true;
        }

        return result;
    }

    /// <summary>
    /// Изменить версию пакета в .Net Framework проекте
    /// </summary>
    /// <param name="xmlNode"></param>
    /// <param name="pkg"></param>
    /// <returns></returns>
    public static bool TryReplaceInFrameworkProject(this XNode xmlNode, Package pkg)
    {
        var result = false;

        var manager = CreateNamespaceResolver();

        var evaluator = new MatchEvaluator(m =>
            m.Value == pkg.IgnoreFileVersion || string.IsNullOrWhiteSpace(pkg.IgnoreFileVersion)
                ? m.Value
                : pkg.FileVersion);

        var query = from c in xmlNode.XPathSelectElements("/ns:Project/ns:ItemGroup/ns:Reference", manager)
            where c.Attribute("Include")!.Value.StartsWith(pkg.StartsWithPattern)
            select c;

        foreach (var elem in query)
        {
            var attr = elem.Attribute("Include");
            var elem2 = elem.XPathSelectElement("./ns:HintPath", manager);
            if (attr == null || elem2 == null) continue;

            attr.Value = Regex.Replace(attr.Value, "(?<=.*Version=).*(?=, Culture.*)", evaluator);
            elem2.Value = Regex.Replace(elem2.Value, "(?<=.*\\.)\\d+\\.\\d+\\.\\d+.*(?=\\\\lib.*)", pkg.Version);
            result = true;
        }

        return result;
    }

    /// <summary>
    /// Изменить версию пакета в package.config файле
    /// </summary>
    /// <param name="xmlNode"></param>
    /// <param name="pkg"></param>
    /// <returns></returns>
    public static bool TryReplaceInPackageConfig(this XNode xmlNode, Package pkg)
    {
        var result = false;

        var query = from c in xmlNode.XPathSelectElements("/packages/package")
            where c.Attribute("id")!.Value.StartsWith(pkg.StartsWithPattern)
            select c;

        foreach (var elem in query)
        {
            var attr = elem.Attribute("version");
            if (attr == null) continue;

            attr.Value = pkg.Version;
            result = true;
        }

        return result;
    }
}