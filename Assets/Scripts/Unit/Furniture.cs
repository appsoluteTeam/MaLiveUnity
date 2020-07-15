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

        //south가 기본값
        public Direction direction = Direction.South;

        public HistoricalData previous { get; private set; }

        private const string Unit_LAYER = "Unit";
        private const string PREVIEW_LAYER = "Preview";

        public void Rotate()
        {
            direction = (Direction)(((int)direction + 1) % 4);
            transform.Rotate(0, 90, 0);
        }

        public void Rotate(Direction dir)
        {

            direction = dir;
            transform.Rotate(0, (int)dir*90, 0);
        }

        public void Move(Vector3 pos)
        {
            Rigidbody r = transform.GetComponent<Rigidbody>();
            r.velocity = (pos - transform.position) * 10;
        }

        public void SetColor(Color color)
        {
            gameObject.GetComponent<Renderer>().material.color = color;
        }


        public void Place()
        {
          
            gameObject.GetComponent<Renderer>().sortingLayerName = Unit_LAYER;
            previous = new HistoricalData(transform.position, direction);
           
        }

        public void Unplaced()
        {
            gameObject.GetComponent<Renderer>().sortingLayerName = PREVIEW_LAYER;
        }

        
    }
}