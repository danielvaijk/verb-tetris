using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Game : MonoBehaviour
{
    /// <summary> Global Scope Variables </summary>

    public string[] verbInfoArray, currentVerbInfo; // Verbo (Infinitivo), Verbo (Conjugado), Pessoa, Numero, Tempo, Modo;

    public bool canSpawn = true;

    public int score;

    public float globalSpeed;

    public Color32 incorrectColor = new Color32();

    public GameObject block;
    public Transform spawn, ray;

    public GUISkin skin;

    private int letterNumber, lastSetNumber, verbNumber;
    private int completed;

    [HideInInspector]
    public int setNumber = 0;
    [HideInInspector]
    public int curVertShift = 0;

    private string afterVerb = "";
    private string result = "";
    private string fixedVelocity = "";

    ArrayList tets = new ArrayList();

    /// <summary> Unity Standard Methods </summary>

    void Start ()
    {
        lastSetNumber = setNumber;
        verbInfoArray = RandomizeVerbArray(verbInfoArray); // Random array of verbInfo's;
        currentVerbInfo = verbInfoArray[verbNumber].Split('.');
        afterVerb = RandomizeVerb(currentVerbInfo[1].ToCharArray());

        NewBlock();
    }

    void OnGUI ()
    {
        GUI.skin = skin;
        fixedVelocity = new string (globalSpeed.ToString().ToCharArray(0, 3));

        GUI.BeginGroup(new Rect(Screen.width - 210, 20, 200, 500));

        GUILayout.Label("Pontos: " + score);
        GUILayout.Label("Velocidade: " + fixedVelocity);
        GUILayout.Label("Completos: " + completed);

        GUILayout.Space(40);

        GUILayout.Label("<color=#B9D3EE>INFORMACAO:</color>");

        GUILayout.Space(20);

        GUILayout.Label("Verbo: " + currentVerbInfo[0]);
        GUILayout.Label("Pessoa: " + currentVerbInfo[2]);
        GUILayout.Label("Numero: " + currentVerbInfo[3]);
        GUILayout.Label("Tempo: " + currentVerbInfo[4]);
        GUILayout.Label("Modo: " + currentVerbInfo[5]);

        GUI.EndGroup();

        GUI.BeginGroup(new Rect(Screen.width - 710, -255, 200, 500));

        GUILayout.Label("<color=#B9D3EE>MENU:</color>");

        GUILayout.Space(20);

        GUILayout.Label("Esquerda: Q");
        GUILayout.Label("Direita: E");
        GUILayout.Label("Sair: F1");

        GUI.EndGroup();
    }

    void Update ()
    {
        if (tets.Count > 0)
        {
            foreach (Cube.TetrimoShift tet in tets)
            {
                if (!tet.trueVert)
                {
                    ArrayList nonOccupied = GetNonOccupiedShifts();
                    int rand = Random.Range(0, nonOccupied.Count);

                    Cube gTet = tet.me.GetComponent<Cube>();

                    gTet.enabled = true;
                    gTet.GetComponent<Cube>().shift = (int)nonOccupied[rand];
                    gTet.GetComponent<Cube>().verticalShift = curVertShift;
                    tets.Remove(tet);
                    gTet.Placed();
                    setNumber--;
                    gTet.enabled = false;

                    nonOccupied.Remove(nonOccupied[rand]);

                    foreach (int shift in nonOccupied)
                        Debug.Log("Non Occupied: " + shift);

                    for (int i = 0; i < tets.Count; i++)
                        Debug.Log("TetElement: " + (tets.ToArray().GetValue(i) as Cube.TetrimoShift).tetrimo + " at " + i);
                }
            }
        }

        // If the amount of letters from a verb set is it's own length:
        if (setNumber == currentVerbInfo[1].Length)
        {
            // If we haven't finished spawning the total amount of verbs:
            if (verbNumber != verbInfoArray.Length - 1) // Undynamicly correct.
            {
                canSpawn = false;
                string[] finalList = new string[5];

                // If the vertical order of the verb is correct:

                Debug.Log("BEFORE FOOR LOOP: " + tets.Count);

                for (int i = -2; i < tets.Count - 2; i++)
                    finalList[i + 2] = GetNumTetrimo(i.ToString()).tetrimo;

                foreach (string s in finalList)
                    result += s;

                // If the horizontal order of the verb is correct:
                if (result == currentVerbInfo[1])
                {
                    completed++; // Info
                    score += 10; // Info
                    globalSpeed += 0.110F;

                    ray.SendMessage("DestroyRow", SendMessageOptions.RequireReceiver);

                    Debug.Log("CORRECT -> Vert: " + curVertShift);
                }
                else
                {
                    curVertShift++;

                    if (curVertShift == 4)
                        SceneManager.LoadScene(3);
                    else
                        ray.SendMessage("UpdateShift", SendMessageOptions.RequireReceiver);

                    foreach (GameObject cube in GameObject.FindGameObjectsWithTag("Letter"))
                        if (cube.GetComponent<Cube>().isSet)
                            cube.GetComponentInChildren<TextMesh>().color = incorrectColor;

                    Debug.Log("INCORRECT -> Vert: " + curVertShift);
                }

                tets.Clear();
                letterNumber = 0;
                result = string.Empty;

                canSpawn = true;
                letterNumber = setNumber = 0;
                verbNumber++;
                currentVerbInfo = verbInfoArray[verbNumber].Split('.');
                afterVerb = RandomizeVerb(currentVerbInfo[1].ToCharArray()); // The newly randomed verb;
            }
            else
            {
                SceneManager.LoadScene(2);
            }
        }

        if (lastSetNumber != setNumber)
        {
            NewBlock();
            lastSetNumber = setNumber;
        }

        if (setNumber == 5 && curVertShift == 3)
            SceneManager.LoadScene(3);

        if (Input.GetKeyDown(KeyCode.F1))
            SceneManager.LoadScene(0);
    }

    /// <summary> Custom Methods </summary>

    string RandomizeVerb (char[] verb)
    {
        int size = verb.Length;

        for (int i = 0; i < size; i++)
        {
            int indexToSwap = UnityEngine.Random.Range(i, size);

            char oldValue = verb[i];
            verb[i] = verb[indexToSwap];
            verb[indexToSwap] = oldValue;
        }

        return new string(verb);
    }

    string[] RandomizeVerbArray (string[] array)
    {
        int size = array.Length;

        for (int i = 0; i < size; i++)
        {
            int indexToSwap = UnityEngine.Random.Range(i, size);

            string oldValue = array[i];
            array[i] = array[indexToSwap];
            array[indexToSwap] = oldValue;
        }

        return array;
    }

    void NewBlock ()
    {
        if (canSpawn)
        {
            GameObject newCube = Instantiate(block, spawn.position, Quaternion.identity) as GameObject;
            newCube.GetComponentInChildren<TextMesh>().text = "[" + afterVerb[letterNumber].ToString().ToUpper() + "]";
            newCube.name = afterVerb[letterNumber].ToString();

            if (letterNumber <= currentVerbInfo[1].Length) letterNumber++;
            else tets.RemoveRange(0, tets.Count - 1);
        }
    }

    void SetTetrimo (Cube.TetrimoShift receivedTet)
    {
        tets.Insert(tets.Count, receivedTet);
        score += 5;

        foreach (Cube.TetrimoShift tet in tets)
            Debug.Log("[HShift: " + tet.shift + "] [VShift: " + tet.vertShift + "->" + curVertShift +  "] [Tetrimo: " + tet.tetrimo + "] at " + tets.IndexOf(tet, 0, tets.Count));
    }

    Cube.TetrimoShift GetNumTetrimo (string number)
    {
        foreach (Cube.TetrimoShift ts in tets)
            if (ts.shift.ToString() == number)
                return ts;

        return null;
    }

    ArrayList GetNonOccupiedShifts ()
    {
        ArrayList testShifts = new ArrayList();

        for (int i = -2; i < 3; i++)
            testShifts.Add(i);

        foreach (Cube.TetrimoShift tet in tets)
            testShifts.Remove(tet.shift);

        return testShifts;
    }
}