using UnityEngine;

public class Earth : Element
{
    public Earth() : base(ElementType.Earth,
                          3,
                          1.5f,
                          .75f, 
                          new NxStatType[] { NxStatType.Resistance }, 
                          new NxStatType[] { NxStatType.Speed })
    {
        // For 3 seconds it takes 25% of Speed off 
    }
}
