# VersionChanger
This is a console application changing the package versions in .csproj files.
It works with .Net Core and .Net Framework projects.
It also changes the package versions in package.config file.

## Configuration
To configure replacement rules of package version you should describe them in appsettings.json file.
```json
{
  "Settings": {
    "Path": ".",
    "Packages": [
      {
        "StartsWithPattern": "AutoMapper",
        "Version": "10.1.1",
        "FileVersion": "10.0.0.0",
        "IgnoreFileVersion": "0.0.0.0"
      }
    ]
  }
}
```
Parameters:
- Path - a directory where projects will be searched recursively;
- Packages - an array of replacement rules
- StartsWithPattern - a package name pattern; required;
- Version - a new version of the package; required;
- FileVersion - a new file version; optional, used for .Net Framework projects only;
if this parameter is missed, .Net Framework project files wount be handled;
- IgnoreFileVersion - a version which will be ignored; optional, used for .Net Framework projects only;
if this parameter is specified, the file version wount be updated for package having specified version;

## Examples
### .Net Core project
```xml
  <ItemGroup>
    <PackageReference Include="AutoMapper" Version="10.0.0" />
  </ItemGroup>
```
In this case the AutoMapper package version will be replaced to 10.1.1. The result is
```xml
  <ItemGroup>
    <PackageReference Include="AutoMapper" Version="10.1.1" />
  </ItemGroup>
```

### .Net Framework project
```xml
    <Reference Include="AutoMapper, Version=10.0.0.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005, processorArchitecture=MSIL">
      <HintPath>..\..\packages\AutoMapper.10.0.0\lib\net461\AutoMapper.dll</HintPath>
    </Reference>
```
and package.config
```xml
<packages>
  <package id="AutoMapper" version="10.0.0" targetFramework="net472" />
```
In this case the AutoMapper package version will be replaced to 10.1.1. The file version will remain unchanged. The result is
```xml
    <Reference Include="AutoMapper, Version=10.0.0.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005, processorArchitecture=MSIL">
      <HintPath>..\..\packages\AutoMapper.10.1.1\lib\net461\AutoMapper.dll</HintPath>
    </Reference>
```
and package.config
```xml
<packages>
  <package id="AutoMapper" version="10.1.1" targetFramework="net472" />
```

