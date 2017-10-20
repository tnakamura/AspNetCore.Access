# AspNetCore.Contrib.Access

ASP.NET Core Middleware for limiting access based on IP address.

## Usage

```c#
public void Configure(IApplicationBuilder app)
{
    app.UseAccess(mappings =>
    {
        mappings.MapPath("/swagger", "127.0.0.1");
    });
}
```

## License

MIT

