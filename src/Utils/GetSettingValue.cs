using System;
using System.Reflection;
using WSMR.Core.Managers;
namespace WSMR.Utils;

public static class GetSettingValue
{
    public static T Get<T>(string setting)
    {
        var settings = SettingsManager.Instance.CurrentSettings;

        var property = typeof(AppSaveData).GetProperty(
            setting,
            BindingFlags.Public | BindingFlags.Instance
        );
        
        return (T)property.GetValue(settings);
    }
}