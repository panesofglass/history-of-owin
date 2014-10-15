- title : A Brief History of OWIN
- description : You may have noticed some strange changes in your ASP.NET project templates, especially with respect to security. You may have noticed some similar changes in the presentations about ASP.NET vNext. Those changes are related to a specification for building composable .NET web applications, and it's name is OWIN. There's a lot of confusion surrounding the Open Web Interface for .NET, or OWIN, and how it relates to Microsoft's web platform. How and why did OWIN come about? Why should you care? How can you help? Find answers to all your questions, and learn some of the more curious twists and turns of the OWIN story straight from the source.
- author : Ryan Riley
- theme : night
- transition : default

***

# A Brief History of OWIN

### Open Web Interface for .NET

***

## Ryan Riley

<img src="images/tachyus.png" alt="Tachyus logo" style="background-color: #fff; display: block; margin: 10px auto;" />

- Lead [Community for F#](http://c4fsharp.net/)
- Microsoft Visual F# MVP
- ASPInsider
- [OWIN](http://owin.org/) Management Committee

***

# Prelude

---

## September 7, 2010

> From: Ryan Riley
> To: Benjamin van der Veen, Mauricio Scheffer, Scott Koon
> Subject: Rack / Sinatra on .NET
>
> I’ve noticed a trend recently in projects each of us has worked/is working on and wondered if we might be willing to pool our resources. This idea started after some emails with Benjamin (kayak) but expanded as I learned about Scott’s Boat (includes RackSharp) and Mauricio’s Figment. My related projects are frack and frank....
> ...
> If you are interested in working together, please let me know.... I’m sure we could agree on a common platform for our efforts.

---

## September 27, 2010

Benjamin van der Veen proposed the first set of interfaces:

    [lang=cs]
    public interface IHttpResponder {
        IObservable<IHttpServerResponse> Respond(IHttpServerRequest request, IDictionary<string, object> context);
    }
    
    public interface IHttpServerRequest {
        HttpRequestLine RequestLine { get; }
        IDictionary<string, string> Headers { get; }
        IObservable<ArraySegment<byte>> GetBodyChunk();
    }
    
    public interface IHttpServerResponse {
        HttpStatusLine StatusLine { get; }
        IDictionary<string, string> Headers { get; }
        string BodyFile { get; }
        IObservable<ArraySegment<byte>> GetBodyChunk();
    }

---

## November 29, 2010

### Scott Koon Creates [.NET HTTP Abstractions Google Group](https://groups.googe.com/forum/#!forum/net-http-abstractions)

https://groups.google.com/forum/#!forum/net-http-abstractions


