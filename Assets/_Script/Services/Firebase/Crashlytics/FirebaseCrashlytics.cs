using Firebase;
using UnityEngine;
using Enviroments;
namespace Services.FirebaseService.Crashlytics
{
    public class FirebaseCrashlytics : MonoBehaviour
    {
        public void Init()
        {
            if (Enviroment.ENV == Enviroment.Env.PROD)
                Firebase.Crashlytics.Crashlytics.ReportUncaughtExceptionsAsFatal = true;
        }
    }
}
