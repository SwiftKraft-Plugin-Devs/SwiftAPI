using PlayerRoles;

namespace SwiftAPI.API.CustomRoles
{
    public abstract class CustomRoleBase(RoleTypeId role, params ItemType[] loadout)
    {
        public RoleTypeId Role = role;

        public ItemType[] Loadout = loadout;
    }
}
