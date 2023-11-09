using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FrameShaper : MonoBehaviour
{
    public int count;

    public Text text;

    public string borderText = "<!                         !>";
    public string semiText = "<!=========================!>";
    public string lastText = @"  /\/\/\/\/\/\/\/\/\/\/\/\/\  ";

    void Start ()
    {
        text.text = borderText;

        for (int i = 0; i < count; i++)
        {
            if (i < count - 2) { text.text = borderText; }
            else if (i == count - 2) { text.text = semiText; }
            else if (i == count - 1) { text.text = lastText; }

            Text part = Instantiate(text, new Vector2(0.5F, 0.52F - (0.03F * i)), Quaternion.identity) as Text;
            part.transform.parent = transform;
        }
    }
}
