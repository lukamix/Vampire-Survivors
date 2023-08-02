using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXP : Item
{
    private EXPType expType = default;
    private SpriteRenderer _spriteRenderer;
    private SpriteRenderer spriteRenderer
    {
        get
        {
            if (_spriteRenderer == null) _spriteRenderer = GetComponent<SpriteRenderer>();
            return _spriteRenderer;
        }
    }
    [SerializeField] private List<Sprite> sprites;
    public void SetEXPType(EXPType type)
    {
        expType = type;
        spriteRenderer.sprite = sprites[(int)expType];
    }
    public EXPType GetEXPType()
    {
        return expType;
    }
}
public enum EXPType
{
    GREEN = 0,
    BLUE,
    Yellow,
    RED
}