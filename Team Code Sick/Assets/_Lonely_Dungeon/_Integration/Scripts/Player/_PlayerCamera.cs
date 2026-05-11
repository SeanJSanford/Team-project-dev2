using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("Target")]

    // Object the camera follows.
    [SerializeField] private Transform target;

    [Header("Camera Position")]

    // Offset from the target position.
    // Controls camera distance and viewing angle.
    [SerializeField]
    private Vector3 offset =
        new Vector3(8f, 14f, -8f);

    [Header("Camera Movement")]

    // Speed used when interpolating camera movement.
    [SerializeField] private float smoothSpeed = 5f;

    [Header("Camera Rotation")]

    // Fixed gameplay camera rotation.
    [SerializeField]
    private Vector3 cameraRotation =
        new Vector3(55f, -45f, 0f);

    /*
     * LateUpdate
     * 
     * Runs after normal Update calls.
     * 
     * Camera movement is handled here to ensure:
     * - Player movement updates first
     * - Camera movement stays smooth
     * - Visual jitter is reduced
     */
    private void LateUpdate()
    {
        // Prevent errors if no target exists.
        if (target == null)
            return;

        // Desired camera position relative to the target.
        Vector3 targetPosition =
            target.position + offset;

        // Smoothly interpolate toward the target position.
        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            smoothSpeed * Time.deltaTime
        );

        // Apply fixed gameplay rotation.
        transform.rotation =
            Quaternion.Euler(cameraRotation);
    }
}

/*
========================================================
Project: Team Code Sick
Script: PlayerCamera.cs

Primary Developer:
- Dai

Integration Support:
- Avery Wilson

System Category:
- Camera System
- Player Follow Camera
- Isometric Gameplay Camera

Purpose:
- Handles gameplay camera positioning and movement.
- Smoothly follows the player while maintaining
  a fixed angled/isometric gameplay perspective.

Current Features:
- Smooth follow movement
- Configurable camera offset
- Fixed gameplay rotation
- Isometric/top-down framing
- Jitter reduction through LateUpdate()

Connected Team Systems:
- Dai: Player movement systems
- Avery: Gameplay readability and combat integration testing
- Sean: Combat aiming readability
- Nilo: Gameplay direction and camera feel

Why This Exists:
Separating camera logic into its own system
keeps gameplay architecture modular and easier
to maintain.

This prevents camera code from overlapping with:
- Player movement
- Combat systems
- Weapon systems
- Enemy AI systems

Design Philosophy:
This camera currently functions as a lightweight
isometric gameplay camera focused on:
- Combat readability
- Enemy visibility
- Smooth player tracking
- Arena awareness

Responsibilities intentionally excluded:
- Player movement logic
- Combat calculations
- Weapon aiming
- Enemy targeting
- UI systems

These responsibilities remain separated into
their own gameplay systems.

Development Notes:
- Uses LateUpdate() to reduce visual jitter.
- Camera movement occurs after player movement updates.
- Designed for fast gameplay iteration and prototyping.
- Built to support isometric/top-down combat readability.

Current Limitations:
- No camera collision handling
- No dynamic zoom
- No camera shake
- No lock-on support
- No cinematic transitions
- No edge scrolling

Future Expansion Ideas:
- Camera shake
- Dynamic zoom
- Combat framing
- Boss arena framing
- Lock-on systems
- Mouse influence
- Split-screen support
- Camera collision handling
- Dynamic combat offsets
========================================================
*/