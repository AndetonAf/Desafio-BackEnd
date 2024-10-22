namespace BackEnd.Configurations
{
    public static class GlobalConfigurations
    {
        public static Settings Settings { get; private set; } = new Settings(
            new Auth(""),
            new ConnectionStrings(""),
            new Gcp(true, "", "", "")
        );

        public static void Initialize(Settings settings)
        {
            Settings = settings;
        }
    }
}
