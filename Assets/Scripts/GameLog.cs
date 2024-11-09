using UnityEngine;
public static class GameLog

{
    public static void LogMessage(string message)
    {
#if UNITY_EDITOR
        Debug.Log(message);
#endif
    }
    public static void LogMessage(string message,UnityEngine.Object context)
    {
#if UNITY_EDITOR
        Debug.Log(message,context);
#endif
    }
    public static void LogWarning(string message)
    {
#if UNITY_EDITOR
        Debug.LogWarning(message);
#endif
    }
    public static void LogError(string message)
    {
#if UNITY_EDITOR
        Debug.LogWarning(message);
#endif
    }

    public static void LogError(string message,UnityEngine.Object context)
    {
#if UNITY_EDITOR
        Debug.LogWarning(message,context);
#endif
    }

    public static void LogBreak(string message)
    {
#if UNITY_EDITOR
        LogMessage(message);
        Debug.Break();
#endif
    }
}