using UnityEngine;
using System.Collections;

public class Cube : MonoBehaviour
{
    /// <summary> Global Scope Variables </summary>

    public int shift = 0; // -2, -1, 0, 1, 2
    public int verticalShift = 0;

    public bool isSet = false; // RayChecker

    public AudioClip movement;

    private float[] yPos = { -1.15F, -0.95F, -0.75F, -0.554F, -0.355F };

    private float speed;

    private GameObject cam;
    private Game g;

    public class TetrimoShift
    {
        public int shift, vertShift;
        public string tetrimo;
        public bool trueVert;
        public GameObject me;
    }

    /// <summary> Unity Standard Methods </summary>

    void Start()
    {
        cam = GameObject.Find("Main Camera");
        g = cam.GetComponent<Game>();
        speed = g.globalSpeed;
    }

    void Update ()
    {
        Ray downRay = new Ray(transform.position, transform.TransformDirection(Vector3.down));
        Ray leftRay = new Ray(transform.position - new Vector3(0.0F, 0.15F, 0.0F), transform.TransformDirection(Vector3.left));
        Ray rightRay = new Ray(transform.position - new Vector3(0.0F, 0.15F, 0.0F), transform.TransformDirection(Vector3.right));

        if (!Physics.Raycast(downRay, 0.15F))
            transform.position += new Vector3(0.0F, -speed, 0.0F) * Time.deltaTime;
        else Placed();

        if (transform.position.y >= -0.4F) verticalShift = 4;
        else if (transform.position.y >= -0.6F) verticalShift = 3;
        else if (transform.position.y >= -0.8F) verticalShift = 2;
        else if (transform.position.y >= -1.0F) verticalShift = 1;
        else  verticalShift = 0;

        if (!Physics.Raycast(leftRay, 0.2F))
            if (Input.GetKeyDown(KeyCode.Q) && shift > -2)
            {
                transform.Translate(Vector3.left * 0.30F);
                GetComponent<AudioSource>().PlayOneShot(movement);
                shift--;
            }

        if (!Physics.Raycast(rightRay, 0.2F))
            if (Input.GetKeyDown(KeyCode.E) && shift < 2)
            {
                transform.Translate(Vector3.right * 0.30F);
                GetComponent<AudioSource>().PlayOneShot(movement);
                shift++;
            }

        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * 0.16F, Color.red);
        Debug.DrawRay(transform.position - new Vector3(0.0F, 0.15F, 0.0F), transform.TransformDirection(Vector3.left) * 0.2F, Color.red);
        Debug.DrawRay(transform.position - new Vector3(0.0F, 0.15F, 0.0F), transform.TransformDirection(Vector3.right) * 0.2F, Color.red);
    }

    /// <summary> Custom Methods </summary>

    public void Placed ()
    {
        transform.position = new Vector3(-0.1F + (0.30F * shift), yPos[verticalShift], transform.position.z);
        isSet = true;

        TetrimoShift tet = new TetrimoShift();
        tet.shift = shift;
        tet.vertShift = verticalShift;
        tet.trueVert = (verticalShift == g.curVertShift);
        tet.tetrimo = name;
        tet.me = gameObject;

        g.setNumber++;
        cam.SendMessage("SetTetrimo", tet, SendMessageOptions.RequireReceiver);
        enabled = false;
    }
}
