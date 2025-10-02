using UnityEngine;

public class Scr_Parallax : MonoBehaviour
{
    private float StartPosX;
    private float StartPosY;

    Transform Cam;

    public float ParallaxEffectX;
    public float ParallaxEffectY;

    void Start()
    {
        Cam = GameObject.FindWithTag("MainCamera").transform;

        StartPosX = transform.position.x;
        StartPosX = transform.position.y;
    }

    void FixedUpdate()
    {
        if (Cam != null)
        {
            Parallax();
        }
        else
        {
            Cam = GameObject.FindWithTag("MainCamera").transform;
        }
    }

    void Parallax()
    {
        float DistanceX = Cam.transform.position.x * ParallaxEffectX;
        float DistanceY = Cam.transform.position.y * ParallaxEffectY;

        transform.position = new Vector3(StartPosX + DistanceX, transform.position.y, transform.position.z);
        transform.position = new Vector3(transform.position.x, StartPosY + DistanceY, transform.position.z);
    }
}
