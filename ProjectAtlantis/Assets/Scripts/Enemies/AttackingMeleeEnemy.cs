using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class AttackingMeleeEnemy : AttackingEnemy
    {
        [Header("Melee Related Stuff")] 
        [SerializeField] private List<float> importantTimes;
        [SerializeField] private float timing;
        private static readonly int Attack1 = Animator.StringToHash("Attack");

        public override void Attack()
        {
            if (stats.AttackCooldown > 0)
            {
                stats.AttackCooldown -= Time.deltaTime;
                return;
            }
            
            print("attack");
            if (!mayAttack)
                return;

            print("may attack");
            anim.SetTrigger(Attack1);
            
            timing += Time.deltaTime;

            // Sets the attack object active at correct time.
            if (!attack.activeSelf)
            {
                if(importantTimes[0] < timing && timing < importantTimes[1])
                    attack.SetActive(true);
            }
            else
            {
                if(importantTimes[1] < timing)
                    attack.SetActive(false);
            }

            if (!(importantTimes[2] < timing)) 
                return;
            
            print("attack over");
            mayAttack = false;
            timing = 0;
            stats.AttackCooldown = stats.AttackMaxCooldown;
        }
    }
}
