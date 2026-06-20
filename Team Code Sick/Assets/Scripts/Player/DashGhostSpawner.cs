using UnityEngine;
using System.Collections;

public class DashGhostSpawner : MonoBehaviour
{
    [SerializeField] Transform visualRoot;
    [SerializeField] Material ghostMaterial;
    [SerializeField] float ghostLifeTime = 0.25f;
    [SerializeField] float ghostAlpha = 0.4f;

    public void SpawnGhost()
    {
        if (visualRoot == null)
        {
            Debug.LogWarning("DashGhostSpawner: Visual Root is missing.");
            return;
        }

        if (ghostMaterial == null)
        {
            Debug.LogWarning("DashGhostSpawner: Ghost Material is missing.");
            return;
        }

        GameObject ghostRoot = new GameObject("Dash Ghost");

        int ghostPartsCreated = 0;

        // For animated character models
        SkinnedMeshRenderer[] skinnedMeshes = visualRoot.GetComponentsInChildren<SkinnedMeshRenderer>();

        foreach (SkinnedMeshRenderer skin in skinnedMeshes)
        {
            Mesh bakedMesh = new Mesh();
            skin.BakeMesh(bakedMesh);

            GameObject ghostPart = new GameObject(skin.name + " Ghost");

            ghostPart.transform.position = skin.transform.position;
            ghostPart.transform.rotation = skin.transform.rotation;
            ghostPart.transform.localScale = skin.transform.lossyScale;
            ghostPart.transform.SetParent(ghostRoot.transform, true);

            MeshFilter mf = ghostPart.AddComponent<MeshFilter>();
            MeshRenderer mr = ghostPart.AddComponent<MeshRenderer>();

            mf.mesh = bakedMesh;
            mr.materials = CreateGhostMaterials(skin.sharedMaterials.Length);

            mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            mr.receiveShadows = false;

            ghostPartsCreated++;
        }

        // For normal meshes like your capsule
        MeshFilter[] meshFilters = visualRoot.GetComponentsInChildren<MeshFilter>();

        foreach (MeshFilter sourceFilter in meshFilters)
        {
            MeshRenderer sourceRenderer = sourceFilter.GetComponent<MeshRenderer>();

            if (sourceRenderer == null)
                continue;

            GameObject ghostPart = new GameObject(sourceFilter.name + " Ghost");

            ghostPart.transform.position = sourceFilter.transform.position;
            ghostPart.transform.rotation = sourceFilter.transform.rotation;
            ghostPart.transform.localScale = sourceFilter.transform.lossyScale;
            ghostPart.transform.SetParent(ghostRoot.transform, true);

            MeshFilter mf = ghostPart.AddComponent<MeshFilter>();
            MeshRenderer mr = ghostPart.AddComponent<MeshRenderer>();

            mf.sharedMesh = sourceFilter.sharedMesh;
            mr.materials = CreateGhostMaterials(sourceRenderer.sharedMaterials.Length);

            mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            mr.receiveShadows = false;

            ghostPartsCreated++;
        }

        if (ghostPartsCreated == 0)
        {
            Debug.LogWarning("DashGhostSpawner: No mesh found under Visual Root.");
            Destroy(ghostRoot);
            return;
        }

        StartCoroutine(FadeAndDestroy(ghostRoot));

        
    }

    Material[] CreateGhostMaterials(int count)
    {
        Material[] mats = new Material[count];

        for (int i = 0; i < count; i++)
        {
            Material mat = new Material(ghostMaterial);

            Color color = mat.color;
            color.a = ghostAlpha;
            mat.color = color;

            mats[i] = mat;
        }

        return mats;
    }

    IEnumerator FadeAndDestroy(GameObject ghostRoot)
    {
        Renderer[] renderers = ghostRoot.GetComponentsInChildren<Renderer>();

        float timer = 0f;

        while (timer < ghostLifeTime)
        {
            float alpha = Mathf.Lerp(ghostAlpha, 0f, timer / ghostLifeTime);

            foreach (Renderer rend in renderers)
            {
                foreach (Material mat in rend.materials)
                {
                    Color color = mat.color;
                    color.a = alpha;
                    mat.color = color;
                }
            }

            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(ghostRoot);
    }
}