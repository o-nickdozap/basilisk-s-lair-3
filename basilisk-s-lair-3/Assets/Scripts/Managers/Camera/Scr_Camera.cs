using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Scr_Camera : MonoBehaviour
{
    GameObject playerObject;
    public float damping;
    public float xMin, xMax, yMin, yMax;
    bool unlockY;

    void Start()
    {
        playerObject = GameObject.Find("Player");
        transform.position = new Vector3(playerObject.transform.position.x, playerObject.transform.position.y, transform.position.z);
    }

    void FixedUpdate()
    {

        float targetX = Mathf.Lerp(transform.position.x, playerObject.transform.position.x, damping);
        float targetY = Mathf.Lerp(transform.position.y, playerObject.transform.position.y, damping);

        transform.position = new Vector3(Mathf.Clamp(targetX, xMin, xMax), transform.position.y, transform.position.z);
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(targetY, yMin, yMax), transform.position.z);

    }
}
