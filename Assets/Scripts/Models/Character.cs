using System;

[Serializable]
public class Character
{
    public float health;
    public float maxHealth;
    public string name;

    public ItemData headArmor;
    public ItemData bodyArmor;

    public ECaliberType caliberType;
    
    public ItemData[] inventory;
}
