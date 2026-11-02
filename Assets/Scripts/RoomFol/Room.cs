using Managers;
using UnityEngine;

namespace RoomFol
{
    [RequireComponent(typeof(Collider2D))]
    public class Room : MonoBehaviour
    {
        [SerializeField] private Transform respawnPoint;
        [SerializeField] private Room respawnRoom;
        [SerializeField] private RoomEnemies enemies;
        [SerializeField] private Collider2D cameraBounds;

        [SerializeField] private GameObject[] switchableObjects;

        private bool _isSwitchableObjOn = false;
        
        public Vector3 RespawnPosition => respawnPoint.position;
        public Room RoomAfterRespawn => respawnRoom != null ? respawnRoom : this;
        public Collider2D CameraBounds => cameraBounds;
        
        private void Reset() => GetComponent<Collider2D>().isTrigger = true;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
                RoomManager.Instance.SetRoom(this);
        }

        public void TurnOn() => SetContentActive(true);

        public void TurnOff() => SetContentActive(false);

        private void SetContentActive(bool active)
        {
            if (_isSwitchableObjOn == active) return;
            _isSwitchableObjOn = active;

            for (int i = 0; i < switchableObjects.Length; i++)
            {
                if(switchableObjects[i] != null)
                    switchableObjects[i].SetActive(active);
            }
        }

        public void ResetRoom() => enemies?.ResetAll();
    }
}
