using UnityEngine;

public class Ice : Element
{
    public Ice() : base(ElementType.Ice,
                        2, 
                        1.05f, 
                        .5f,
                        new NxStatType[] { NxStatType.Resistance },
                        new NxStatType[] { NxStatType.Speed, NxStatType.FireRate })
    {
        // For 2 seconds it takes 20% of Speed and FireRate off
    }
}
