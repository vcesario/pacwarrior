using System;

namespace topdown1;

public class Animation
{
    private float m_MaxDuration;
    private TimeSpan m_AnimationStartTime;

    private Action m_OnAnimationEnded;

    private float m_Elapsed => (float)(GameScreen.RoundDuration - m_AnimationStartTime).TotalSeconds;

    public Animation(float duration, Action onEnded = null)
    {
        m_MaxDuration = duration;
        m_OnAnimationEnded = onEnded;
    }

    public void Play()
    {
        m_AnimationStartTime = GameScreen.RoundDuration;
    }

    public void Update()
    {
        if (m_Elapsed > m_MaxDuration)
        {
            if (m_OnAnimationEnded != null)
            {
                m_OnAnimationEnded.Invoke();
                m_OnAnimationEnded = null;
            }
        }
    }

    public void Draw()
    {
        // ...
    }
}