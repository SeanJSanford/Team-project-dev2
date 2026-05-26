using UnityEngine;
using System.Collections;

public class DashGhostFade : MonoBehaviour
{
    [SerializeField] float lifeTime = 0.25f;
    [SerializeField] float startAlpha = 0.4f;

    Renderer[] renderers;

    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
        StartCoroutine(FadeAndDestroy());
    }

    IEnumerator FadeAndDestroy()
    {
        float timer = 0f;

        while (timer < lifeTime)
        {
            float alpha = Mathf.Lerp(startAlpha, 0f, timer / lifeTime);

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

        Destroy(gameObject);
    }
}