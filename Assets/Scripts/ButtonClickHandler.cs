using UnityEngine;
using UnityEngine.UI;

public class ButtonClickHandler : MonoBehaviour
{
    /// <summary>
    ///   React to a button click event.  Used in the UI Button action definition.
    /// </summary>
    /// <param name="button"></param>
    public void onButtonClicked(Button button)
    {
        // which GameObject?
        GameObject go = GameObject.Find("GameController");
        if (go != null)
        {
            Home gameController = go.GetComponent<Home>();
            if (gameController == null)
            {
                Debug.LogError("Missing game controller...");
                return;
            }
            if (button.name == "Join")
            {
                gameController.onJoinButtonClicked();
            }
            else if (button.name == "Leave")
            {
                gameController.onLeaveButtonClicked();
            }
        }
    }
}
