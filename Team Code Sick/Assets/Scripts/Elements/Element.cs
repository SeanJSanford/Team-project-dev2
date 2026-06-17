using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Element
{
    public ElementType type { get; protected set; }
    public float timer { get; protected set; }
    public float BuffAmount { get; protected set; }
    public float DebuffAmount { get; protected set; }
    public NxStatType[] BuffTargetType { get; protected set; }
    public NxStatType[] DebuffTargetType { get; protected set; }
    public void ModifyTargetBuffPickup(ICharacter target)
    {

        for (int typeIndex = 0; typeIndex < BuffTargetType.Length; typeIndex++)
            target.ModifyStat(BuffTargetType[typeIndex], BuffAmount);
    }
    public void ModifyTargetBuffDrop(ICharacter target)
    {

        for (int typeIndex = 0; typeIndex < BuffTargetType.Length; typeIndex++)
            target.ModifyStat(BuffTargetType[typeIndex], -BuffAmount);
    }
    public IEnumerator ModifyTargetDebuff(ICharacter target)
    {
        List<float> originalValues = new List<float>();
        foreach (NxStatType _type in DebuffTargetType)
        {
            float originalAmount = target.GetStat(_type);
            originalValues.Add(originalAmount);
            target.SetStat(_type, originalAmount * DebuffAmount);
        }

        target.timerLock = true;

        yield return new WaitForSeconds(timer);

        target.timerLock = false;

        for (int typeIndex = 0; typeIndex < DebuffTargetType.Length; typeIndex++)
            target.SetStat(DebuffTargetType[typeIndex], originalValues[typeIndex]);
    }

    public Element(ElementType _type, float _timer, float _BuffAmount, float _DebuffAmount, NxStatType[] _BuffTargetType, NxStatType[] _DebuffTargetType)
    {
        type = _type;
        timer = _timer;
        BuffAmount = _BuffAmount;
        DebuffAmount = _DebuffAmount;
        BuffTargetType = _BuffTargetType;
        DebuffTargetType = _DebuffTargetType;
    }

    public static Element ElementObject(int type)
    {
        switch (type)
        {
            case ((int)ElementType.Fire):
                return new Fire();
            case ((int)ElementType.Ice):
                return new Ice();
            case ((int)ElementType.Lightning):
                return new Lightning();
            case ((int)ElementType.Earth):
                return new Earth();
            default:
                return new Fire();
        }
    }

    public static ElementType RandomElement()
    {
        return (ElementType)Random.Range(0, (int)ElementType.ELEMENT_MAX);
    }
}