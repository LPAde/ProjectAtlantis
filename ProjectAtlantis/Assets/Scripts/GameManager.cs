using Enemies;
using Gameplay.Rhythm;
using PlayerScripts;
using UI;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    #region Private Fields
    
    [SerializeField] private Player player;
    [SerializeField] private Camera mainCam;
    
    [Header("Managers")]
    [SerializeField] private RhythmManager rhythmManager;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private HudManager hudManager;
    [SerializeField] private AudioManager audioManager;
    

    #endregion

    #region Properties

     public Player Player => player;
     public Camera MainCam => mainCam;
     public RhythmManager RhythmManager => rhythmManager;
     public EnemyManager EnemyManager => enemyManager;
     public HudManager HudManager => hudManager;
     public AudioManager AudioManager => audioManager;

    #endregion
    
    private void Awake()
    {
        if(Instance != null)
            Destroy(gameObject);
        else 
            Instance = this;
    }
}