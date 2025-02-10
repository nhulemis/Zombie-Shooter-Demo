// using System.Diagnostics;
// using UnityEngine;
//
// public static class Log
// {
//     [Conditional("DebugLog")]
//     public static void Debug(object message) => UnityEngine.Debug.Log(message);
//
//     [Conditional("DebugLog")]
//     public static void Debug(object message, UnityEngine.Object context)
//     {
//         UnityEngine.Debug.Log(message, context);
//     }
//
//     [Conditional("DebugLog")]
//     public static void Debug(object message, Color color)
//     {
//         UnityEngine.Debug.Log((object)Log.AddColorToMessage(message, color));
//     }
//
//     [Conditional("DebugLog")]
//     public static void DebugFormat(string format, params object[] args)
//     {
//         UnityEngine.Debug.LogFormat(format, args);
//     }
//
//     [Conditional("DebugLog")]
//     public static void DebugFormat(UnityEngine.Object context, string format, params object[] args)
//     {
//         UnityEngine.Debug.LogFormat(context, format, args);
//     }
//
//     [Conditional("DebugLog")]
//     public static void Warning(object message) => UnityEngine.Debug.LogWarning(message);
//
//     [Conditional("DebugLog")]
//     public static void Warning(object message, UnityEngine.Object context)
//     {
//         UnityEngine.Debug.LogWarning(message, context);
//     }
//
//     [Conditional("DebugLog")]
//     public static void WarningFormat(string format, params object[] args)
//     {
//         UnityEngine.Debug.LogWarningFormat(format, args);
//     }
//
//     [Conditional("DebugLog")]
//     public static void WarningFormat(UnityEngine.Object context, string format, params object[] args)
//     {
//         UnityEngine.Debug.LogWarningFormat(context, format, args);
//     }
//
//     [Conditional("DebugLog")]
//     public static void Error(object message) => UnityEngine.Debug.LogError(message);
//
//     [Conditional("DebugLog")]
//     public static void Error(object message, UnityEngine.Object context)
//     {
//         UnityEngine.Debug.LogError(message, context);
//     }
//
//     [Conditional("DebugLog")]
//     public static void ErrorFormat(string format, params object[] args)
//     {
//         UnityEngine.Debug.LogErrorFormat(format, args);
//     }
//
//     [Conditional("DebugLog")]
//     public static void ErrorFormat(UnityEngine.Object context, string format, params object[] args)
//     {
//         UnityEngine.Debug.LogErrorFormat(context, format, args);
//     }
//
//     [Conditional("DebugLog")]
//     public static void Exception(System.Exception exception) => UnityEngine.Debug.LogException(exception);
//
//     [Conditional("DebugLog")]
//     public static void Exception(System.Exception exception, UnityEngine.Object context)
//     {
//         UnityEngine.Debug.LogException(exception, context);
//     }
//
//     private static string AddColorToMessage(object message, Color color)
//     {
//         return string.Format("<color=#{0}>{1}</color>", (object)ColorUtility.ToHtmlStringRGB(color), message);
//     }
// }