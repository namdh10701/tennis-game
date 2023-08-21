namespace Enviroments
{
    public static class Enviroment
    {
        public enum Env
        {
            DEV, TEST_TEST_ADS, TEST_REAL_ADS, TEST_SKIP_ADS, PROD
        }
        private static Env _env;

        public static Env ENV
        {
            get { return _env; }
            set { _env = value; }
        }
    }
}