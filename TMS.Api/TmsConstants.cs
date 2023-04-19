namespace TMS.Api;

public struct TmsConstants
{
    public struct Policies
    {
        public const string Admin = nameof(Admin);
    }

    public struct Roles
    {
        public const string Admin = nameof(Admin);

        public const string TaskCreate = nameof(TaskCreate);
        public const string TaskUpdate = nameof(TaskUpdate);
        public const string TaskDelete = nameof(TaskDelete);
    }

    public struct Claims
    {
        public const string Role = "role";
    }
}