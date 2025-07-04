using System.Linq;
using UnityEditor;
using UnityEngine;

public class WebGLBuildScript
{
    [MenuItem("Build/WebGL")]
    public static void BuildWebGL()
    {
        // 컬러 스페이스 강제 설정
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.WebGL, BuildTarget.WebGL);
        PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Disabled;
        PlayerSettings.colorSpace = ColorSpace.Gamma;

        // 빌드 설정에 등록된 씬만 사용
        var scenes = EditorBuildSettings.scenes
            .Where(s => s.enabled)
            .Select(s => s.path)
            .ToArray();

        if (scenes.Length == 0)
        {
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