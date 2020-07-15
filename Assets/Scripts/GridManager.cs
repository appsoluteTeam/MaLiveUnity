using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Model;

public class GridManager : MonoBehaviour {

    
    private Furniture SelectedFurniture;

    private bool dragging = false;

    public Transform interactBtnGroup;
    public Button placeButton;
    public Button rotateButton;
	public Button undoButton;
    public Toggle mode;
    private float mZCoord;
    private Vector3 mOffset;

    public Button addButton_test;
    public GameObject sample_object; 
    
    private float planeY;


    void Awake ()
    {
    }

    void Start () {
        placeButton.onClick.AddListener(() => {
			OnPlaceFurniture(SelectedFurniture);
            interactBtnGroup.gameObject.SetActive(false);
        });
        rotateButton.onClick.AddListener(() => {
  //              RotateItem(out area);
        });
        undoButton.onClick.AddListener(() => {
            OnUndo(SelectedFurniture);
			interactBtnGroup.gameObject.SetActive(false);
		});
        addButton_test.onClick.AddListener(() =>
        {
            AddNewFurniture(0);
        });
      //  mode.onValueChanged.AddListener(value => grids.enabled = value);
    }
	
	void Update () {
        if (!mode.isOn)
            return;
        mode.interactable = SelectedFurniture == null;

        if (Input.GetMouseButtonDown(0))
        {
            OnBeginDrag(isHold => dragging = isHold);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            dragging = false;
            OnEndDrag();
        }

        if (dragging)
        {
            OnDrag();
        }
    }

    private void OnBeginDrag(Action<bool> isHold) {
        if (SelectedFurniture == null)
        {
            var furniture = OnSelect(child => child.transform.parent.GetComponent<Furniture>() != null);
            if (furniture != null)
            {
                SelectedFurniture = furniture.GetComponent<Furniture>();
                SelectedFurniture.Unplaced();
            }
            isHold(furniture != null);
        }
        else
        {
            var furniture = OnSelect(child => child.transform.parent.GetComponent<Furniture>() != null);
            isHold(furniture != null && furniture.transform.GetComponent<Furniture>() == SelectedFurniture);
        
        }
    
    }

    private void OnDrag()
    {
        if (SelectedFurniture == null)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        float distance; // the distance from the ray origin to the ray intersection of the plane
        planeY = SelectedFurniture.transform.localScale.y/2;
        Plane plane = new Plane(Vector3.up, Vector3.up * planeY); // ground plane
        if (plane.Raycast(ray, out distance))
        {
            SelectedFurniture.Move(ray.GetPoint(distance));
        }
        interactBtnGroup.gameObject.SetActive(false);
        
    }

    private void OnEndDrag()
    {
        if (SelectedFurniture == null)
            return;

        var centerPoint = Camera.main.WorldToScreenPoint(SelectedFurniture.transform.position);
        interactBtnGroup.position = centerPoint;
        interactBtnGroup.gameObject.SetActive(true);
        //TODO: colider 충돌시 false값 출력
        placeButton.interactable = true;
        undoButton.interactable = SelectedFurniture.previous != null;
    }

    private void RotateItem()
    {
		if (SelectedFurniture != null)
        {
            SelectedFurniture.Rotate();
            //TODO: colider 충돌시 false값 출력
            placeButton.interactable = true;
        }
    }

    private GameObject OnSelect(Predicate<GameObject> condition)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.transform.gameObject.GetComponent<Furniture>() != null)
            {
                Debug.Log(hit.transform.gameObject.name);
                return hit.transform.gameObject;
            }
            
        }
        return null;
    }

    private void OnPlaceFurniture(Furniture furniture)
    {
        if (furniture == null)
            return;
            furniture.Place();
            furniture.SetColor(Color.white);
            SelectedFurniture = null;
        }
 
  /*  private bool OnInvalid(Furniture furniture, out List<Tile> area)
    {
        area = new List<Tile>();
        if (furniture.direction == Direction.North || furniture.direction == Direction.East)
        {
            for (int i = 0; i < furniture.width; i++)
            {
                for (int j = 0; j < furniture.length; j++)
                {
                    var tile = tiles.GetTileByCoordinate(furniture.origin.x - j, furniture.origin.z - i);
                    if (tile == null || tile.isBlock)
                    {
                        furniture.SetColor(Color.red);
                        return true;
                    }

                    area.Add(tile);
                }
            }
        }
        else
        {
            for (int i = 0; i < furniture.width; i++)
            {
                for (int j = 0; j < furniture.length; j++)
                {
                    var tile = tiles.GetTileByCoordinate(furniture.origin.x + j, furniture.origin.z + i);
                    if (tile == null || tile.isBlock)
                    {
                        furniture.SetColor(Color.red);
                        return true;
                    }

                    area.Add(tile);
                }
            }
        }

        furniture.SetColor(Color.green);
        return false;
    }

    */

	private void OnUndo (Furniture furniture)
	{
        if (furniture.previous == null)
            return;

        furniture.Move (furniture.previous.pos);
        furniture.Rotate (furniture.previous.direction);
        OnPlaceFurniture(furniture);
    }

    private void AddNewFurniture(int furniture_id)
    {
        GameObject temp = Instantiate(sample_object, new Vector3(1, sample_object.transform.localScale.y/2, 1), Quaternion.identity);

        temp.transform.parent = GameObject.Find("Unit").transform;
        OnPlaceFurniture(temp.GetComponent<Furniture>());


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