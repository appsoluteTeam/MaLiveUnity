using UnityEngine;

namespace Model
{
    public class HistoricalData
    {
        public Vector3 pos { get; private set; }
        public Direction direction { get; private set; }

        public HistoricalData(Vector3 previousPos, Direction previousDir)
        {
            this.direction = previousDir;
            this.pos = previousPos;
        }
    }
}
