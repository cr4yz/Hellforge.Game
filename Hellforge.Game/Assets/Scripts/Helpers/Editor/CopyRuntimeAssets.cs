using UnityEditor;
using System.IO;
using UnityEditor.Build;
using UnityEngine;
using UnityEditor.Build.Reporting;

class CopyRuntimeAssets : IPostprocessBuildWithReport
{
    public int callbackOrder { get { return 0; } }

    public void OnPostprocessBuild(BuildReport report)
    {
        var sourceFolder = Application.dataPath + "/RuntimeAssets";
        var targetFolder = Path.GetDirectoryName(report.summary.outputPath) + "\\" + Application.productName + "_Data\\RuntimeAssets";
        FileUtil.CopyFileOrDirectory(sourceFolder, targetFolder);
    }
}