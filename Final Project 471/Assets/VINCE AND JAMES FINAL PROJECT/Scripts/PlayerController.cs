using UnityEngine;

public class PlayerController : MonoBehaviour
{
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Falldoor"))
        {
            // Optionally, you can check if the player is still pressing "O"
            if (Input.GetKeyDown(KeyCode.O))
            {
                Debug.Log("Player is touching the Falldoor and pressed O.");
            }
        }
    }
}
