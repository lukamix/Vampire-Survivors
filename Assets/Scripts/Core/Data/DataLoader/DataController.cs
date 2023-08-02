using DataLoader.VO;
public class DataController : Singleton<DataController>
{
    private SoldiersVO soldiers_VO;
    private ShopVO shop_VO;
    private SkillsVO skillsVO;
    private UserDataVO userData_VO;
    private EffectVO _effect_VO;
    private ItemVO _item_VO;
    private LevelVO _level_VO;

    public EffectVO effectVO
    {
        get
        {
            if (_effect_VO == null)
                _effect_VO = new EffectVO();
            return _effect_VO;
        }
    }

    public UserDataVO UserDataVo
    {
        get
        {
            if (userData_VO == null)
                userData_VO = new UserDataVO();
            return userData_VO;
        }
    }

    public SkillsVO SkillsVO
    {
        get
        {
            if (skillsVO == null)
                skillsVO = new SkillsVO();
            return skillsVO;
        }
    }

    public ShopVO ShopVO
    {
        get
        {
            if (shop_VO == null)
                shop_VO = new ShopVO();
            return shop_VO;
        }
    }

    public SoldiersVO soldiersVO
    {
        get
        {
            if (soldiers_VO == null)
                soldiers_VO = new SoldiersVO();
            return soldiers_VO;
        }
    }

    public ItemVO itemVO
    {
        get
        {
            if (_item_VO == null)
                _item_VO = new ItemVO();
            return _item_VO;
        }
    }

    public LevelVO levelVO
    {
        get
        {
            if (_level_VO == null)
                _level_VO = new LevelVO();
            return _level_VO;
        }
    }
}
