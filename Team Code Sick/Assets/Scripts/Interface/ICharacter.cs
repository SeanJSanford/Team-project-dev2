using UnityEngine;

public interface ICharacter
{
    float HP { get; set; }
    float Speed { get; set; }
    float Damage { get; set; }
    float Resistance {  get; set; }
    bool timerLock { get; set; }

    void ModifyStat(NxStatType stat, float amount);
    /*
     * 
      Implement this function in Player and Enemies

      switch (stat)
        {
            case StatType.HP:
                HP = amount;
                break;

            case StatType.Speed:
                Speed = amount;
                break;

            case StatType.Damage:
                Damage = amount;
                break;

            case StatType.Resistance:
                Resistance = amount;
                break;
        }
     */

    float GetStat(NxStatType stat);
    /*
     * 
      Implement this function in Player and Enemies

      switch (stat)
        {
            case StatType.HP:
                return HP;

            case StatType.Speed:
                return Speed;

            case StatType.Damage:
                return Damage;

            case StatType.Resistance:
                return Resistance;
        }
     */
}
