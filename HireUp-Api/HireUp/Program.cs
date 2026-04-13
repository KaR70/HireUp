using HireUp;
using HireUp.Database;
using HireUp.Database.Interfaces;
using HireUp.Database.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDependecies(builder.Configuration);

// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ISkillRepository, SkillRepository>();
builder.Services.AddScoped<IJobListingRepository, JobListingRepository>();
builder.Services.AddScoped<IMockInterviewRepository, MockInterviewRepository>();
builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();
builder.Services.AddScoped<ISavedJobRepository, SavedJobRepository>();


// Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

#region Creating Database in Docker
if (Environment.GetEnvironmentVariable("RUN_MIGRATIONS_ON_STARTUP") == "true")
{
    try
    {
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await dbContext.Database.MigrateAsync();
        }
    }
    catch (Exception ex)
    {
        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}
#endregion

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await DataSeeder.SeedAllAsync(services);
}
app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseRouting(); 
app.UseAuthorization();

app.MapControllers();
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // التأكد من وجود قاعدة البيانات
    context.Database.EnsureCreated();

    // 1. إضافة مستخدم تجريبي لو الجدول فاضي
    var user = context.Users.FirstOrDefault();
    if (user == null)
    {
        user = new ApplicationUser { UserName = "rehab@test.com", Email = "rehab@test.com" };
        context.Users.Add(user);
        context.SaveChanges();
    }

    // 2. إضافة وظيفة تجريبية لو الجدول فاضي
    var job = context.JobListings.FirstOrDefault();
    if (job == null)
    {
        job = new JobListing { Title = "Software Engineer", Description = "Test Job" };
        context.JobListings.Add(job);
        context.SaveChanges();
    }

    // 3. اطبع الـ IDs الحقيقية اللي اتعملت
    Console.WriteLine("=====================================");
    Console.WriteLine("SUCCESS! USE THESE IN SWAGGER:");
    Console.WriteLine($"USER ID: {user.Id}");
    Console.WriteLine($"JOB ID: {job.Id}");
    Console.WriteLine("=====================================");
}

app.Run();