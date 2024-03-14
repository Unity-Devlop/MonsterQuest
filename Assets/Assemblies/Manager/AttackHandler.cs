using Mirror;

namespace Game
{
    public static class AttackHandler
    {
        [Client]
        public static void HandleAttack(IEntity entity, IHittable target, int damagePoint)
        {
            if (!target.canBeHit) return;

            if (entity.groupId == TeamGroup.Default.id || target.groupId == TeamGroup.Default.id ||
                entity.groupId != target.groupId)
            {
                target.canBeHit = false;  // 客户端暂时设置为不可被攻击 后面服务器同步
                target.HandleBeAttack(damagePoint);
            }
        }
    }
}