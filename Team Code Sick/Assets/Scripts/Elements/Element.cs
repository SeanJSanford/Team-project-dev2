using UnityEngine;
using System.Collections;

public abstract class Element
{
    public int buffTimer { get; protected set; }
    public int debuffTimer { get; protected set; }
    public float BuffAmount { get; protected set; }
    public float DebuffAmount { get; protected set; }
    public NxStatType BuffTargetType { get; protected set; }
    public NxStatType DebuffTargetType { get; protected set; }
    public IEnumerator ModifyTargetBuff(ICharacter target)
    {
        float originalAmount = target.GetStat(BuffTargetType);

        target.ModifyStat(BuffTargetType, originalAmount * BuffAmount);

        yield return new WaitForSeconds(buffTimer);

        target.ModifyStat(BuffTargetType, originalAmount);

    }
    public IEnumerator ModifyTargetDebuff(ICharacter target)
    {
        float originalAmount = target.GetStat(DebuffTargetType);

        target.ModifyStat(DebuffTargetType, originalAmount * DebuffAmount);

        yield return new WaitForSeconds(debuffTimer);

        target.ModifyStat(DebuffTargetType, originalAmount);
    }
}