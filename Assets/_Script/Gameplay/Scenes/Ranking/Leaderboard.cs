using GooglePlayGames;
using UnityEngine.Events;
using UnityEngine;
using Enviroments;
using GooglePlayGames.BasicApi;
using Common;
using System.Collections;

namespace Gameplay
{
    public class Leaderboard : SingletonPersistent<Leaderboard>
    {
        public bool isConnectdToLeaderboard = false;
        public bool IsLeaderboardUIShowing = false;
        private bool previousAuthState = false;
        private void Start()
        {
            if (Enviroment.ENV == Enviroment.Env.PROD)
            {
                PlayGamesPlatform.DebugLogEnabled = false;
            }
            else
            {
                //PlayGamesPlatform.DebugLogEnabled = true;
            }
            PlayGamesPlatform.Activate();
            SignInToLeaderboardAutomatic();
        }

        public void SignInToLeaderboardAutomatic()
        {
            PlayGamesPlatform.Instance.Authenticate((result) =>
            {
                switch (result)
                {
                    case SignInStatus.Success:
                        Debug.Log("Automatic authentication successful");
                        isConnectdToLeaderboard = true;
                        break;
                    default:
                        Debug.Log("Automatic authentication failed " + result);
                        isConnectdToLeaderboard = false;

                        break;
                }
            });
        }

        public void SignInToLeaderboardManual(UnityAction<bool> callback = null)
        {
            if (isConnectdToLeaderboard)
            {
                callback?.Invoke(true);
            }
            else
            {
                PlayGamesPlatform.Instance.ManuallyAuthenticate((result) =>
                {
                    if (result == SignInStatus.Success)
                    {
                        // Continue with Play Games Services
                        Debug.Log("manual authentication successful");
                        isConnectdToLeaderboard = true;
                    }
                    else
                    {
                        // Disable your integration with Play Games Services or show a login button
                        // to ask users to sign-in. Clicking it should call
                        Debug.Log("manual authentication failed: " + result);
                        isConnectdToLeaderboard = false;
                    }
                    callback?.Invoke(isConnectdToLeaderboard);

                });
            }
        }
        public void ShowLeaderboard()
        {
            Social.ShowLeaderboardUI();
            IsLeaderboardUIShowing = true;
        }
        public void ReportScore(int score)
        {
            if (isConnectdToLeaderboard == false)
            {
                return;
            }
            string idLeaderboard = GPGSIds.leaderboard_topcat;
            Social.ReportScore(score, idLeaderboard, (bool success) =>
            {
                Debug.Log("report score success with score: " + score + "id: " + idLeaderboard);
            });
        }

        private void Update()
        {
            // Detect leaderboard UI close
            if (IsLeaderboardUIShowing && !Social.localUser.authenticated)
            {
                IsLeaderboardUIShowing = false;
            }
        }
    }
}