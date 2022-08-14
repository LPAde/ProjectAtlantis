using UnityEngine;

namespace Enemies
{
    public class AttackingMeleeEnemy : AttackingEnemy
    {
        private static readonly int Attack1 = Animator.StringToHash("Attack");

        protected bool DidHit;

        /// <summary>
        /// Attacks are being done with the Animator.
        /// </summary>
        public override void Attack()
        {
            if (stats.AttackCooldown > 0)
            {
                stats.AttackCooldown -= Time.deltaTime;
                return;
            }

            anim.SetTrigger(Attack1);
            source.Play();
            
            stats.AttackCooldown = stats.AttackMaxCooldown;
        }

        public override void StartAttack()
        {
            IsInAnimation = true;
            if(DidHit)
                return;
            
            if (stats.AttackRange * stats.AttackRange > (attack.transform.position - GameManager.Instance.Player.PlayerController.transform.position).sqrMagnitude)
            {
                GameManager.Instance.Player.TakeDamage(stats.Strength);
                DidHit = true;
            }
        }

        public override void EndAttack()
        {
            base.EndAttack();
            DidHit = false;
        }
    }
}
