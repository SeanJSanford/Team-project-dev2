using System.Collections;
using UnityEngine;

public class BossCutscene : MonoBehaviour
{
    public IEnumerator PlayBossIntro(Transform boss)
    {
        Camera cam = Camera.main;

        playerCamera camScript =
            cam.GetComponent<playerCamera>();

        if (camScript != null)
            camScript.enabled = false;

        Vector3 oldPos = cam.transform.position;
        Quaternion oldRot = cam.transform.rotation;

        Vector3 startPos =
            boss.position + new Vector3(0f, 3f, -5f);

        Vector3 endPos =
            boss.position + new Vector3(0f, 1.5f, -2f);

        cam.transform.position = startPos;
        cam.transform.LookAt(boss.position + Vector3.up);

        float duration = 2f;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime;

            float t = timer / duration;

            cam.transform.position =
                Vector3.Lerp(startPos, endPos, t);

            cam.transform.LookAt(
                boss.position + Vector3.up
            );

            // Screen shake
            cam.transform.position +=
                Random.insideUnitSphere * 0.05f;

            yield return null;
        }

        yield return new WaitForSecondsRealtime(0.5f);

        cam.transform.position = oldPos;
        cam.transform.rotation = oldRot;

        if (camScript != null)
            camScript.enabled = true;
    }
}