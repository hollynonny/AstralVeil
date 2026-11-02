using UnityEngine;

namespace Managers
{
    public static class LayerManager
    {
        private const string PLAYER_LAYER_NAME = "Player";
        private const string GROUND_LAYER_NAME = "Ground";
        private const string ENEMY_LAYER_NAME = "Enemy";
        private const string TRAP_LAYER_NAME = "Trap";
    
        public static readonly LayerMask GroundLayerMask = LayerMask.GetMask(GROUND_LAYER_NAME);
        public static readonly LayerMask EnemyLayerMask = LayerMask.GetMask(ENEMY_LAYER_NAME);
        public static readonly LayerMask TrapLayerMask = LayerMask.GetMask(TRAP_LAYER_NAME);
        public static readonly LayerMask PlayerLayerMask = LayerMask.GetMask(PLAYER_LAYER_NAME);

        public static readonly int GroundLayer = LayerMask.NameToLayer(GROUND_LAYER_NAME);
        public static readonly int EnemyLayer = LayerMask.NameToLayer(ENEMY_LAYER_NAME);
        public static readonly int TrapLayer = LayerMask.NameToLayer(TRAP_LAYER_NAME);
        public static readonly int PlayerLayer = LayerMask.NameToLayer(PLAYER_LAYER_NAME);
    }
}
