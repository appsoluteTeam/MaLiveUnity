using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;

namespace Model
{
    //동서남북
    public enum Direction : int
    {
        South,
        West,
        North,
        East,
    }

    //Base Unit 상속
    public class Furniture : BaseUnit
    {

        //각 방향에 대한 게임 오브젝트들
        //public GameObject[] ListDirectionItem = new GameObject[4];

        //south가 기본값
        private Direction direction = Direction.South;

        //이전 값? 잘 모르겠당
        public HistoricalData previous { get; private set; }

        private List<GameObject> blocks;

        private const string Unit_LAYER = "Unit";
        private const string PREVIEW_LAYER = "Preview";

        private GameObject pivot;

        public void Start()
        {
            pivot = transform.parent.gameObject;
        }

        //회전
        public void Rotate()
        {
            direction = (Direction)(((int)direction + 1) % 4);


            //foreach (var dir in ListDirectionItem)
            //    dir.SetActive(false);

            //gameobject들중 해당 direction만 남기고 나머지 setActive false
            //ListDirectionItem[(int)direction].SetActive(true);
            //가로 세로 조정
            pivot.transform.Rotate(0, 90, 0);
            var temp = width;
            width = length;
            length = temp;
        }


        //뭐하는 함수지
        public void Rotate(Direction dir)
        {
            //dir파라미터 - 현재 방향 의 절대값의 2로 나눈 몫이 1이면
            if (Mathf.Abs(dir - direction) % 2 == 1)
            {
                var temp = width;
                width = length;
                length = temp;
            }
            direction = dir;
            pivot.transform.Rotate(0, 90, 0);
//            foreach (var diritem in ListDirectionItem)
//                diritem.SetActive(false);

        //   ListDirectionItem[(int)dir].SetActive(true);
        }

        //게임오브젝트의 위치를 타일의 위치로 옮긴다.
        public void Move(Tile tile)
        {
            gameObject.transform.position = new Vector3(tile.gameObject.transform.position.x+length/2.0f-0.5f,tile.gameObject.transform.position.y+height/2
                ,tile.gameObject.transform.position.z+width/2.0f-0.5f);
           // pivot.transform.position = tile.gameObject.transform.position;
            origin = tile;
            Debug.Log(origin.name);
        }

        //각 방향별 게임오브젝트마다 색깔을 바꾸어줌
        public void SetColor(Color color)
        {
            gameObject.GetComponent<Renderer>().material.color = color;
        }


        public void Place(List<Tile> tiles)
        {
            base.tiles = tiles;
            //물체가 있는 타일을 block시킨다
            base.tiles.ForEach(tile => tile.isBlock = true);
            foreach(var tile in tiles)
            {
                Debug.Log("여기다"+tile.name);
            }

            //랜더링 시키기?
            //layer 호가정
            gameObject.GetComponent<Renderer>().sortingLayerName = Unit_LAYER;

            //취소했을때를 위하여
            previous = new HistoricalData(origin, direction);
            //create box collider in 3D.
          //  Block(tiles);
        }

        public void Unplaced()
        {
            Debug.Log("unplaced called");
            tiles.ForEach(tile => tile.isBlock = false);
            tiles = new List<Tile>();

            //PREVIEW로 옮김
            gameObject.GetComponent<Renderer>().sortingLayerName = PREVIEW_LAYER;

        //    UnBlock();

        }

        //이 함수는 무엇이냐? navigation을 위하여 물체에 큐브를 두어 캐릭터가 못지나가게 막아주는것
        private void Block(List<Tile> tiles)
        {
            blocks = new List<GameObject>();
            foreach (var tile in tiles)
            {
                var block = GameObject.CreatePrimitive(PrimitiveType.Cube);
                block.transform.SetParent(GameObject.Find("AIManager").transform);
                block.transform.localEulerAngles = new Vector3(0, 0, 0);
                //    block.transform.localScale = new Vector3(2.8f, 1f, 2.8f);
                block.transform.localScale = new Vector3(1f, 1f, 1f);
//                block.transform.position = new Vector3(tile.gameObject.transform.position.x, 0, tile.gameObject.transform.position.y * 2);

                block.transform.position = new Vector3(tile.gameObject.transform.position.x, 0, tile.gameObject.transform.position.z);
                block.AddComponent<NavMeshObstacle>().carving = true;
                block.GetComponent<Renderer>().enabled = false;
                blocks.Add(block);
            }
        }

        private void UnBlock()
        {
            if (blocks != null)
            {
                blocks.ForEach(block => DestroyImmediate(block));
                blocks = null;
            }
        }
    }
}