namespace PDP_RBAC1.Model
{
    public class Permission
    {
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is Permission)) return false;
            Permission p = obj as Permission;
            return p.Name.Equals(Name);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}