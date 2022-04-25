using PlayerScripts;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    #region Private Fields
    
    [SerializeField] private RhythmManager rhythmManager;
    [SerializeField] private Player player;
    [SerializeField] private Camera mainCam;
    

    #endregion

    public RhythmManager RhythmManager => rhythmManager;
    public Player Player => player;
    public Camera MainCam => mainCam;

    private void Awake()
    {
        if(Instance != null)
            Destroy(gameObject);
        else 
            Instance = this;
    }
}
