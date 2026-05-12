using UnityEngine;

public class PlayerTopDownController : MonoBehaviour
{
    [SerializeField] CharacterController controller;
    [SerializeField] int speed = 6;

    Vector3 moveDir;

    void Start()
    {
        if (controller == null)
            controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Movement();
        AimAtMouse();
    }

    void Movement()
    {
        moveDir = new Vector3(
            Input.GetAxisRaw("Horizontal"),
            0,
            Input.GetAxisRaw("Vertical")
        ).normalized;

        controller.Move(moveDir * speed * Time.deltaTime);
    }

    void AimAtMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 point = ray.GetPoint(distance);
            Vector3 direction = point - transform.position;
            direction.y = 0;

            if (direction.sqrMagnitude > 0.01f)
                transform.forward = direction;
        }
    }
}