using System.Xml.Linq;

namespace VersionChanger.Replacers;

public interface IReplacer
{
    bool Replace(XNode project, XNode package = default);
}