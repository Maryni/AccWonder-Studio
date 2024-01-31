using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AppsFlyerSDK;
using System;

public class AppsFlyerObjectScript : MonoBehaviour, IAppsFlyerConversionData
{

    public string devKey;
    public string appID;
    public string UWPAppID;
    public string macOSAppID;
    public bool isDebug;
    public bool getConversionData;
    public bool isOK;
    public string URLFAILURE;

    public Dictionary<string, object> conversionDataDictionaryAccWonder = new Dictionary<string, object>();
    public Dictionary<string, object> DataAccWonder = new Dictionary<string, object>();
    private string packageName;
    private string signalID;
    public Action onReady;

    private void Start()
    {
        AppsFlyer.setIsDebug(isDebug);
#if UNITY_WSA_10_0 && !UNITY_EDITOR
        AppsFlyer.initSDK(devKey, UWPAppID, getConversionData ? this : null);
#elif UNITY_STANDALONE_OSX && !UNITY_EDITOR
    AppsFlyer.initSDK(devKey, macOSAppID, getConversionData ? this : null);
#elif UNITY_IOS
        AppsFlyer.initSDK(devKey, appID, getConversionData ? this : null);
#elif UNITY_ANDROID
        AppsFlyer.initSDK(devKey, null, getConversionData ? this : null);
#endif
        AppsFlyer.startSDK();
    }

    public void onConversionDataSuccess(string conversionData)
    {
        AppsFlyer.AFLog("didReceiveConversionData", conversionData);
        conversionDataDictionaryAccWonder = AppsFlyer.CallbackStringToDictionary(conversionData);

        if (isOK)
        {
            conversionDataDictionaryAccWonder["dev_key"] = devKey;
            conversionDataDictionaryAccWonder["app_id"] = packageName;
            conversionDataDictionaryAccWonder["appsflyer_id"] = AppsFlyer.getAppsFlyerId();
            conversionDataDictionaryAccWonder["signal_app_id"] = signalID;

            DataAccWonder = conversionDataDictionaryAccWonder;
            onReady?.Invoke();
        }
    }

    public void onConversionDataFail(string error)
    {
        AppsFlyer.AFLog("didReceiveConversionDataWithError", error);
    }

    public void onAppOpenAttribution(string attributionData)
    {
        AppsFlyer.AFLog("onAppOpenAttribution", attributionData);
        Dictionary<string, object> attributionDataDictionary = AppsFlyer.CallbackStringToDictionary(attributionData);
    }

    public void onAppOpenAttributionFailure(string error)
    {
        AppsFlyer.AFLog("onAppOpenAttributionFailure", error);
    }

    public void SetOnReady(Action action) => onReady += action;

    public void AddValueToDictionary(string package, string signal)
    {
        packageName = package;
        signalID = signal;
        isOK = true;
        StartCoroutine(StartData());
    }

    private IEnumerator StartData()
    {
        yield return new WaitForSeconds(2f);
        AppsFlyer.getConversionData(name);
    }
}
