using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

public partial class AddinFinder
{
    List<string> fodyFiles;

    public List<string> FodyFiles
    {
        get
        {
            if (fodyFiles == null)
            {
                fodyFiles = FindAddinDirectoriesLegacy().ToList();
            }

            return fodyFiles;
        }
    }

    public string FindAddinAssembly(string packageName)
    {
        Debug.Assert(weaversFromWellKnownPaths != null, "call FindAddinDirectories() first");

        if (weaversFromWellKnownPaths.TryGetValue(packageName, out var filePath))
        {
            return filePath;
        }

        var packageFileName = packageName + WeaverDllSuffix;

        return FodyFiles.Where(x => string.Equals(Path.GetFileName(x), packageFileName, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(AssemblyVersionReader.GetAssemblyVersion)
            .FirstOrDefault();
    }
}