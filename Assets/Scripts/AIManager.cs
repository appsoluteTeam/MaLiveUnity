using Model;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class AIManager : MonoBehaviour {

    public NavMeshAgent agent;
    public GameObject idol;
    public Toggle mode;

    void Awake()
    {
    }

    void Update ()
    {
        var pos = agent.gameObject.transform.position;
        idol.transform.position = new Vector3(pos.x, 0,pos.z);
        var unit = idol.GetComponent<Character>();

        if (!mode.isOn)
        {
            if (Input.GetMouseButtonUp(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                var hits = Physics.RaycastAll(ray, Mathf.Infinity);
                
                foreach (var hit in hits)
                {
                    var selectedObject= hit.transform.gameObject;
                    if (selectedObject != null)
                    {
                        var position = selectedObject.gameObject.transform.position;
                        agent.SetDestination(new Vector3(position.x, 0, position.z));
                        Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(idol.transform.position);
                        Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);
                        float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);
                        idol.transform.rotation = Quaternion.Euler(new Vector3(-90f, 0f, angle));

                    }
                }
                
            }
        }
        float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
        {
            return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
        }

    }
}
