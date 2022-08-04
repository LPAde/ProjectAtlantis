using UnityEngine;

namespace Enemies
{
    public class AttackingMeleeEnemy : AttackingEnemy
    {
        private static readonly int Attack1 = Animator.StringToHash("Attack");
        [SerializeField] private AudioSource source;

        private bool _didHit;

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
            
            if (!mayAttack)
                return;

            anim.SetTrigger(Attack1);
            
            mayAttack = false;
            stats.AttackCooldown = stats.AttackMaxCooldown;
        }

        public void StartAttack()
        {
            if(_didHit)
                return;
            
            print("distance" + (attack.transform.position - GameManager.Instance.Player.PlayerController.transform.position).sqrMagnitude);
            print("AR: " + stats.AttackRange);
            if (stats.AttackRange * stats.AttackRange > (attack.transform.position - GameManager.Instance.Player.PlayerController.transform.position).sqrMagnitude)
            {
                GameManager.Instance.Player.TakeDamage(stats.Strength);
                _didHit = true;
            }
            
            source.Play();
        }

        public void EndAttack()
        {
            _didHit = false;
        }
    }
}
