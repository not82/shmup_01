using DefaultNamespace.BossStates;
using DefaultNamespace.GameStates;
using Zenject;

namespace DefaultNamespace
{
    public class BossStateController : StateController<BossState>, IInitializable
    {
        public void Initialize()
        {
            AddState(BossState.Seb1, bossStateSeb1);
            AddState(BossState.Dead, bossStateDead);
        }

        [Inject] private BossStateSeb1 bossStateSeb1;
        [Inject] private BossStateDead bossStateDead;
    }
}