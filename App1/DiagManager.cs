using SynergizDiag;
using SynergizDiag.Enums;
using SynergizDiag.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

public class DiagManager  {
    private List<DiagInfoServiceBase> services;
    private string adress = DevicePortalAPIHelper.EMPTY_IP;
    public CollectingType CollectingBy;

    public List<DiagInfoServiceBase> Services
    {
        get
        {
            if (services == null)
            {
                services = new List<DiagInfoServiceBase>();
            }
            
            return services;
        }

        set
        {
            services = value;
        }
    }


    #region boolean config attributes
    public bool GatherSystemInfo = true;
    public bool GatherNetworkInfo = true;
    public bool GatherHolographicOSInfo = true;
    public bool GatherPowerInfo = true;
    #endregion
    /// <summary>
    /// Fill service list according to the selected Collecting type
    /// </summary>
    protected void fillServices()
    {
        Services.Clear();
        switch (CollectingBy)
        {
            case CollectingType.ByAPI:
                if (GatherSystemInfo) Services.Add(new SystemInfoServiceAPI());
                if (GatherNetworkInfo) Services.Add(new NetworkingInfoServiceAPI());
                if (GatherHolographicOSInfo) Services.Add(new HolographicOsInfoServiceAPI());
                if (GatherPowerInfo) Services.Add(new PowerInfoServiceAPI());
                break;
            case CollectingType.ByLocalClass:
                if (GatherPowerInfo) Services.Add(new PowerInfoServiceLocal());
                if (GatherSystemInfo) Services.Add(new SystemInfoServiceLocal());
                if (GatherNetworkInfo) Services.Add(new NetworkingInfoServiceLocal());
                break;
            default:
                break;
        }
    }

    public void SetConnectivity(DeviceConnectedBy connectedBy, String Adress=DevicePortalAPIHelper.EMPTY_IP)
    {
        DevicePortalAPIHelper.SetConnectivity(connectedBy,Adress);
    }

    /// <summary>
    /// Set the credentials to access API
    /// </summary>
    /// <param name="_login"></param>
    /// <param name="_password"></param>
    public void SetCredentials(string _login, string _password)
    {
        DevicePortalAPIHelper.SetCredentials(_login, _password);
    }

    public async Task<Dictionary<string, string>> GetAllInfos()
    {
        fillServices();
        Dictionary<string, string> infos = new Dictionary<string, string>();

        foreach (DiagInfoServiceBase item in Services)
        {
            try
            {
                    infos = await item.GetInfos(infos);
                
            }
            catch (Exception e)
            {
                infos.Add("Error with ", item.GetLabel());
                infos.Add("Error : ", e.Message);
            }
        }
        return infos;
    }

    /// <summary>
    /// Get infos in about a single category
    /// </summary>
    /// <param name="service"></param>
    /// <returns></returns>
    protected async Task<Dictionary<string, string>> getInfo(DiagInfoServiceBase service)
    {
        Dictionary<string, string> infos = new Dictionary<string, string>();
        try
        {
            infos = await service.GetInfos(infos);

        }
        catch (Exception e)
        {
            infos.Add("Error with ", service.GetLabel());
            infos.Add("Error : ", e.Message);
        }
        return infos;

    }

    #region Specific category methods (delegate GetInfosMethod)
    public delegate Task<Dictionary<string, string>> GetInfosMethod();

    public async Task<Dictionary<string, string>> GetNetworkInfos()
    {
        return await getInfo(CollectingBy == CollectingType.ByAPI ? (DiagInfoServiceBase)new NetworkingInfoServiceAPI() : new NetworkingInfoServiceLocal());
    }

    public async Task<Dictionary<string, string>> GetSystemInfos()
    {
        return await getInfo(CollectingBy == CollectingType.ByAPI ? (DiagInfoServiceBase)new SystemInfoServiceAPI() : new SystemInfoServiceLocal());
    }

    public async Task<Dictionary<string, string>> GetPowerInfos()
    {
        return await getInfo(CollectingBy == CollectingType.ByAPI ? (DiagInfoServiceBase)new PowerInfoServiceAPI() : new PowerInfoServiceLocal());
    }

    public async Task<Dictionary<string, string>> GetHolographicOSInfos()
    {
        Dictionary<string, string> infos = new Dictionary<string, string>();

        if (CollectingBy == CollectingType.ByAPI)
        {
            infos = await getInfo(new HolographicOsInfoServiceAPI());
        }
        else
        {
            infos.Add("Not Available", "these Informations can only be gathered by API");
        }

        return infos;
    }

    #endregion
}
