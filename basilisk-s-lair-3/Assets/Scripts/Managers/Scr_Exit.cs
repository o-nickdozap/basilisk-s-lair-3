using UnityEngine;
using UnityEngine.SceneManagement;

public class Scr_Exit : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D _col)
    {
        if (_col.collider.CompareTag("Player")) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
            Debug.Log("seila");
        }
    }
}
