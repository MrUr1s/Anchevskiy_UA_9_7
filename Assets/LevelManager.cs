using System.Collections;
using System.Linq ;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    [SerializeField, Tooltip("Префаб блока")]
    private Transform _block;
    [SerializeField, Tooltip("Блоки")]
    private List<Transform> _blocks;
    [SerializeField, Tooltip("Количество блоков")]
    private int _count = 10;
    [SerializeField, Tooltip("Размер тунеля")]
    private int _sizeTunnel=2;
    [SerializeField, Tooltip("Длина тунеля")]
    private int _lengthTunnel=8;

    public int Count
    {
        get => _count;
        set => _count = value;
    }

    public void Reset()
    {
        Debug.Log("NextLevel");
        int sizeTunnel = _sizeTunnel / 2;
        int lengthTunnel = _lengthTunnel / 2;
        for (var i = 0; i < Count; i++)
            {
                if (_blocks.All(t => t.gameObject.activeSelf)&&_blocks.Count<Count)
                    _blocks.Add(Instantiate(_block,
                    transform.position + new Vector3(Random.Range(-sizeTunnel, sizeTunnel + 1), Random.Range(-sizeTunnel, sizeTunnel + 1), Random.Range(-lengthTunnel, lengthTunnel + 1)),
                    new Quaternion(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)),
                    this.transform));
                else
            {
                _blocks.ForEach(t => t.gameObject.SetActive(true));
                return;
            }  
                
            }
    }
    public bool CheckLevel()
    {
        var isCheck = _blocks.All(t => !t.gameObject.activeSelf);
        if (isCheck)
            Reset();
        return isCheck;
    }
    private void Awake()
    {
        Reset();
        if(instance == null)
        instance=this;
    }

    
}
