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
        private Rigidbody r;
        public void Rotate()
        {
            direction++;
            transform.Rotate(0, 90, 0);
        }
      public void Move(Vector3 pos)
        {
            r = transform.GetComponent<Rigidbody>();
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
            direction = 0;
        }

        public void Unplaced()
        {
            gameObject.GetComponent<Renderer>().sortingLayerName = PREVIEW_LAYER;
        }

        // 가속도 0으로 만들기
        public void ToVelocityZero()
        {
            r.velocity = Vector3.zero;
        }

        // 가구 잠금
        public void SetOnIsKinematic()
        {
            transform.GetComponent<Rigidbody>().isKinematic = true;
        }
        // 잠금 해제
        public void SetOffIsKinematic()
        {
            transform.GetComponent<Rigidbody>().isKinematic = false;
        }

        public void UndoMove()
        {
            transform.position = previous.pos;
        }
        public void UndoRotate()
        {
            Debug.Log(direction);
            transform.Rotate(0, (int)direction * 90 * -1, 0); 
            direction = 0;
        }
    }
}