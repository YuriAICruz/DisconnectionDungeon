﻿using UnityEngine;

namespace Graphene.DisconnectionDungeon
{
    public class AnimationManager
    {
        private Animator _animator;

        public AnimationManager(Animator animator)
        {
            _animator = animator;
        }
        
        public void Interact()
        {
            _animator.SetTrigger("Interact");            
        }

        public void Attack()
        {
            _animator.SetTrigger("Attack");   
        }
        
        public void AttackSeq()
        {
            _animator.SetTrigger("Attack_Seq"); 
        }

        public void SetSpeed(float speed)
        {
            _animator.SetFloat("Speed", speed);   
        }

        public void Jump(bool state)
        {
            _animator.SetBool("Jumping", state);
        }

        public void SetGroundState(bool state)
        {
            _animator.SetBool("Grounded", state);   
        }

        public void TouchWall(Vector2Int dir)
        {
            _animator.SetFloat("WallX", dir.x);   
            _animator.SetFloat("WallY", dir.y);   
        }
    }
}