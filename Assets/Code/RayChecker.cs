using UnityEngine;
using System.Collections;

public class RayChecker : MonoBehaviour
{
    public float distance;

    public RaycastHit[] hits;

    private RaycastHit hit;

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.TransformDirection(Vector3.right));
        hits = Physics.RaycastAll(ray, distance);

        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * distance, Color.red);
    }

    // SendMessage()
    void UpdateShift ()
    {
        transform.Translate(new Vector3(0.0F, 0.2F, 0.0F));
    }

    // SendMesage()
    void DestroyRow ()
    {
        foreach (RaycastHit hit in hits)
            if (hit.collider.GetComponent<Cube>().isSet)
                Destroy(hit.collider.gameObject);

        GameObject.Find("Main Camera").GetComponent<Game>().score += 10;
    }
}
