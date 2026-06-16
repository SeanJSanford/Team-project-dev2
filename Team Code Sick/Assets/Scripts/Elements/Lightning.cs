using UnityEngine;

public class Lightning : Element
{
    public Lightning() : base(ElementType.Lightning,
                              1, 
                              1.1f, 
                              .4f,
                              new NxStatType[] { NxStatType.FireRate },
                              new NxStatType[] { NxStatType.Speed })
    {
        // For 1 seconds it takes 60% of Speed off 
    }
}
