using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEditor.Build.Reporting;

public class BuildHelper
{
    public static void PerformBuild()
    {
        string projectName = PlayerSettings.productName;
        string buildFolder = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "jenkinsBuild", projectName, "UnityBuild");

        if (!Directory.Exists(buildFolder))
            Directory.CreateDirectory(buildFolder);

        Debug.Log("Building iOS project: " + projectName + " into: " + buildFolder);

        
        string[] scenes = EditorBuildSettings.scenes
            .Where(scene => scene.enabled)
            .Select(scene => scene.path)
            .ToArray();

        if (scenes.Length == 0)
        {
            Debug.LogError("❌ No scenes enabled in Build Settings!");
            EditorApplication.Exit(1);
            return;
        }

        BuildPlayerOptions buildOptions = new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = buildFolder,
            target = BuildTarget.iOS,
            options = BuildOptions.None
        };

        BuildReport report = BuildPipeline.BuildPlayer(buildOptions);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("✅ Build succeeded: " + summary.totalSize + " bytes");
        }
        else if (summary.result == BuildResult.Failed)
        {
            Debug.LogError("❌ Build failed!");
            EditorApplication.Exit(1);
        }
    }
}
