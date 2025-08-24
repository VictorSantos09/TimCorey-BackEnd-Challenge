using ChallengeCore;
using ChallengeUI.EndPoints;

namespace ChallengeUI;

public static class Configuration
{
    public static WebApplication ConfigureUI(this WebApplication app)
    {
        app.MapUser();
        app.MapProducts();
        app.MapPurchases();
        
        app.AddCore();

        return app;
    }
}
