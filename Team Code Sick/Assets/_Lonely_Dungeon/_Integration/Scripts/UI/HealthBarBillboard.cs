using UnityEngine;

public class HealthBarBillboard : MonoBehaviour
{
    [Header("Camera Reference")]
    [SerializeField] private Camera targetCamera;

    [Header("Billboard Settings")]
    [SerializeField] private bool lockXRotation = false;
    [SerializeField] private bool lockZRotation = true;

    private void Awake()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;
    }

    private void LateUpdate()
    {
        if (targetCamera == null)
            return;

        transform.rotation = targetCamera.transform.rotation;

        Vector3 eulerRotation = transform.eulerAngles;

        if (lockXRotation)
            eulerRotation.x = 0f;

        if (lockZRotation)
            eulerRotation.z = 0f;

        transform.eulerAngles = eulerRotation;
    }
}


/*
========================================================
Project: Team Code Sick / Lonely Dungeon
Script: HealthBarBillboard.cs

Primary Developer:
- Avery Wilson

System Category:
- UI
- Enemy Health UI
- Camera-facing World Space UI

Purpose:
- Keeps world-space enemy health bars facing the camera.
- Prevents enemy health UI from rotating awkwardly with enemy models.
- Improves readability for fixed isometric/top-down camera gameplay.

Connected Systems:
- EnemyHealth
- EnemyStats
- PlayerCamera

Development Notes:
- Attach this script to the HealthBarCanvas child object.
- Designed for world-space enemy health bars.
========================================================
*/
