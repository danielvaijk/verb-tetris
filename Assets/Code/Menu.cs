using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour
{
    public GUISkin skin;
    public GUIStyle titleStyle, labelStyle;
    public AudioClip clickSound;

    private string click = "";

    void Awake ()
    {
        Screen.SetResolution(800, 600, false);
    }

    void OnGUI ()
    {
        GUI.skin = skin;

        if (click == "") MainMenu();
        else if (click == "creditos") Credits();
        else if (click == "regras") Rules();
    }

    void Update ()
    {
        GetComponent<Camera>().aspect = 8.0F / 6.0F;

        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (click == "") Application.LoadLevel(1);
            else if (click == "creditos" || click == "regras") click = "";

            GetComponent<AudioSource>().PlayOneShot(clickSound);
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            click = "creditos";
            GetComponent<AudioSource>().PlayOneShot(clickSound);
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            click = "regras";
            GetComponent<AudioSource>().PlayOneShot(clickSound);
        }

        if (Input.GetKeyDown(KeyCode.F4))
            Application.Quit();
    }

    void MainMenu ()
    {
        GUILayout.Label("Tetris de Gramatica", titleStyle);

        GUI.BeginGroup(new Rect(Screen.width / 2 - 75, Screen.height - 520, 400, 300));

        GUILayout.Label("Jogar: F1", labelStyle);
        GUILayout.Label("Creditos: F2", labelStyle);
        GUILayout.Label("Regras: F3", labelStyle);
        GUILayout.Label("Sair: F4", labelStyle);

        GUI.EndGroup();
    }

    void Credits ()
    {
        GUI.Label(new Rect(100, -10, 100, 100), "<color=#2D5953>Creditos</color>", titleStyle);

        GUI.BeginGroup(new Rect(Screen.width / 2 - 320, Screen.height - 450, 710, 300));

        GUILayout.Label("<color=#28B016>Codigo</color>|<color=#28B016>Ideias</color>|<color=#28B016>Debug:</color> Daniel Van Dijk,");
        GUILayout.Label("<color=#28B016>Testes</color>|<color=#28B016>Ideias</color>|<color=#28B016>Debug:</color> Augusto Seixas,");
        GUILayout.Label("<color=#28B016>Verbos</color>|<color=#28B016>Debug:</color> Vagner Humberto Wentz,");
        GUILayout.Label("<color=#28B016>Apresentacao:</color> Daniel de Melo,");
        GUILayout.Label("<color=#28B016>Apresentacao:</color> Kelvin Henrique.");

        GUILayout.Space(70);

        GUILayout.BeginHorizontal();
        GUILayout.Space(230);
        GUILayout.Label("Voltar: F1");
        GUILayout.EndHorizontal();

        GUI.EndGroup();
    }

    void Rules ()
    {
        GUI.Label(new Rect(100, -10, 100, 100), "<color=#2D5953>Regras</color>", titleStyle);

        GUI.BeginGroup(new Rect(Screen.width / 2 - 395, Screen.height - 450, 750, 300));

        GUILayout.Label("1. Alinhe os verbos corretamente;");
        GUILayout.Label("2. Os verbos devem estar na horizontal;");
        GUILayout.Label("3. A cada erro, o verbo errado permanecera;");
        GUILayout.Label("4. Evite acumular mais de 4 verbos.");

        GUI.EndGroup();

        GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2, 20, 10), "Voltar: F1", labelStyle);
    }
}
