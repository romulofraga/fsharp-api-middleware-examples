namespace fsharpApi.Controllers

open System
open System.Collections.Generic
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open fsharpApi

[<ApiController>]
[<Route("[controller]")>]
type WeatherForecastController(logger: ILogger<WeatherForecastController>) =
    inherit ControllerBase()

    let summaries =
        [| "Freezing"
           "Bracing"
           "Chilly"
           "Cool"
           "Mild"
           "Warm"
           "Balmy"
           "Hot"
           "Sweltering"
           "Scorching" |]

    [<HttpGet>]
    member controller.Get() =
        let rng = System.Random()

        [| for index in 0 .. summaries.Count() - 1 ->
               { Date = DateTime.Now.AddDays(float index)
                 TemperatureC = rng.Next(-20, 55)
                 Summary = summaries.[index] } |]

    [<HttpPost>]
    member controller.Post([<FromBody>] summary: string) =
        let updatedSummaries = Array.append summaries [| summary |]
        Console.ForegroundColor <- ConsoleColor.DarkYellow
        Console.WriteLine(summaries)
        let rng = System.Random()

        [| for index in 0 .. updatedSummaries.Length - 1 ->
               { Date = DateTime.Now.AddDays(float index)
                 TemperatureC = rng.Next(-20, 55)
                 Summary = updatedSummaries.[index] } |]
