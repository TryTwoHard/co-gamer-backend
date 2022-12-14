namespace Tournament.API.Extensions;

public static class ApplicationExtensions
{
    public static void UseInfrastructure(this WebApplication app)
    {

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseExceptionHandler("/error");
        // app.UseMiddleware<ExceptionMiddleware>();
        // app.UseHttpsRedirection();

        // app.UseAuthorization();

        app.MapControllers();
    }
}