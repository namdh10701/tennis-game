using TMPro;
using UnityEngine;
public class MenuSceneUI : MonoBehaviour
{
    private MenuSceneManager _sceneManager;

    public TextMeshProUGUI incrementalText;
    public TextMeshProUGUI sportText;

    public void Init(MenuSceneManager sceneManager)
    {
        _sceneManager = sceneManager;
    }

    private void Awake()
    {
        incrementalText.text = "Speed: x" + _sceneManager.MatchSetting.Incremental;
        sportText.text = _sceneManager.MatchSetting.GetCurrentSportNameToString();
    }

    public void OnIncrementalClick()
    {
        _sceneManager.MatchSetting.ChangeIncremental();
        incrementalText.text = "Speed: x" + _sceneManager.MatchSetting.Incremental;
    }

    public void OnChangeGameClick()
    {
        _sceneManager.MatchSetting.ChangeGame();
        sportText.text = _sceneManager.MatchSetting.GetCurrentSportNameToString();
    }

    public void OnPlayCick()
    {
        _sceneManager.StartMatch();
    }
}

