module PocketApiAuthorization

open System
open Utils
open PocketApi

let autorizationCompleteCallbackUri = "https://getpocket.com/a/"

let getRequestTokenCode consumerKey = 
    let parameters = createParameters consumerKey
    parameters.Add("redirect_uri", autorizationCompleteCallbackUri)
    let response = Async.RunSynchronously(getPocketApiMethodContent "oauth/request" parameters)
    
    let requestTokenCode = 
        match response with
        | ParseRegex "code=(.*)" [ code ] -> code
        | _ -> String.Empty
    requestTokenCode

let getAuthorizeUrl consumerKey requestTokenCode = 
    sprintf "https://getpocket.com/auth/authorize?request_token=%s&redirect_uri=%s" requestTokenCode 
        autorizationCompleteCallbackUri

let authorize consumerKey requestTokenCode = 
    let parameters = createParameters consumerKey
    parameters.Add("code", requestTokenCode)
    let autorizationResponse = Async.RunSynchronously(getPocketApiMethodContent "oauth/authorize" parameters)
    
    let (userName, accessToken) = 
        match autorizationResponse with
        | ParseRegex "access_token=(.+)&username=(.+)" [ token; user ] -> (user, token)
        | _ -> ("", "")
    (userName, accessToken)
