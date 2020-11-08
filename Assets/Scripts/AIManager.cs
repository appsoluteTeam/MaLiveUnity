using Model;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class AIManager : MonoBehaviour {

    public NavMeshAgent agent;
    public GameObject idol;
    public Toggle mode;
    private float mZCoord;
    private Vector3 mOffset;

    void Awake()
    {
    }

    void Update ()
    {
        var pos = agent.gameObject.transform.position;
        idol.transform.position = new Vector3(pos.x, 0,pos.z);


        if (!mode.isOn)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Input.GetMouseButtonUp(0))
            {
                if (Physics.Raycast(ray, out hit, 100))
                {
                    agent.destination = hit.point;
                    Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(idol.transform.position);
                    Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);
                    float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);
                    idol.transform.rotation = Quaternion.Euler(new Vector3(-90f, 0f, angle));
                }
            }
        }
        float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
        {
            return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
        }

    }

    private Vector3 GetMouseAsWorldPoint()
    {
        // Pixel coordinates of mouse (x,y)
        Vector3 mousePoint = Input.mousePosition;

        // z coordinate of game object on screen
        mousePoint.z = mZCoord;

        // Convert it to world points
        return Camera.main.ScreenToWorldPoint(mousePoint);

    }
}
