using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Final : MonoBehaviour
{
    public Type resultType;
    public GUIStyle style;

    public enum Type
    {
        won,
        lost
    }

    void OnGUI ()
    {
        if (resultType == Type.won) WinWindow();
        else if (resultType == Type.lost) LooseWindow();
    }

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.F1))
            SceneManager.LoadScene(1);
        if (Input.GetKeyDown(KeyCode.F2))
            SceneManager.LoadScene(0);
    }

    void WinWindow ()
    {
        GUILayout.Space(10);

        GUI.Label(new Rect(Screen.width / 2 - 200, Screen.height / 2 - 100, 100, 100), "Voce venceu esta rodada! Parabens!", style);
        GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 30, 100, 100), "Tentar denovo: F1", style);
        GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2, 100, 100), "Voltar ao Menu: F2", style);
    }

    void LooseWindow ()
    {
        GUILayout.Space(10);

        GUI.Label(new Rect(Screen.width / 2 - 250, Screen.height / 2 - 100, 100, 100), "Voce perdeu esta rodada... Talvez na proxima!", style);
        GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 30, 100, 100), "Tentar denovo: F1", style);
        GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2, 100, 100), "Voltar ao Menu: F2", style);
    }
}
