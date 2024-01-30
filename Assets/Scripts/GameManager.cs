using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    #endregion

    
    public List<Image> laughImages;
    public int bossLaughedCount;
    
    public GameObject bossLaughMeter;

    public Image healthBar;
    public float health;
    public float maxHealth;

    public Animator characterAnimator;
    private static readonly int Death = Animator.StringToHash("Death");
    public bool dead;
    public GameObject skipButton;
    public TextMeshProUGUI skipText;

    public CanvasGroup youLostCG;
    public GameObject youLost;
    void Start()
    {
        maxHealth = health;
    }
    
    public void LaughMeterIncrease()
    {
        laughImages[bossLaughedCount].gameObject.SetActive(true);
        var color = new Color(laughImages[bossLaughedCount].color.r, laughImages[bossLaughedCount].color.g,
            laughImages[bossLaughedCount].color.b, 1);
        laughImages[bossLaughedCount].DOColor(color, 2f);
        bossLaughedCount++;
    }

    public void UpdateHealthBar()
    {
        health = Mathf.Clamp(health,0, maxHealth);
        healthBar.transform.localScale = new Vector3(health / maxHealth, 1, 1);
        if (health <= 0.1f)
        {
            characterAnimator.SetTrigger(Death);
            dead = true;
            youLost.SetActive(true);
            float angle = 0;
            DOTween.To(() => angle, x => angle = x, 1, 1)
                .OnUpdate(() =>
                {
                    youLostCG.alpha = angle;
                }).SetDelay(3f);
        }
    }

    public void RestartTheGame()
    {
        SceneManager.LoadScene(0);
    }
}