using UnityEngine;

public class Scr_ItemRotation : MonoBehaviour
{
    [SerializeField] float _rotationX, _rotationY, _rotationZ;

    void Update()
    {
        transform.Rotate(_rotationX * Time.deltaTime, _rotationY * Time.deltaTime, _rotationZ * Time.deltaTime, Space.Self);
    }
}
