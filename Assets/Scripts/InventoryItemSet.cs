public class InventoryItemSet
{
    public Item item;
    public int itemCount;

    private float _currCoolTime = 0f;
    private float _maxCoolTime = 10f;

    public InventoryItemSet(Item item, int count)
    {
        this.item = item;
        itemCount = count;
    }

    public float Get_CurrentCoolTime() { return _currCoolTime; }
    public float Get_MaxCoolTime() { return _maxCoolTime; }
}
