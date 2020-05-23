using UnityEngine;
namespace Model
{
    public class Tile : MonoBehaviour
    {
        public int x { get; private set; }
        public int y { get; private set; }

        public int z { get; private set; }

        [SerializeField]
        public bool isBlock = false;

        public void Set(int x, int y,int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
}
