using UnityEngine;

[CreateAssetMenu(menuName ="Shadow Protocol/Loot Data")]
public class LootDataSO : ScriptableObject  //to easily create multiple collectible data
{
    public string lootname;
    public int value;
    public Color meshColor;
}
