using UnityEngine;
using System.Collections.Generic;

public class CameraObstructionHandler : MonoBehaviour
{
    public Transform player;
    public LayerMask obstructionMask;
    public float sphereRadius = 0.5f;

    private List<Renderer> currentRenderers = new List<Renderer>();

    void Update()
    {
        ResetPrevious(); // 기존 투명 처리 복원

        Vector3 direction = player.position - transform.position;
        float distance = direction.magnitude;

        // 🔍 Raycast: 카메라와 플레이어 사이 막힘 감지
        if (Physics.Raycast(transform.position, direction.normalized, out RaycastHit hit, distance, obstructionMask))
        {
            TryMakeTransparent(hit.collider.GetComponent<Renderer>());
        }

        // 🔍 OverlapSphere: 카메라 주변에 들어간 오브젝트도 감지
        Collider[] overlaps = Physics.OverlapSphere(transform.position, sphereRadius, obstructionMask);
        foreach (var col in overlaps)
        {
            TryMakeTransparent(col.GetComponent<Renderer>());
        }
    }

    void TryMakeTransparent(Renderer rend)
    {
        if (rend == null || currentRenderers.Contains(rend)) return;

        foreach (Material mat in rend.materials)
        {
            SetMaterialTransparent(mat, 0.3f);
        }
        currentRenderers.Add(rend);
    }

    void ResetPrevious()
    {
        foreach (Renderer rend in currentRenderers)
        {
            if (rend == null) continue;

            foreach (Material mat in rend.materials)
            {
                SetMaterialOpaque(mat);
            }
        }
        currentRenderers.Clear();
    }

    void SetMaterialTransparent(Material mat, float alpha)
    {
        mat.SetFloat("_Mode", 2);
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.EnableKeyword("_ALPHABLEND_ON");
        mat.renderQueue = 3000;

        Color c = mat.color;
        c.a = alpha;
        mat.color = c;
    }

    void SetMaterialOpaque(Material mat)
    {
        mat.SetFloat("_Mode", 0);
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        mat.SetInt("_ZWrite", 1);
        mat.DisableKeyword("_ALPHABLEND_ON");
        mat.renderQueue = -1;

        Color c = mat.color;
        c.a = 1f;
        mat.color = c;
    }
}
