using Holoon.MjmlToHtml;
using Holoon.MjmlToHtml.Example;

Console.WriteLine("MjmlToHtml - Start");

var data = GetData();
static Model GetData() => new()
{
    Link = "https://github.com/Holoon/MjmlToHtml",
    Username = "Arthur<br/>Dent",
    TestObject = new Role { MemberId = 42, MemberName = "Foo", RoleId = 1, RoleName = "Bar" }, 
    ProjectName = "Example-Test-Project"
};

var options = new Options
{
    I18NBasePath = @"locales",
    OnScribanErrors = (errors) => errors.ToList().ForEach(error => Console.WriteLine(error.ToString())),
    OnMjmlErrors = (errors) => errors.ToList().ForEach(error => Console.WriteLine(error.ToString())),
    TemplateLoader = new DefaultTemplateLoader()
};
options.TemplateCustomFunctions.Add("role", (Func<Role?, string>)FormatHtml);

var generator = new MailGenerator(options);
var mail = generator.GetMail(@"welcome.mjml.sbntxt", data, language: "en", Format.Mjml);

// TODO: Send mail

Console.WriteLine("MjmlToHtml - End");

static string FormatHtml(Role? role)
{
    var referenceStr = String.Empty;

    if (role?.RoleId != null)
        referenceStr += $"<mj-text style=\"color: #3BD75E\">@{role.RoleName}</mj-text>"; 
    if (role?.MemberId != null) 
        referenceStr += $"<mj-text style=\"font-weight: bold; color: #000000\">.{role.MemberName}</mj-text>";

    return referenceStr;
}
