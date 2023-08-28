namespace Enviroments
{
    public static class Enviroment
    {
        public enum Env
        {
            DEV, TEST, PROD
        }
        private static Env _env;

        public static Env ENV
        {
            get { return _env; }
            set { _env = value; }
        }
    }
}