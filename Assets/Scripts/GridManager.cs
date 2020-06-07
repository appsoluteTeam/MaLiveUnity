using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Model;

public class GridManager : MonoBehaviour {

    private Tiles tiles;
    private Sorter sorter;

    private Furniture SelectedFurniture;

    private bool dragging = false;

    public Transform interactBtnGroup;
    public Button placeButton;
    public Button rotateButton;
	public Button undoButton;
    public Toggle mode;


    private float mZCoord;
    private Vector3 mOffset;

    void Awake ()
    {
        sorter = GameObject.Find("Unit").GetComponent<Sorter>();
        tiles = GameObject.Find("Tiles").GetComponent<Tiles>();
    }

    void Start () {
        placeButton.onClick.AddListener(() => {
			OnPlaceFurniture(SelectedFurniture);
            sorter.SortAll();
            interactBtnGroup.gameObject.SetActive(false);
        });
        rotateButton.onClick.AddListener(() => {
                List<Tile> area;
                RotateItem(out area);
        });
        undoButton.onClick.AddListener(() => {
            OnUndo(SelectedFurniture);
            sorter.SortAll();
			interactBtnGroup.gameObject.SetActive(false);
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

        var tile = OnSelectTile(obj => obj.GetComponent<Tile>() != null);
        if (tile != null)
        {
            interactBtnGroup.gameObject.SetActive(false);
            SelectedFurniture.Move(tile.GetComponent<Tile>());

            List<Tile> area;
            OnInvalid(SelectedFurniture, out area);
        }
    }

    private void OnEndDrag()
    {
        if (SelectedFurniture == null)
            return;

        var centerPoint = Camera.main.WorldToScreenPoint(SelectedFurniture.transform.position);
        interactBtnGroup.position = centerPoint;
        interactBtnGroup.gameObject.SetActive(true);

        List<Tile> area;
        placeButton.interactable = !(OnInvalid(SelectedFurniture, out area));
        undoButton.interactable = SelectedFurniture.previous != null;
    }

    private void RotateItem(out List<Tile> area)
    {
        area = new List<Tile>();
		if (SelectedFurniture != null)
        {
            SelectedFurniture.Rotate();
			placeButton.interactable = !(OnInvalid(SelectedFurniture, out area));
        }
    }

    private GameObject OnSelect(Predicate<GameObject> condition)
	{	
        //var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
       // var hits = Physics.RaycastAll(ray, Mathf.Infinity);

      //  foreach (var hit in hits)
       //     if (condition(hit.transform.gameObject))
      //      {
      //          Debug.Log("hoelo");
      //          return hit.transform.gameObject;
     //       }
     //   return null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Debug.Log("OnSelected");

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

    private GameObject OnSelectTile(Predicate<GameObject> condition)
    {
        //var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // var hits = Physics.RaycastAll(ray, Mathf.Infinity);

        //  foreach (var hit in hits)
        //     if (condition(hit.transform.gameObject))
        //      {
        //          Debug.Log("hoelo");
        //          return hit.transform.gameObject;
        //       }
        //   return null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.transform.gameObject.GetComponent<Tile>() != null)
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

        List<Tile> area;
        if (!OnInvalid(furniture, out area))
        {
            furniture.Place(area);
            furniture.SetColor(Color.white);
            SelectedFurniture = null;
        }
    }

    private bool OnInvalid(Furniture furniture, out List<Tile> area)
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

	private void OnUndo (Furniture furniture)
	{
        if (furniture.previous == null)
            return;

        furniture.Move (furniture.previous.tile);
        furniture.Rotate (furniture.previous.direction);
        OnPlaceFurniture(furniture);
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
