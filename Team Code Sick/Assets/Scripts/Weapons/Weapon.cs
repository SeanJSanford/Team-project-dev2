using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Inventory/Weapon")]
public class Weapon : ScriptableObject
{
    public float shootsPerSecond;
    public float damage;
    public float slowDown;
    public ElementType elementType;
    public int lifeStealAmount = 1;
    public Vector3 bulletSize = new Vector3(1f, 1f, 1f);
    public int bulletFrequency = 0;
    public int maxFrequency = 5;
    public bool lifeSteal = false;
    public bool multipleElements = false;
    public bool biggerBullet = false;

    //public void SetWeapon()
    //{
        //gamemanager.instance.playerScript.SetWeapon(this);
    //}
}
