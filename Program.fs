namespace fsharpApi

#nowarn "20"

open System
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.HttpLogging
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.Primitives

module Program =
    open System.Globalization
    open System.Diagnostics
    open System.Threading.Tasks

    type StopWatchClassMiddleware(next: RequestDelegate) =
        member this.Invoke(context: HttpContext) : Task =
            task {
                let sw = new Stopwatch()
                sw.Start()
                do! context |> next.Invoke |> Async.AwaitTask
                sw.Stop()
                printfn "Elapsed class mode: %A" sw.Elapsed
            }

    type RequestCultureMiddleware(next: RequestDelegate) =
        member this.InvokeAsync(context: HttpContext) =
            async {
                let cultureQuery = context.Request.Query.["culture"]

                if not (String.IsNullOrWhiteSpace cultureQuery) then
                    let culture = new CultureInfo(cultureQuery)
                    CultureInfo.CurrentCulture <- culture
                    CultureInfo.CurrentUICulture <- culture

                do! next.Invoke(context) |> Async.AwaitTask
                printfn "Hello World!"
            }

    let stopWatchFunctionalMiddleware (context: HttpContext) (next: RequestDelegate) : Task =
        task {
            let sw = new Stopwatch()
            sw.Start()
            do! context |> next.Invoke |> Async.AwaitTask
            sw.Stop()
            printfn "Elapsed functional mode: %A" sw.Elapsed
        }


    let exitCode = 0


    [<EntryPoint>]
    let main args =

        let builder = WebApplication.CreateBuilder(args)

        builder.Services.AddControllers()

        // builder.Services.AddHttpLogging(fun logging -> logging.LoggingFields <- HttpLoggingFields.ResponseBody)

        builder.Services.AddEndpointsApiExplorer()
        builder.Services.AddSwaggerGen()

        let app = builder.Build()

        // app.UseHttpLogging()

        // function mode
        app.Use(stopWatchFunctionalMiddleware)

        // class mode
        app.UseMiddleware<StopWatchClassMiddleware>()

        app.UseSwagger()
        app.UseSwaggerUI()

        app.UseAuthentication()
        app.UseAuthorization()

        app.MapControllers()

        app.Run()

        exitCode
