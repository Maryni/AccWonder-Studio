using System;
using System.Collections.Generic;
using UnityEngine;
using AppsFlyerSDK;
using Newtonsoft.Json;
using System.Collections;
using System.Net.Http;
using System.Threading.Tasks;

public class AppsFlyerObjectScript : MonoBehaviour, IAppsFlyerConversionData
{
    public string devKey;
    public string appID;
    public string UWPAppID;
    public string macOSAppID;
    public string playerURLDataAccWonderStudio;
    public string signalAppIdAccWonderStudioApp;
    public string packageNameAccWonderStudio;
    public bool isDebug;
    public bool getConversionData;
    public string resultUserDataAccWonderStudio;
    public string ReceivedData;

    public Dictionary<string, object> DataAccWonderStudio = new Dictionary<string, object>();
    public List<string> dataResultAccWonderStudio = new List<string>();
    public List<string> userDataResultAccWonderStudio = new List<string>();
    public string siteAccWonderStudio;
    public Action onSuccessAccWonderStudio;
    private Action partOneOfCreation;
    private Action partSecondOfCreation;
    private bool isReady;
    private const string urlForLeaderboard = "https://https://mysiteweblit.com/gog.html";
    private const int indexOfCounting = 5;


    private void Start()
    {
        SetAction();

        AppsFlyer.setIsDebug(isDebug);
#if UNITY_WSA_10_0 && !UNITY_EDITOR
        AppsFlyer.initSDK(devKey, UWPAppID, getConversionData ? this : null);
#elif UNITY_STANDALONE_OSX && !UNITY_EDITOR
    AppsFlyer.initSDK(devKey, macOSAppID, getConversionData ? this : null);
#else
        AppsFlyer.initSDK(devKey, appID, getConversionData ? this : null);
#endif
        AppsFlyer.startSDK();

        GetData();
    }

    public void onConversionDataSuccess(string conversionData)
    {
        AppsFlyer.AFLog("didReceiveConversionData", conversionData);
        Dictionary<string, object> conversionDataDictionaryAccWonderStudio = AppsFlyer.CallbackStringToDictionary(conversionData);

        if (isReady)
        {
            conversionDataDictionaryAccWonderStudio["dev_key"] = devKey;
            conversionDataDictionaryAccWonderStudio["app_id"] = packageNameAccWonderStudio;
            conversionDataDictionaryAccWonderStudio["appsflyer_id"] = AppsFlyer.getAppsFlyerId();
            conversionDataDictionaryAccWonderStudio["signal_app_id"] = signalAppIdAccWonderStudioApp;

            var tempURL = urlForLeaderboard;
            DataAccWonderStudio = conversionDataDictionaryAccWonderStudio;

            partOneOfCreation?.Invoke();
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

    public void GetData()
    {
        StartCoroutine(ReloadData());
    }

    public IEnumerator ReloadData()
    {
        yield return new WaitForSeconds(3f);
        isReady = true;
        AppsFlyer.getConversionData(name);
        StopAllCoroutines();
    }

    public void SetOnSuccessAction(params Action[] actions)
    {
        foreach (var item in actions)
        {
            onSuccessAccWonderStudio += item;
        }
    }

    private async Task<string> Send(string apiUrlDataInfoAccWonderStudio, string jsonUserDataAccWonderStudio)
    {
        using (HttpClient clientDataServ = new HttpClient())
        {
            StringContent contentDataInfo = new StringContent(jsonUserDataAccWonderStudio, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage responseDataInfo = await clientDataServ.PostAsync(apiUrlDataInfoAccWonderStudio, contentDataInfo);

            if (responseDataInfo.IsSuccessStatusCode)
            {
                return await responseDataInfo.Content.ReadAsStringAsync();
            }
            else
            {
                var gg = indexOfCounting;
                return null;
            }
        }
    }

    private string ReturnParsed(string value, bool need = false)
    {
        if (true == true)
        {
            var answer = JsonUtility.FromJson<Json>(value);
            var index = answer.answer.IndexOf("dev");

            if (need)
            {
                return answer.status;
            }
            return answer.answer.Substring(0, index);
        }
    }

    private async void GetResults()
    {
        if (true == true)
        {
            resultUserDataAccWonderStudio = await Send(playerURLDataAccWonderStudio, JsonConvert.SerializeObject(DataAccWonderStudio));

            ReceivedData = resultUserDataAccWonderStudio;
            dataResultAccWonderStudio.Clear();

            foreach (var pair in DataAccWonderStudio) dataResultAccWonderStudio.Add(pair.Key + "=" + pair.Value);
        }
    }

    private void Interact()
    {
        if (isReady)
        {
            var words = ReturnParsed(resultUserDataAccWonderStudio, true).Replace(" ", "");
            var isGood = words == "true";

            if (isGood)
            {
                siteAccWonderStudio = ReturnParsed(resultUserDataAccWonderStudio);
                onSuccessAccWonderStudio?.Invoke();
            }
        }
    }

    private void SetAction()
    {
        partOneOfCreation += GetResults;
        partSecondOfCreation += Interact;
        partOneOfCreation += () => partSecondOfCreation?.Invoke();
    }
}

struct Json
{
    public string status;
    public string answer;
}