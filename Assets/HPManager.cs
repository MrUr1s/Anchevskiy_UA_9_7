using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPManager : MonoBehaviour
{
    [SerializeField, Tooltip("�������� �������")]
    private int _hp=3;
    private int _defaultHP;
    public int HP => _hp;

    public void setHP(int hp = -1)
    {
        _hp += hp;
        Debug.Log("�������� ��������"+_hp);
        if(_hp <= 0)
        {
            Reset();
            Debug.Log("��������");
        }
    }

    public void Reset()
    {
        _hp = _defaultHP;
        FindObjectOfType<Ball>().Reset();
        LevelManager.instance.Reset();
           
    }
    private void Awake()
    {
        _defaultHP = _hp;
    }
}
