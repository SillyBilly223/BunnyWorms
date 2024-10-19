using BunnyWorms.Actions;
using System;
using System.Collections.Generic;
using System.Text;
using static UnityEngine.UI.CanvasScaler;

namespace BunnyWorms.CustomePassives
{
    public class BunnyWormPassive : BasePassiveAbilitySO
    {
        public override bool IsPassiveImmediate => true;

        public override bool DoesPassiveTrigger => true;

        public override void TriggerPassive(object sender, object args)
        {
            BunnyWormForbiddenFruitAction.TryAddAction();
        }

        public override void OnPassiveConnected(IUnit unit)
        {
            CombatManager._instance.AddObserver(TryTriggerPassive, TriggerCalls.OnRoundFinished.ToString(), unit);
        }

        public override void OnPassiveDisconnected(IUnit unit)
        {
            CombatManager._instance.RemoveObserver(TryTriggerPassive, TriggerCalls.OnRoundFinished.ToString(), unit);
        }
    }
}
