using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NyscIdentify.Common.Infrastructure.Extensions
{
    public static class RestSharpExtensions
    {
        public static async Task<IRestResponse> MakeRequest(this IRestClient client, 
            string url, object body, Method type = Method.GET)
        {
            IRestResponse response = null;

            try
            {
                IRestRequest request = new RestRequest(url, type);
                request.AddJsonBody(body);

                response = await client.ExecuteTaskAsync(request);

                Core.Log.Debug(response.Content);
            }
            catch (Exception ex)
            {
                Core.Log.Error($"An error occured while creating making a {type} request to ({url})\n{ex}");
            }

            return response;
        }

        public static async Task<IRestResponse> MakeGetRequest(this IRestClient client, 
            string url, params (string Name, string Value)[] queryParameters)
        {
            IRestResponse response = null;

            try
            {
                IRestRequest request = new RestRequest(url, Method.GET);

                foreach (var param in queryParameters)
                    request.AddQueryParameter(param.Name, param.Value);

                response = await client.ExecuteTaskAsync(request);

                Core.Log.Debug(response.Content);
            }
            catch (Exception ex)
            {
                Core.Log.Error($"An error occured while creating making a {nameof(Method.GET)} request to ({url})\n{ex}");
            }

            return response;
        }

        public static async Task<T> Get<T>(this IRestClient client, string url, params (string Name, string Value)[] queryParameters)
        {
            IRestResponse response = null;
            T data = default(T);

            try
            {
                IRestRequest request = new RestRequest(url, Method.GET);

                foreach (var param in queryParameters)
                    request.AddQueryParameter(param.Name, param.Value);

                response = await client.ExecuteTaskAsync(request);


                Core.Log.Debug($"Response From ({url})");
                Core.Log.Debug(response.Content);

                data = JsonConvert.DeserializeObject<T>(response.Content);
            }
            catch (Exception ex)
            {
                Core.Log.Error($"An error occured while creating making a {nameof(Method.GET)} request to ({url})\n{ex}");
            }

            return data;
        }
    }
}
