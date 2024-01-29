using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public ThirdPersonController thirdPersonController;

    public AudioSource effectSource;
    public AudioSource musicSource;

    public AudioClip footSteps;
    public AudioClip runSteps;
    public AudioClip ballWhoosh;
    public AudioClip talk;
    public AudioClip yoyo;
    public AudioClip card;
    public AudioClip flower;
    public AudioClip flute;
    public AudioClip hammer;
    public AudioClip enemyLaugh;
    public AudioClip kingLaugh;
    public AudioClip kingGrumble;
    public AudioClip swordWhoosh;
    public AudioClip swordHit;
    public AudioClip greatSwordWhoosh;
    public AudioClip greatSwordHit;
    public AudioClip bossHit;
    public AudioClip groundSlam;
    public AudioClip bossJump;
    public AudioClip warCry;
    public AudioClip rasengan;

    public AudioSource combatMusic;
    public AudioSource bossMusic;
    
    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void FootStepSFX()
    {
        effectSource.PlayOneShot(footSteps);
    }

    public void FootStepRunFX()
    {
        effectSource.PlayOneShot(runSteps);
    }

    public void BallWhooshFX()
    {
        effectSource.PlayOneShot(ballWhoosh);
    }
    
    public void TalkFX()
    {
        effectSource.PlayOneShot(talk);
    }
    
    public void YoYoFx()
    {
        effectSource.PlayOneShot(yoyo);
    }
    
    public void CardFX()
    {
        effectSource.PlayOneShot(card);
    }
     
    public void FlowerFX()
    {
        effectSource.PlayOneShot(flower);
    }
    
    public void FluteFX()
    {
        effectSource.PlayOneShot(flute);
    }

    public void HammerFX()
    {
        effectSource.PlayOneShot(hammer);
    }

    public void EnemyLaugh()
    {
        effectSource.PlayOneShot(enemyLaugh);
    }

    public void KingLaugh()
    {
        effectSource.PlayOneShot(kingLaugh);
    }
    
    public void KingGrumble()
    {
        effectSource.PlayOneShot(kingGrumble);
    }

    public void SwordWhoosh()
    {
        effectSource.PlayOneShot(swordWhoosh);
    }

    public void SwordHit()
    {
        effectSource.PlayOneShot(swordHit);
    }

    public void GreatSwordWhoosh()
    {
        effectSource.PlayOneShot(greatSwordWhoosh);
    }

    public void GreatSwordHit()
    {
        effectSource.PlayOneShot(greatSwordHit);
    }

    public void BossHit()
    {
        effectSource.PlayOneShot(bossHit);
    }

    public void GroundSlam()
    {
        effectSource.PlayOneShot(groundSlam);
    }

    public void BossJump()
    {
        effectSource.PlayOneShot(bossJump);
    }
    
    public void WarCry()
    {        
        effectSource.PlayOneShot(warCry);
    }

    public void Rasengan()
    {        
        effectSource.PlayOneShot(rasengan);
    }
    
    // Play a single clip through the sound effects source.
    public void PlayEffect(AudioClip clip, bool loop = false)
    {
        effectSource.clip = clip;
        effectSource.loop = loop;
        effectSource.Play();
    }
    
    // Play a single clip through the music source.
    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }

    // Change the volume of the music
    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    // Change the volume of the sound effects
    public void SetEffectsVolume(float volume)
    {
        effectSource.volume = volume;
    }

    
}