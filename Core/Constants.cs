namespace ASP.NETCoreIdentityCustom.Core
{
    public static class Constants
    {
        public static class Roles
        {
            public const string Teacher = "Teacher";
            public const string Student = "Student";
            public const string Admin = "Admin";
        }

        public static class Policies
        {
            public const string RequireTeacher = "RequireTeacher";
            public const string RequireStudent = "RequireStudent";
            public const string RequireAdmin = "RequireAdmin";
        }
    }
}
