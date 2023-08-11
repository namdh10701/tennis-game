using Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Monetization.Ads.UI;

public class GameOverPanel : InterPopup
{
    [SerializeField] private TextMeshProUGUI _time;
    [SerializeField] private TextMeshProUGUI _score;
    [SerializeField] private TextMeshProUGUI _highScore;

    public void Init(MatchData matchData, GameData gameData)
    {
        _time.text = $"Time: {matchData.ElapsedTime}";
        _score.text = $"Score: {matchData.Score}";
        _highScore.text = $"High Score: {gameData.HighScore}";
    }
}