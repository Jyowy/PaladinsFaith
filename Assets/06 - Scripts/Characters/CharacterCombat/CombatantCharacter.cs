using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PaladinsFaith.Characters;
using PaladinsFaith.Spells;
using UnityEngine.Events;
using PaladinsFaith.Combat.AlteredStates;
using Sirenix.OdinInspector;
using PaladinsFaith.Combat.Combos;

namespace PaladinsFaith.Combat
{
    public abstract class CombatantCharacter : Character,
        AttackReceiver, DamageReceiver,
        AlteredStateReceiver
    {
        [SerializeField]
        protected HealthBar healthBar = null;
        [SerializeField]
        protected ContinuousResource mana = new ContinuousResource(100f);
        [SerializeField]
        protected CombatModule combatModule = null;
        [SerializeField]
        protected SpellModule spellModule = null;

        [SerializeField]
        protected AlteredState.Type immuneTo = 0;

        public UnityEvent OnDeath = null;

        [ShowInInspector]
        public AlteredState.Type AlteredStates { get; set; } = 0;

        protected override void Awake()
        {
            base.Awake();
            healthBar.Initialize(OnDead);
            combatModule.SetStamina(stamina);
            stamina.Initialize(gameObject);
            if (spellModule != null)
            {
                spellModule.SetMana(mana);
            }

            moveModule.OnPushed?.AddListener(Pushed);
            moveModule.OnStandedUp?.AddListener(StandedUp);
        }

        protected override void Update()
        {
            base.Update();
            float dt = Time.deltaTime;
            mana.Update(dt);
        }

        protected virtual bool CanMove()
        {
            return DoesntHaveAlteredState(AlteredState.Type.Stunned);
        }

        public virtual AttackResult ReceiveAttack(Attack attack)
        {
            AttackResult result = AttackResult.Success;

            if (HasAlteredState(AlteredState.Type.Shielded))
            {
                ReceivedAttackWhileShielded(attack);
                result = AttackResult.Defended;
            }
            else if (combatModule.CanBlock(attack))
            {
                combatModule.Block(attack);
                result = AttackResult.Defended;
            }
            else
            {
                attack.effectsOnImpact.ApplyOnImpact(attack.attacker, gameObject, attack.impactPoint, attack.multiplier);
            }

            return result;
        }

        public virtual void ReceiveDamage(float damage)
        {
            healthBar.ReceiveDamage(damage);
        }

        public virtual void Heal(float heal)
        {
            healthBar.Heal(heal);
        }

        protected virtual void OnDead()
        {
            OnDeath?.Invoke();
        }

        public virtual void Attack(CombatMove comboElement)
        {
            combatModule.TryToAttack(comboElement);
        }

        protected virtual void Pushed()
        {
            combatModule.CancelAttack();
        }

        public virtual void ReceiveAlteredState(AlteredState state)
        {
            AlteredState.Type type = state.type;
            if (!CanReceiveAlteredState(type))
            {
                return;
            }

            float duration = state.duration;

            switch (type)
            {
                case AlteredState.Type.KnockedDown:
                    KnockDown(duration);
                    break;

                case AlteredState.Type.Shielded:
                    Shielded(duration);
                    break;

                case AlteredState.Type.Stunned:
                    Stun(duration);
                    break;
            }

            AddAlteredState(type);
        }

        private bool CanReceiveAlteredState(AlteredState.Type type)
        {
            return !immuneTo.HasFlag(type)
                && !AlteredStates.HasFlag(type);
        }

        protected virtual void AddAlteredState(AlteredState.Type state)
        {
            AlteredStates |= state;
        }

        protected virtual void RemoveAlteredState(AlteredState.Type state)
        {
            AlteredStates &= ~state;
        }

        protected virtual bool HasAlteredState(AlteredState.Type state)
        {
            return AlteredStates.HasFlag(state);
        }

        protected virtual bool DoesntHaveAlteredState(AlteredState.Type state)
        {
            return !AlteredStates.HasFlag(state);
        }

        protected void KnockDown(float duration)
        {
            moveModule.KnockDownStarted();
            combatModule.Stop();
            combatModule.DisableAttacks();
            KnockDownStarted();
            Timers.StartGameTimer(this, "KnockedDown", duration, KnockDownFinished);
        }

        protected virtual void KnockDownStarted() { }

        protected virtual void KnockDownFinished()
        {
            moveModule.KnockDownFinished();
            RemoveAlteredState(AlteredState.Type.KnockedDown);
        }

        protected virtual void StandedUp()
        {
            combatModule.EnableAttacks();
        }

        protected void Shielded(float duration)
        {
            Timers.StartGameTimer(this, "Shielded", duration, ShieldedFinished);
        }

        protected virtual void ShieldedFinished()
        {
            RemoveAlteredState(AlteredState.Type.Shielded);
        }

        protected virtual void ReceivedAttackWhileShielded(Attack attack)
        {
        }


        protected void Stun(float duration)
        {
            Timers.StartGameTimer(this, "Stunned", duration, StunFinished);
        }

        protected virtual void StunStarted() { }

        protected virtual void StunFinished()
        {
            RemoveAlteredState(AlteredState.Type.Stunned);
        }
    }
}