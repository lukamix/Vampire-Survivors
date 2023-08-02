public class ItemVO : BaseMutilVO
{
    public ItemVO()
    {
        LoadDataByDirectories<BaseVO>("Items");
    }
}
