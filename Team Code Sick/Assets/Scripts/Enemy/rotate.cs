using UnityEngine;

public class rotate : MonoBehaviour
{
    [SerializeField] int speed;

    private Quaternion rotation;

    void Start()
    {
        // Cache the child's starting world rotation
        rotation = transform.rotation;
    }
    // Update is called once per frame
    void Update()
    {
        rotation = Quaternion.Euler(0f, speed * Time.deltaTime, 0f);

        transform.rotation = rotation;
    }
}