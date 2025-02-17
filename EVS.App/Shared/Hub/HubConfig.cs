namespace EVS.App.Infrastructure.Notifiers;

public static class HubConfig
{
    public static string EVENT_HUB_URL = "/eventHub";
    public static bool HUB_ENABLE_DETAILED_ERRORS = true;
    public static int HUB_TIMEOUT_SECONDS = 60;
    public static long HUB_MAX_MESSAGE_SIZE = 1024 * 1024 * 5;  //5MB
}