using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ItemsDescription")]
public class ItemsContent : ScriptableObject
{
    public List<ItemDescription> items;
}

[Serializable]
public class ItemDescription
{
    public uint id;
    public EItemType type;
    public EArmorType armorType;
    public ECaliberType caliberType;
    public Sprite icon;
    public string name;
    public float weight;
    public int maxCount;
    public float healValue;
    public float armorValue;
    public float damage;
}