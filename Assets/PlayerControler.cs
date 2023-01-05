using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControler : MonoBehaviour
{
    [SerializeField,Tooltip("Номер игрока")]
    private Players _player;
    public Players Player => _player;
    private PlayerInput _playerInput;
    private InputAction _playerActionMove;

    [SerializeField, Tooltip("Скорость движение")]
    private float _speed;

    [SerializeField, Tooltip("Префаб шара")]
    private Ball _ball;
    [SerializeField, Tooltip("Направление движение")]
    private Vector3 _direction=new();

    private bool isHWall = false, isVWall = false;

    private void Awake()
    {
        _playerInput = new();
        _ball = FindObjectOfType<Ball>();
        if (_player== Players.Player1)
        {
            _ball.ResetBall(transform.position + Vector3.forward * 1.1f, Vector3.forward);
        }
    }
    private void OnEnable()
    {
        switch (_player)
        {
            case Players.Player1:
                _playerInput.Player1.Enable();
                _playerActionMove = _playerInput.Player1.Move;
                _playerInput.Player1.Fire.performed += Fire_performed;
                break;
            case Players.Player2:
                _playerInput.Player2.Enable();
                _playerActionMove = _playerInput.Player2.Move;
                _playerInput.Player2.Fire.performed += Fire_performed;
                break;
        }
    }
    private void Update()
    {
        Move(_playerActionMove);
    }
    private void Fire_performed(InputAction.CallbackContext obj)
    {
        _ball.Fire();
    }

    private void OnDisable()
    {
        switch (_player)
        {
            case Players.Player1:
                _playerInput.Player1.Enable();
                _playerInput.Player1.Fire.performed -= Fire_performed;
                break;
            case Players.Player2:
                _playerInput.Player2.Enable();
                _playerInput.Player2.Fire.performed -= Fire_performed;
                break;
        }
    }

    void Move(InputAction Input)
    {
        var move = Input.ReadValue<Vector2>();
        if (move != Vector2.zero)
        {
            var tempx = isHWall ?
                (transform.position.x > 0 ?
                (move.x < 0 ? move.x : 0) :
                (move.x > 0 ? move.x : 0)
                ) :
                move.x; 
            var tempy = isVWall ?
                (transform.position.y > 0 ?
                (move.y < 0 ? move.y : 0) :
                (move.y > 0 ? move.y : 0)
                ) :
                move.y;
            _direction = new Vector3(tempx,tempy) * _speed;
        }
        else
            _direction = Vector3.Lerp(_direction, Vector3.zero, Time.deltaTime);
        transform.position += _direction * Time.deltaTime;

        if (_ball.IsFollowing&& _ball.Player==_player)
        {
            var posZ = Vector3.forward;
            switch (_player)
            {
                case 0:
                    posZ *= 1.1f;
                    break;
                case (Players)1:
                    posZ *= -1.1f;
                    break;

            }
            _ball.Folowing(transform.position+ posZ);
        }

    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<HWall>())
            isHWall=false;
        if (collision.gameObject.GetComponent<VWall>())
            isVWall = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<HWall>())
        {
            isHWall = true;
            _direction=new Vector3(0, _direction.y, _direction.z);
        }
        if (collision.gameObject.GetComponent<VWall>())
        {
            isVWall = true;
            _direction = new Vector3(_direction.x, 0, _direction.z);
        }
    }
}
 public enum Players
{
    Player1,Player2
}