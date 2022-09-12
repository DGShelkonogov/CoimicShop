namespace ComicShop
{
    public class DBConnection
    {
        private static ApplicationContext applicationContext;

        public static ApplicationContext getConnection()
        {
            if (applicationContext == null)
            {
                applicationContext = new ApplicationContext();
            }
            return applicationContext;
        }
    }
}
