using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSceneManager : MonoBehaviour
{
    public MatchSetting MatchSetting;
    [SerializeField] private MenuSceneUI _menuSceneUI;
    private int _currentSportIndex;
    private void Awake()
    {
        _menuSceneUI.Init(this);
    }

    public void StartMatch()
    {
        SceneManager.LoadScene("MatchScene");
    }
}

