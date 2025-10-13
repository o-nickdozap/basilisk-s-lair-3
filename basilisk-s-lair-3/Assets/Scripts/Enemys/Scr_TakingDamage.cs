using UnityEngine;

public class Scr_TakingDamage : MonoBehaviour
{
    [SerializeField] float _molhoLife;

    private Rigidbody2D _rig;

    private GameObject _playerObject;
    private Vector2 _playerPos;

    [SerializeField] float _knockForceX, _knockForceY;

    void Start()
    {
        _rig = this.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (_playerObject != null)
        {
            _playerPos = this.transform.position - _playerObject.transform.position;
        }
        else
        {
            _playerObject = GameObject.FindGameObjectWithTag("Player");
        }

        if (_molhoLife <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Damage(int Damage)
    {
        _molhoLife -= Damage;
        KnockBack();
    }

    public void KnockBack()
    {
        _rig.linearVelocity = new Vector2(Mathf.Sign(_playerPos.x) * _knockForceX, Mathf.Sign(_playerPos.y) * _knockForceY);
    }
}
