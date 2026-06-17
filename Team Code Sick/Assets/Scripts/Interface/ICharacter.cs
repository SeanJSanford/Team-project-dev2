using UnityEngine;

public interface ICharacter
{
    public float HP { get; set; }
    public float speed { get; set; }
    public float Damage { get; set; }
    public float Resistance {  get; set; }
    public float shootRate { get; set; }
    bool timerLock { get; set; }
    public Element elementType { get; set; }

    void ModifyStat(NxStatType stat, float amount);
    /*
     * 
      Implement this function in Player and Enemies

      switch (stat)
        {
            case NxStatType.HP:
                HP += amount;
                break;

            case NxStatType.Speed:
                speed += amount;
                break;

            case NxStatType.Damage:
                Damage += amount;
                break;

            case NxStatType.Resistance:
                Resistance += amount;
                break;
            case NxStatType.FireRate:
                shootRate += amount;
                break;
        }
     */
    void SetStat(NxStatType stat, float amount);
    /*
     * 
      Implement this function in Player and Enemies

      switch (stat)
        {
            case NxStatType.HP:
                HP = amount;
                break;

            case NxStatType.Speed:
                speed = amount;
                break;

            case NxStatType.Damage:
                Damage = amount;
                break;

            case NxStatType.Resistance:
                Resistance = amount;
                break;

            case NxStatType.FireRate:
                shootRate = amount;
                break;
        }
     */

    float GetStat(NxStatType stat);
    /*
     * 
      Implement this function in Player and Enemies

      switch (stat)
        {
            case NxStatType.HP:
                return HP;

            case NxStatType.Speed:
                return speed;

            case NxStatType.Damage:
                return Damage;

            case NxStatType.Resistance:
                return Resistance;

            case NxStatType.FireRate:
                return shootRate;
        }
     */
}
