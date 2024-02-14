using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaladinsFaith.PlayerInput
{
    public class PlayerInputData
    {
        public Vector2 move = Vector2.zero;
        public Vector2 cameraRotation = Vector2.zero;
        public bool fastMove = false;

        public bool defenseActive = false;
        public bool heavyAttack = false;

        private readonly BufferedActivation lightAttack = new BufferedActivation();
        private readonly float lightAttackBufferTime = 1.5f;

        private readonly BufferedActivation spell = new BufferedActivation();
        private readonly float magicBufferTime = 1.5f;

        public readonly ButtonActivation prevSpell = new ButtonActivation();
        public readonly ButtonActivation nextSpell = new ButtonActivation();

        public readonly BufferedActivation interact = new BufferedActivation();
        private readonly float intearctionBufferTime = 0.25f;

        private float prevUpdateTime = 0f;

        public void Update()
        {
            float currentTime = Time.time;
            float dt = currentTime - prevUpdateTime;
            prevUpdateTime = currentTime;

            if (prevUpdateTime == 0f
                || dt <= 0f)
            {
                return;
            }

            lightAttack.UpdateTime(dt);
            spell.UpdateTime(dt);
            interact.UpdateTime(dt);
        }

        public void LightAttack()
        {
            lightAttack.Activate(lightAttackBufferTime);
        }

        public bool IsLightAttackActive()
        {
            return lightAttack.Active;
        }

        public void ConsumeLightAttack()
        {
            lightAttack.Finish();
        }

        public bool IsPrevSpellRequested()
        {
            return prevSpell.Active;
        }

        public void ConsumePrevSpellRequest()
        {
            prevSpell.Consume();
        }

        public bool IsNextSpellRequested()
        {
            return nextSpell.Active;
        }

        public void ConsumeNextSpellRequest()
        {
            nextSpell.Consume();
        }

        public void Spell()
        {
            spell.Activate(magicBufferTime);
        }

        public bool IsSpellActive()
        {
            return spell.Active;
        }

        public void ConsumeSpell()
        {
            spell.Finish();
        }

        public void Interact()
        {
            interact.Activate(intearctionBufferTime);
        }

        public bool IsInteractActive()
        {
            return interact.Active;
        }

        public void ConsumeInteract()
        {
            interact.Finish();
        }
    }
}