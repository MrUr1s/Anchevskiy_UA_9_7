using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField, Tooltip("Скорость полета")]
    private float _speed=1;
    private float _defaultSpeed=1;
    [SerializeField, Tooltip("Ускорение")]
    private float _accelaretion=0.5f;
    [SerializeField, Tooltip("Максимальная скорость полета")]
    private float _maxSpeed=5;

    private Coroutine _coroutine;
    private Vector3 _positionDefault;
    private Vector3 _directionDefault;
    private Transform _transform;
    [SerializeField]
    private Vector3 _direction;

    [SerializeField, Tooltip("Следование за игроком")]
    private bool _isFollowing = true;
    public bool IsFollowing => _isFollowing;
    [SerializeField, Tooltip("Какому игроку принадлежит мяч")]
    private Players _player=0;
    public Players Player => _player;

    private void Awake()
    {
        _defaultSpeed = _speed;
           _transform = GetComponent<Transform>();
    }
    public void Fire()
    {
        _isFollowing = false;
        if (_coroutine == null)
            _coroutine = StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        while (true)
        {
            yield return null;  
            _transform.position += _direction * Time.deltaTime * _speed;
        }
    }
    public void Folowing(Vector3 folowing)
    {
        _transform.position = folowing;
    }
    public void Reset()
    {
        ResetBall(_positionDefault, _directionDefault);
    }
    public void ResetBall(Vector3 position,Vector3 direction)
    {
        _isFollowing = true;
        _speed = _defaultSpeed;
        _coroutine=null;
        _positionDefault = this.transform.position = position;
        _directionDefault =_direction = direction;
        if (_coroutine != null)
            StopCoroutine(_coroutine);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_maxSpeed > _speed)
            _speed += _accelaretion;
        else
            _speed = _maxSpeed;
        Vector3 upBlock = collision.gameObject.transform.up;
        Vector3 rightBlock = collision.gameObject.transform.right; 
        Vector3 forwardBlock = collision.gameObject.transform.forward;

        Vector3 upForward = Vector3.Reflect(_direction, upBlock);
        Vector3 rightForward = Vector3.Reflect(_direction, rightBlock);
        Vector3 forwardForward = Vector3.Reflect(_direction, forwardBlock);

       // Debug.Log(collision.gameObject.name);   
        if (collision.gameObject.GetComponent<VWall>())
        {
            _direction = upForward;
        }
        if (collision.gameObject.GetComponent<HWall>())
        {
            _direction = rightForward;
        }
        if (collision.gameObject.GetComponent<Block>())
        {           
            collision.gameObject.SetActive(false);
            _direction = rightForward+ upForward;
            if (LevelManager.instance.CheckLevel())
                Reset();

        }
        if (collision.gameObject.GetComponent<PlayerControler>())
        {
            _player = collision.gameObject.GetComponent<PlayerControler>().Player;
            _direction = forwardForward;
        }
        if (collision.gameObject.GetComponent<Gate>())
        {
            FindObjectOfType<HPManager>().setHP();
            Vector3 position = new Vector3();
            Vector3 direction = new Vector3();
            _player = collision.gameObject.GetComponent<Gate>().Players;
            switch (_player)
            {
                case 0:
                    position.z = 2.1f;
                    direction=Vector3.forward;
                    break;
                    case (Players)1:
                    position.z = -2.1f;
                    direction = -Vector3.forward;
                    break;

            }
            ResetBall(collision.gameObject.transform.position + position, direction);
            return;
        }
        _direction = _direction.normalized;
    }
}
