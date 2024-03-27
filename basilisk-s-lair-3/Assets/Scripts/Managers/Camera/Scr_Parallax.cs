using UnityEngine;

public class Scr_Parallax : MonoBehaviour
{

    //private float length;
    private float StartPosX;
    private float StartPosY;

    Transform Cam;

    public float ParallaxEffectX;
    public float ParallaxEffectY;

    void Start()
    {
        Cam = GameObject.Find("Main Camera").transform;

        StartPosX = transform.position.x;
        StartPosX = transform.position.y;
    }

    void FixedUpdate()
    {

        float DistanceX = Cam.transform.position.x * ParallaxEffectX;
        float DistanceY = Cam.transform.position.y * ParallaxEffectY;

        transform.position = new Vector3(StartPosX + DistanceX, transform.position.y, transform.position.z);
        transform.position = new Vector3(transform.position.x, StartPosY + DistanceY, transform.position.z);

    }
}
