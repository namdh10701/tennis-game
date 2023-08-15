using JetBrains.Annotations;
using Services.FirebaseService.Remote;
using System;
using UnityEngine;

namespace Gameplay
{
    public class DifficultyManager : MonoBehaviour
    {
        private MatchData _matchData;
        private float _timescaleStep;
        private IncrementalStep _incrementalStep;
        private MatchManager _matchManager;
        private GameDataManager _gameDataManager;
        public void Init(MatchData matchData, IncrementalStep incrementalStep, float timescaleStep, MatchManager matchManager, GameDataManager gameDataManager)
        {
            _matchManager = matchManager;
            _timescaleStep = timescaleStep;
            _incrementalStep = incrementalStep;
            _matchData = matchData;
            _gameDataManager = gameDataManager;
        }

        public void ApplyDifficulty()
        {
            Time.timeScale = _timescaleStep * _matchData.MatchSettings.Incremental;
        }
        public void UpdateDifficulty()
        {
            if (_matchData.MatchSettings.Incremental >= MatchSetting.MaxIncremental)
                return;
            if (_matchData.Score >= _incrementalStep.Steps[_matchData.MatchSettings.Incremental - 1].TriggerScore)
            {
                _matchData.MatchSettings.ChangeIncremental();
                ApplyDifficulty();
                _matchManager.OnDifficultyChange();
                if (_matchData.MatchSettings.Incremental > _gameDataManager.GameDatas.UnlockedIncremental)
                {
                    _gameDataManager.GameDatas.UnlockedIncremental = _matchData.MatchSettings.Incremental;
                    _gameDataManager.SaveDatas();
                }
            }
        }
    }
}