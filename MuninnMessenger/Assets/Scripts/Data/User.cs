public class User
{
    public long Id;
    public string Name;
    public string AvatarUrl;

    public static bool operator==(User a, User b)
    {
        if (a is null || b is null)
        {
            return false;
        }

        return a.Id == b.Id;
    }

    public static bool operator!=(User a, User b)
    {
        if (a is null || b is null)
        {
            return false;
        }

        return a.Id != b.Id;
    }

    public override bool Equals(object obj)
    {
        if (obj is User user)
        {
            return Id == user.Id;
        }

        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public override string ToString()
    {
        return $"[{Id}] {Name}";
    }
}