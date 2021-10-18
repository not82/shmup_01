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
            AddState(BossState.Dead, bossStateDead);
        }

        [Inject] private BossStatePhaseOne bossStatePhaseOne;
        [Inject] private BossStateDead bossStateDead;
    }
}