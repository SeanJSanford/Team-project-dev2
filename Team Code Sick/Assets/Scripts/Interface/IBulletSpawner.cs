using UnityEngine;

public interface IBulletSpawner
{
    Element elementType { get; set; }

    public void SetElement(Element element);

}
