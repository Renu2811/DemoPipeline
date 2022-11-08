

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var logger = LoggerFactory.Create(config =>
{
    config.AddConsole();
}).CreateLogger("Program");


var app = builder.Build();

app.Map("/map1", HandleMapTest1);

app.Map("/map2", HandleMapTest2);

app.Map("/seg", HandleMultiSeg);


app.MapWhen(context => context.Request.Query.ContainsKey("branch"), HandleBranch);


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days.You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

static void HandleBranch(IApplicationBuilder app)
{
    app.Run(async context =>
    {
        var branch1 = context.Request.Query["branch"];
        await context.Response.WriteAsync($"Branch = {branch1}");
    });
}

app.Use(async (context, next) =>
    {
        logger.LogInformation("Request");
        await next();
        logger.LogInformation("Response");
    });

app.Run(async context =>
{
    await context.Response.WriteAsync("Request and Response");
});


//app.Use(async (context, next) =>
//{
//    await next.Invoke();

//});

//app.Run(async context =>
//{
//    await context.Response.WriteAsync("Hello world!");
//});

//app.Run();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.Run();


//By routing ,we will get the appropriate response



static void HandleMultiSeg(IApplicationBuilder app)
{
    app.Run(async context =>
    {
        await context.Response.WriteAsync("Segment");
    });
}


static void HandleMapTest1(IApplicationBuilder app)
{
    app.Run(async context =>
    {
        await context.Response.WriteAsync("MapTest 1");
    });
}

static void HandleMapTest2(IApplicationBuilder app)
{
    app.Run(async context =>
    {
        await context.Response.WriteAsync("MapTest 2");
    });
}

