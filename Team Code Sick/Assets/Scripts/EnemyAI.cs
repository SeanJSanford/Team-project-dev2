using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{

    [SerializeField] Renderer rend;
    [SerializeField] int HP;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;
    [SerializeField] Transform gunPivot;
    [SerializeField] Transform shootPos;
    [SerializeField] int gunRotateSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
