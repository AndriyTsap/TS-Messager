namespace ConsoleApp1
{
    public class AppSettings
    {
        private AppSettings() { }

        private static AppSettings _instance;

        public static AppSettings Instance
        {
            get
            {
                if(_instance == null)
                    _instance = new AppSettings();
                return _instance;
            }
        }

        public string UsersFilePath
        {
            get { return "../Data/User.xml"; }
        }

        public string MessagesFilePath
        {
            get { return "../Data/User.xml"; }
        }

        public string GroupsFilePath
        {
            get { return "../Data/User.xml"; }
        }

        public string UserGroupsFilePath
        {
            get { return "../Data/User.xml"; }
        }

        public string ConnectionString
        {
            get { return @"Server = (localdb)\MSSQLLocalDB; Database = PhotoGalery; Trusted_Connection = True; MultipleActiveResultSets = true"; }
        }
    }
}