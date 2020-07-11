using Model;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class AIManager : MonoBehaviour {

    private Sorter sorter;
    private Tiles tiles;

    public NavMeshAgent agent;
    public GameObject idol;
    public Toggle mode;

    void Awake()
    {
        sorter = GameObject.Find("Unit").GetComponent<Sorter>();
        tiles = GameObject.Find("Tiles").GetComponent<Tiles>();
    }

    void Update ()
    {
        var pos = agent.gameObject.transform.position;
        idol.transform.position = new Vector3(pos.x, 0,pos.z);
        var unit = idol.GetComponent<Character>();
        var tile = tiles.GetTileByPoint(idol.transform.position);


        if (unit.origin != tile)
        {
            unit.UpdateTile(tile);
            sorter.SortUnit(unit);
        }

        //좌우 변경
      //  idol.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = agent.gameObject.transform.eulerAngles.z < 180f;


        //edit모드가 아닐때 클릭시 이동
        if (!mode.isOn)
        {
             //RaycastHit hit;
            if (Input.GetMouseButtonUp(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                var hits = Physics.RaycastAll(ray, Mathf.Infinity);

              //  if (Physics.Raycast(ray,out hit, 100))
              //  {
              //      agent.destination = hit.point;
              //  }
                
                foreach (var hit in hits)
                {
                    var selectedTile = hit.transform.gameObject.GetComponent<Tile>();
                    if (selectedTile != null)
                    {
                        var position = selectedTile.gameObject.transform.position;
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
