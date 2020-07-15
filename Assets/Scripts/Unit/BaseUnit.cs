using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    public class BaseUnit : MonoBehaviour
    {
        //x,y,z
        public int width = 1;
        public int length = 1;
        public int height = 1;

        private int sortedOrder;
        
        public int order
        {
            get { return sortedOrder; }
            set
            {
                sortedOrder = value;
                foreach (Transform child in transform)
                {
                    child.GetComponent<Renderer>().sortingOrder = value;
                }
            }
        }

        public Tile origin { get; set; }
        
        //unit이 가지고 있는 타일들
        protected List<Tile> tiles = new List<Tile>();
        
        public List<Tile> GetTiles()
        {
            return tiles;
        }
            
      }
 }
