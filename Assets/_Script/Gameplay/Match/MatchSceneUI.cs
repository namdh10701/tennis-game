using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchSceneUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _countdownText;
    [SerializeField] private TextMeshProUGUI _timeCountText;
    [SerializeField] private TextMeshProUGUI _scoreText;

    private MatchEvent _matchEvent;
    private MatchData _matchData;


    [SerializeField] private TextMeshProUGUI _endMatchTimeText;
    [SerializeField] private TextMeshProUGUI _endMatchScoreText;
    [SerializeField] private GameObject _endMatchPopup;
    public void Init(MatchEvent matchEvent, MatchData matchData)
    {
        _matchEvent = matchEvent;
        _matchData = matchData;
    }
    private void OnEnable()
    {
        if (_matchEvent != null)
        {
            _matchEvent.CountdownToStart.AddListener(() => ToggleCountdownText(true));
            _matchEvent.RemainingTimeToStart.AddListener((remaining) => UpdateCountdownText(remaining));
            _matchEvent.MatchStart.AddListener(() => ToggleCountdownText(false));
            _matchEvent.ScoreUpdate.AddListener(() => UpdateScore());
            _matchEvent.TimeUpdate.AddListener(() => UpdateTime());
            _matchEvent.MatchEnd.AddListener(() => DisplayEndMatchPopup());
        }
    }
    private void OnDisable()
    {
        _matchEvent.CountdownToStart.RemoveListener(() => ToggleCountdownText(true));
        _matchEvent.RemainingTimeToStart.RemoveListener((remaining) => UpdateCountdownText(remaining));
        _matchEvent.MatchStart.RemoveListener(() => ToggleCountdownText(false));
        _matchEvent.ScoreUpdate.RemoveListener(() => UpdateScore());
        _matchEvent.TimeUpdate.RemoveListener(() => UpdateTime());
        _matchEvent.MatchEnd.RemoveListener(() => DisplayEndMatchPopup());
    }

    private void ToggleCountdownText(bool IsShowing)
    {
        _countdownText.gameObject.SetActive(IsShowing);
    }

    public void UpdateCountdownText(float newTime)
    {
        _countdownText.text = newTime.ToString();
    }

    public void UpdateTime()
    {
        _timeCountText.text = _matchData.ElapsedTime.ToString("00:00");
    }

    public void UpdateScore()
    {
        _scoreText.text = _matchData.Score.ToString();
    }

    public void DisplayEndMatchPopup()
    {
        _endMatchTimeText.text = _matchData.ElapsedTime.ToString("00:00");
        _endMatchScoreText.text = _matchData.Score.ToString();
        _endMatchPopup.SetActive(true);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}