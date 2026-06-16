using UnityEngine;

public class Fire : Element
{
    public Fire() : base(ElementType.Fire,
                         5, 
                         1.25f, 
                         .9f, 
                         new NxStatType[] { NxStatType.Damage },
                         new NxStatType[] { NxStatType.Resistance }) 
    {
        // For 5 second it takes 10% of resistance off
    }
}
