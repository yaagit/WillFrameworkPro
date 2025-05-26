using WillFrameworkPro.Core.Attributes.Types;

namespace WillFrameworkPro.Core.Context
{
    internal static class PermissionForIdentities
    {
        public static PermissionFlags View = PermissionFlags._None | PermissionFlags.InjectModel | PermissionFlags.InjectService;
        public static PermissionFlags Service = PermissionFlags._None | PermissionFlags.InjectModel | PermissionFlags.InjectHighLevelCommandManager;
        public static PermissionFlags Model = PermissionFlags._None;
        public static PermissionFlags General = PermissionFlags._None | PermissionFlags.InjectCommandManager;

        public static PermissionFlags GetPermissionsByIdentityType(TypeEnum typeEnum)
        {
            switch (typeEnum)
            {
                case TypeEnum.View:
                    return View;
                case TypeEnum.Service:
                    return Service;
                case TypeEnum.Model:
                    return Model;
            }
            return General;
        }
    }
}