using UnityEngine;

public class ApplicationQuitScript : MonoBehaviour
{
    public KeyCode quitKey = KeyCode.Escape;

    private void Update()
    {
        if (Input.GetKeyDown(quitKey))
        {
            Application.Quit();
        }
    }
}