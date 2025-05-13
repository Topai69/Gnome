using UnityEngine;

public class MinimapCameraLimit : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject Player;
    private void LateUpdate()
    {
        transform.position = new Vector3(Player.transform.position.x, 50, Player.transform.position.z);

        
    }
}
