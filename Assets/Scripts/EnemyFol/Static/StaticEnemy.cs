namespace EnemyFol.Static
{
    public class StaticEnemy : Enemy
    {
        public StaticEnemyStates States { get; private set; }

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        { }

        protected override EnemyState CreateStates()
        {
            States = new StaticEnemyStates(this);
            return States.Idle;
        }
    }
}
