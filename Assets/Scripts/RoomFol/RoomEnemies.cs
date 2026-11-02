using Managers;
using UnityEngine;

namespace RoomFol
{
    public class RoomEnemies : MonoBehaviour
    {
        private IResettable[] _items;

        private void Awake()
        {
            _items = GetComponentsInChildren<IResettable>(true);
        }

        public void ResetAll()
        {
            for(int i = 0; i < _items.Length; i++)
                _items[i].ResetState();
        }
    }
}
