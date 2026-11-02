using System.Collections;
using System.Collections.Generic;
using CameraFol;
using PlayerFol;
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
        [SerializeField] private Transform player;
        
        public Transform Player => player;
        
        public Room CurrentRoom { get; private set; }
        public List<Room> InRooms { get; private set; }
        public List<Room> AllRooms { get; private set; }

        private void Awake()
        {
            Instance = this;
            CurrentRoom = startRoom;
            
            Room[] rooms = FindObjectsByType<Room>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            AllRooms = new List<Room>();
            foreach (Room room in rooms)
            {
                room.TurnOff();
                AllRooms.Add(room);
            }     
            CurrentRoom.TurnOn();
            
            InRooms = new List<Room>();
            InRooms.Add(startRoom);
        }

        private void Start() => ApplyBounds(CurrentRoom);

        public void SetRoom(Room room)
        {
            if (room == CurrentRoom) return;
            if(!InRooms.Contains(room)) InRooms.Add(room);
            
            StartCoroutine(WaitForSetRoom(room));
        }

        private WaitForSecondsRealtime _peakWait = new(0.5f);
        private IEnumerator WaitForSetRoom(Room room)
        {
            room.TurnOn();
            
            CameraEffects.Instance.StartEmitting();
            
            yield return _peakWait;
            
            CameraEffects.Instance.StopEmitting();
            
            CurrentRoom.TurnOff();
            CurrentRoom = room;
            
            ApplyBounds(CurrentRoom);
        }

        public void Respawn()
        {
            Room died = CurrentRoom;
            foreach (Room room in InRooms)
            {
                room.ResetRoom();
            }
            InRooms.Clear();
            
            CurrentRoom.TurnOff();
            CurrentRoom = died.RoomAfterRespawn;
            CurrentRoom.TurnOn();
            InRooms.Add(CurrentRoom);
            
            Vector3 pos = CurrentRoom.RespawnPosition;
            Vector3 delta = pos - player.transform.position;
            player.transform.position = pos;
            
            ApplyBounds(CurrentRoom);
            
            vcam.OnTargetObjectWarped(player.transform, delta);
        }

        private void ApplyBounds(Room room)
        {
            confiner.BoundingShape2D = room.CameraBounds;
            confiner.InvalidateBoundingShapeCache();
        }
    }
}
