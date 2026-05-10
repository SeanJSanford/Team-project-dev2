using UnityEngine;

/*
 * PlayerCamera
 * 
 * Purpose:
 * Handles the player's gameplay camera movement and positioning.
 * 
 * Current Behavior:
 * - Follows a target transform
 * - Smoothly interpolates camera movement
 * - Maintains a fixed isometric-style rotation
 * 
 * Why This Exists:
 * Separating camera behavior into its own script
 * keeps movement, combat, and camera systems modular.
 * 
 * Connected Systems:
 * - PlayerMovement
 * - Player GameObject
 * - Future camera shake systems
 * - Future lock-on systems
 * - Future cinematic systems
 * 
 * Design Notes:
 * This camera currently behaves as a simple
 * top-down/isometric follow camera.
 * 
 * It should NOT:
 * - Control player movement
 * - Handle aiming logic
 * - Handle combat systems
 * 
 * Those systems should remain independent.
 * 
 * Future Expansion Ideas:
 * - Camera shake
 * - Dynamic zoom
 * - Target lock-on
 * - Mouse look influence
 * - Arena boss framing
 * - Split-screen support
 * - Camera collision handling
 * - Dynamic combat offsets
 */

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