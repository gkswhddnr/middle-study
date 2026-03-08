using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public Text playerStateText;
    public void OnPlayerDead()
    {
        playerStateText.text = "You die";
    }

}
