using DefaultNamespace.BossStates;
using DefaultNamespace.GameStates;
using Zenject;

namespace DefaultNamespace
{
    public class BossStateController : StateController<BossState>, IInitializable
    {
        public void Initialize()
        {
            AddState(BossState.Phase1, bossStatePhaseOne);
            AddState(BossState.Phase2, bossStatePhaseTwo);
        }

        [Inject] private BossStatePhaseOne bossStatePhaseOne;
        [Inject] private BossStatePhaseTwo bossStatePhaseTwo;
    }
}