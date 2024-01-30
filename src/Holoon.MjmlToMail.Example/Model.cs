namespace Holoon.MjmlToHtml.Example;
public class Model
{
    public string? Username { get; set; }
    public string? Link { get; set; }
    public string? ProjectName { get; set; }
    public Role? TestObject { get; set; }
}
public class Role
{
    public int? RoleId { get; set; }
    public int? MemberId { get; set; }
    public string? RoleName { get; set; }
    public string? MemberName { get; set; }
}
