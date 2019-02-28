using Custom.GameManager;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public void OnPressButton()
    {
        GameManager.StartGame();
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            OnPressButton();
        }
    }

}
