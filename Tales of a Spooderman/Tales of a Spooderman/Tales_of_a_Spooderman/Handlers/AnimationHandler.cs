using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tales_of_a_Spooderman.Core;

namespace Tales_of_a_Spooderman.Handlers
{
    class AnimationHandler
    {
        private List<Animation> animations = new List<Animation>();
        private Animation[] prequalifiedAnimations;

        public AnimationHandler(Animation[] anims)
        {
            SetPrequalAnims(anims);
        }

        public void Update(GameTime gameTime)
        {
            foreach(Animation anim in animations)
            {
                anim.Animate(gameTime, true);
            }
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle destRec, SpriteEffects effect)
        {
            foreach(Animation anim in animations)
            {
                anim.Draw(spriteBatch, destRec, Color.White, effect);
            }
        }

        public void AddAnimation(Animation anim)
        {
            animations.Add(anim);
        }

        public void RemoveAnimation(AnimationStates anim)
        {
            for(int i = 0; i < animations.Count; i++)
            {
                if (animations[i].GetAnimationName().Equals(anim))
                {
                    animations[i].Reset();
                    animations.Remove(animations[i]);
                }
            }
        }

        public AnimationStates GetCurrentAnimationName()
        {
            foreach (Animation anim in animations)
            {
               return anim.GetAnimationName();
            }

            return AnimationStates.NONE;
        }

        public Animation GetCurrentAnimation()
        {
            foreach (Animation anim in animations)
            {
                return anim;
            }

            return null;
        }

        public void SwapAnimations(Animation anim)
        {
            if(animations.Count != 0)
                RemoveAnimation(GetCurrentAnimation().GetAnimationName());
            AddAnimation(anim);
        }

        public Animation CheckAnimation(AnimationStates animT)
        {
            foreach (Animation anim in prequalifiedAnimations)
            {
                if (anim.GetAnimationName().Equals(animT))
                    return anim;
            }

            return null;
        }

        private void SetPrequalAnims(Animation[] anims)
        {
            this.prequalifiedAnimations = anims;
        }
    }
}
