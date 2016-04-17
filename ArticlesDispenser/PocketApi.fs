module PocketApi

open System
open System.Collections.Generic
open System.Net.Http

let createParameters consumerKey = 
    let methodParameters = new Dictionary<string, string>()
    methodParameters.Add("consumer_key", consumerKey)
    methodParameters

let createAccessParameters consumerKey accessKey = 
    let parameters = createParameters consumerKey
    parameters.Add("access_token", accessKey)
    parameters

let createRequest methodParameters (methodName : string) = 
    let request = new HttpRequestMessage(HttpMethod.Post, methodName)
    request.Content <- new FormUrlEncodedContent(methodParameters)
    request

let getResponseAsync request = 
    async { 
        let client = new HttpClient()
        client.BaseAddress <- new Uri("https://getpocket.com/v3/")
        use! response = Async.AwaitTask<HttpResponseMessage>(client.SendAsync(request))
        let! content = Async.AwaitTask<string>(response.Content.ReadAsStringAsync())
        return content
    }

let getPocketApiMethodContent methodName methodParameters = 
    async { 
        use request = createRequest methodParameters methodName
        let! content = getResponseAsync request
        return content
    }

let getItemsContent parseArticles consumerKey accessKey = 
    async { 
        let parameters = createAccessParameters consumerKey accessKey
        parameters.Add("detailType", "complete")
        let! content = getPocketApiMethodContent "get" parameters
        let articles = content |> parseArticles
        return articles
    }
