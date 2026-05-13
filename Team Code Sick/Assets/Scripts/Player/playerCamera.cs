using UnityEngine;
/// <summary>
/// Script made by Dai
/// </summary>
public class playerCamera : MonoBehaviour
{
    

    [SerializeField] Vector3 offset = new Vector3(8f, 14f, -8f);
    [SerializeField] float smoothSpeed = 5f;
    [SerializeField] Vector3 cameraRotation = new Vector3(55f, -45f, 0f);
    void LateUpdate()
    {
        if (gamemanager.instance.player.transform == null)
            return;

        Vector3 targetPosition = gamemanager.instance.player.transform.position + offset;

        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(cameraRotation);
    }

}