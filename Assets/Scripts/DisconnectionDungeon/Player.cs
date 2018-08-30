﻿using System.Collections;
using DisconnectionDungeon.InputSystem;
using Graphene.Acting;
using Graphene.Acting.Collectables;
using UnityEngine;
using Physics = UnityEngine.Physics;

namespace DisconnectionDungeon
{
    public class Player : Actor
    {
        private DDManager _manager;

        public CharacterPhysics Physics;

        private SpriteRenderer _renderer;

        private InputManager _input;
        private bool _moving;
        private IInteractible _currentIntreactible;

        private bool _canClear;

        private void Awake()
        {
            Life.OnDie += Die;

            _input = new InputManager();
            _input.Direction += Move;
            _input.Interact += Interact;

            _renderer = GetComponent<SpriteRenderer>();

            Physics.OnTriggerEnter += OnTriggered;
        }

        private void Move(Vector2 dir)
        {
            var dirInt = new Vector2Int((int) (dir.x), (int) (dir.y));

            if (dir.x > 0)
                _renderer.flipX = true;
            else if (dir.x < 0)
                _renderer.flipX = false;

            _canClear = true;

            if (Physics.CheckCollision(transform.position, dirInt) || dirInt.magnitude <= 0 || _moving) return;

            StartCoroutine(Mover(dirInt));
        }

        IEnumerator Mover(Vector2Int dir)
        {
            _moving = true;

            var v3Dir = new Vector3(dir.x, dir.y);
            var finalPos = transform.position + v3Dir;
            while (true)
            {
                transform.position += v3Dir * Time.deltaTime * Physics.Speed;

                yield return null;

                if ((finalPos - transform.position).magnitude < 0.1f)
                {
                    transform.position = finalPos;
                    break;
                }
            }

            _moving = false;

            if (_canClear)
                _currentIntreactible = null;
        }

        public void Transport(Vector3Int destination, bool popup)
        {
            if (popup)
            {
                transform.position = destination;
                return;
            }

            var dir = destination - transform.position;
            StartCoroutine(Mover(new Vector2Int((int) dir.x, (int) dir.y)));
        }

        public bool CanInteract()
        {
            return _currentIntreactible != null;
        }

        private void Interact()
        {
            if (_currentIntreactible == null) return;

            _currentIntreactible.Interact();
        }

        private void Start()
        {
            _manager = FindObjectOfType<DDManager>();
        }

        private void Die()
        {
            _manager.OnPlayerDie();

            Debug.LogError("Die");
        }

        public override void DoDamage(int damage)
        {
            Life.ReceiveDamage(damage);
        }

        protected override void OnTriggered(RaycastHit2D hit)
        {
            var col = hit.transform.GetComponent<ICollectable>();
            if (col != null)
            {
                col.Collect(this);
            }

            var intreactible = hit.transform.GetComponent<IInteractible>();

            if (intreactible != null)
            {
                _currentIntreactible = intreactible;
                _canClear = false;
            }
        }
    }
}