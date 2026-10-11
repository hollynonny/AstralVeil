using RoomFol;
using Unity.Cinemachine;
using UnityEngine;

namespace Managers
{
    public class RoomManager : MonoBehaviour
    {
        public static RoomManager Instance { get; private set; }

        [SerializeField] private Room startRoom;
        [SerializeField] private CinemachineCamera vcam;
        [SerializeField] private CinemachineConfiner2D confiner;
        
        public Room CurrentRoom { get; private set; }

        private void Awake()
        {
            Instance = this;
            CurrentRoom = startRoom;
        }

        private void Start() => ApplyBounds(CurrentRoom);

        public void SetRoom(Room room)
        {
            if (room == CurrentRoom) return;
            CurrentRoom = room;
            ApplyBounds(CurrentRoom);
        }

        public void Respawn(Transform player)
        {
            Room died = CurrentRoom;
            died.ResetRoom();
            
            Vector3 pos = died.RespawnPosition;
            Vector3 delta = pos - player.position;
            player.position = pos;
            
            CurrentRoom = died.RoomAfterRespawn;
            ApplyBounds(CurrentRoom);
            
            vcam.OnTargetObjectWarped(player, delta);
        }

        private void ApplyBounds(Room room)
        {
            Debug.Log("Applying bounds!");
            confiner.BoundingShape2D = room.CameraBounds;
            confiner.InvalidateBoundingShapeCache();
        }
    }
}
