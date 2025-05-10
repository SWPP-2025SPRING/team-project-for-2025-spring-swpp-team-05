using UnityEditor;
using UnityEngine;
using System.Linq;

public class WebGLBuildScript
{
    [MenuItem("Build/WebGL")]
    public static void BuildWebGL()
    {
        // 컬러 스페이스 강제 설정
        PlayerSettings.colorSpace = ColorSpace.Gamma;

        // 빌드 설정에 등록된 씬만 사용
        var scenes = EditorBuildSettings.scenes
            .Where(s => s.enabled)
            .Select(s => s.path)
            .ToArray();

        if (scenes.Length == 0)
        {
            Debug.LogError("No scenes are added to the Build Settings!");
            return;
        }

        BuildPipeline.BuildPlayer(
            scenes,
            "Build/WebGL",
            BuildTarget.WebGL,
            BuildOptions.None
        );
    }
}