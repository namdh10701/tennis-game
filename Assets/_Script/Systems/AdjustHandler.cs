using com.adjust.sdk;
using Enviroments;

public static class AdjustHandler
{   
    public static void Init()
    {
        AdjustEnvironment adjustEnvironment;
        if (Enviroment.ENV != Enviroment.Env.PROD)
        {
            adjustEnvironment = AdjustEnvironment.Sandbox;
        }
        else
        {
            adjustEnvironment = AdjustEnvironment.Production;
        }
        AdjustConfig config = new AdjustConfig("2xsbg44bal4w", adjustEnvironment);
        Adjust.start(config);
    }
}