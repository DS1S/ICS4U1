using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tales_of_a_Spooderman.Core
{
    public enum AnimationStates
    {
        //Primary
        NONE,

        //Animation States
        RUN,
        PUNCH,
        JUMP,
        IDLE,
        SWING,
        SHOOT,
        FLY,
        HURT,
        WALK,
        BOMB,

        //Menu Animations
        MAIN_MENU_BACKGROUND,

    }

    public enum EnemyStates
    {
        //Enemy States
        NAVIGATING,
        SHOOTING,
        MOVINGAWAY,
    }

}
