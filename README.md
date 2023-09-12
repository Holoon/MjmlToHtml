<p align="center">
  <img src="https://github.com/Holoon/MjmlToHtml/raw/main/doc/logo.png" width="389" title="MJML Logo">
</p>

# MjmlToHtml

Template tool for creating translatable e-mails by combining the three tools: scriban, I18Next.Net and mjml-net.
	
## Installation 

```
Install-Package Holoon.MjmlToHtml
```

Nuget package: https://www.nuget.org/packages/Holoon.MjmlToHtml/

## Usage

### Basic usage

```c#
var data = GetData();
static Model GetData() => new()
{
    Link = "https://github.com/Holoon/MjmlToHtml",
    Username = "Arthur Dent"
};

var options = new Options
{
    I18NBasePath = @"locales"
};

var generator = new MailGenerator(options);
var mail = generator.GetMail(@"welcome.mjml.sbntxt", data, language: "en", Format.Mjml);
```

With:

- welcome.mjml.sbntxt
```html
<mjml>
	<mj-head>
		<mj-title>
			{{ t 'welcome:header-title' }}
		</mj-title>
		<mj-preview>
			{{ t 'welcome:header-subtitle' }}
		</mj-preview>
	</mj-head>
	<mj-body>
		<mj-wrapper>
		
			<mj-section>
				<mj-column>
					<mj-text>
						{{ t 'commons:hello' }} {{ username }},
					</mj-text>
					<mj-text>
						{{ t 'commons:your-login-is' }} 
						<b class="highlight type-member">
							{{ username }}
						</b>
					</mj-text>
					<mj-button href="{{ link }}">
						{{ t 'welcome:button-initialize-account' }}
					</mj-button>
				</mj-column>
			</mj-section>

		</mj-wrapper>
	</mj-body>
</mjml>
```

- locales\en\commons.json
```json
{
	"hello": "Hello",
	"your-login-is": "Your login is:"
}
```

- locales\en\welcome.json
```json
{
	"button-initialize-account": "Sign In",
	"header-subtitle": "Your account has just been created on Example-Test-Project",
	"header-title": "Welcome",
}
```

### Custom functions

You can add custom functions to templates:

```c#
var data = new Model()
{
	// ...
    TestObject = new Role { MemberId = 42, MemberName = "Foo", RoleId = 1, RoleName = "Bar" }
};
var options = new Options
{
    // ...
};
options.TemplateCustomFunctions.Add("role", (Func<Role?, string>)FormatHtml);

var generator = new MailGenerator(options);
var mail = generator.GetMail(@"template.mjml.sbntxt", data, language: "en", Format.Mjml);

static string FormatHtml(Role? role)
{
    var referenceStr = String.Empty;

    if (role?.RoleId != null)
        referenceStr += $"<mj-text style=\"color: #3BD75E\">@{role.RoleName}</mj-text>"; 
    if (role?.MemberId != null) 
        referenceStr += $"<mj-text style=\"font-weight: bold; color: #000000\">.{role.MemberName}</mj-text>";

    return referenceStr;
}
```

With, in yout template
```html
// ...
<mj-text>
	{{ role test_object }}
</mj-text>
// ...
```

A complete example can be found here: https://github.com/Holoon/MjmlToHtml/tree/main/src/Holoon.MjmlToMail.Example

### Using {{ include sub-template.sbntxt }}

If your template contains sub-templates, in accordance with the Scriban documentation, you must provide a `TemplateLoader`.
Example:

```html
// ...
<mj-text>
	{{ include 'templates/footer.sbntxt' }}
</mj-text>
// ...
```

```c#
var options = new Options
{
    // ...
    TemplateLoader = new DefaultTemplateLoader()
};
var generator = new MailGenerator(options);
```

Or, for example:

```html
// ...
<mj-text>
	{{ include 'footer.sbntxt' }}
</mj-text>
// ...
```

```c#
var options = new Options
{
    // ...
    TemplateLoader = new DefaultTemplateLoader("templates")
};
var generator = new MailGenerator(options);
```

Or, of course, you can provide your own implementation of `ITemplateLoader`. 

## Quick Links
	
- scriban: https://github.com/scriban/scriban
- I18Next.Net: https://github.com/DarkLiKally/I18Next.Net
- mjml-net: https://github.com/SebastianStehle/mjml-net
	
## Contributing

If you'd like to contribute, please fork the repository and use a feature branch. Pull requests are welcome. Please respect existing style in code.

## Licensing

The code in this project is licensed under BSD-3-Clause license.
