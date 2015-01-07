- title : A Brief History of OWIN
- description : You may have noticed some strange changes in your ASP.NET project templates, especially with respect to security. You may have noticed some similar changes in the presentations about ASP.NET vNext. Those changes are related to a specification for building composable .NET web applications, and it's name is OWIN. There's a lot of confusion surrounding the Open Web Interface for .NET, or OWIN, and how it relates to Microsoft's web platform. How and why did OWIN come about? Why should you care? How can you help? Find answers to all your questions, and learn some of the more curious twists and turns of the OWIN story straight from the source.
- author : Ryan Riley
- theme : night
- transition : default

***

# A Brief History of OWIN

## Open Web Interface for .NET

***

## Ryan Riley

<img src="images/tachyus.png" alt="Tachyus logo" style="background-color: #fff; display: block; margin: 10px auto;" />

- Lead [Community for F#](http://c4fsharp.net/)
- Microsoft Visual F# MVP
- ASPInsider
- [OWIN](http://owin.org/) Management Committee

***

# Why OWIN?

***

## Problem: Desire for Fast and Light-weight Frameworks and Servers

* ASP.NET was "too heavy"
* IIS was "too slow"

---

## Solutions

* Boat (repo removed)
* [Figment](http://bugsquash.blogspot.com/2010/08/figment-web-dsl-for-f.html)
* [Frank](http://frankfs.net/)
* [Kayak](http://web.archive.org/web/20101102133523/http://kayakhttp.com/)
* [NancyFx](http://nancyfx.com)
* [Nina](http://jondot.github.io/nina/) (no updates in 4 years)
* [OpenRasta](http://openrasta.org/)
* [Suave](http://suave.io/)
* [WCF Web API](https://wcf.codeplex.com/)

***

## Problem: Repetition

* [Sinatra](http://www.sinatrarb.com/) clones
* ASP.NET adapters
* Self-host adapters (esp. `HttpListener`)

---

## Solution: Collaborate

---

## September 7, 2010

> I’ve noticed a trend recently in projects each of us has worked/is working on and wondered if we might be willing to pool our resources....
>
> ...
>
> If you are interested in working together, please let me know.... I’m sure we could agree on a common platform for our efforts.

- [Ryan Riley](https://twitter.com/panesofglass) to [Benjamin van der Veen](https://twitter.com/bvanderveen), [Mauricio Scheffer](https://twitter.com/mausch), and [Scott Koon](https://twitter.com/lazycoder)

---

## September 27, 2010

Benjamin van der Veen proposed the first set of interfaces:

    [lang=cs]
    public interface IHttpResponder {
        IObservable<IHttpServerResponse> Respond(
            IHttpServerRequest request,
            IDictionary<string, object> context);
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

Scott Koon creates [.NET HTTP Abstractions](https://groups.googe.com/forum/#!forum/net-http-abstractions)

https://groups.google.com/forum/#!forum/net-http-abstractions

***

## Problem: Opinions and Existing Implementations

---

## Solution: Define a Common Interface

* [Rack](https://rack.github.io/) (Ruby)
* [WSGI](http://wsgi.readthedocs.org/en/latest/#) (Python)

---

## Consensus to share server wrappers

***

# Progress!

---

## Name: Open Web Interface for .NET (OWIN)

---

## Specification writing begins

---

## BHAG: Run ASP.NET MVC on OWIN

***

## Problem: Trouble in Paradise

---

## Troublemakers

* Object-oriented faction
* Functional faction
* Dynamic faction

---

## Disagreement on library dependency

---

## Disagreement on interface types

---

## Solution : No Dependency Required

---

## Solution : Use Delegates and FCL Types

---

## What about the Iron- Languages?

`IDictionary<string, object>` to the rescue!

***

## Problem: How to Represent Async Access to Message Body?

---

## Proposed Solution #1: `Stream`

* Rejected
* Considered "too heavy"
* Async access considered "too slow"

---

## Proposed Solution #2: `IObservable<T>`

* Rejected
* Back-pressure a concern
* Required library dependency for .NET 3.5

---

## Proposed Solution #3: `Task<T>`

* Rejected
* Required .NET 4.0
* Some libraries had existing .NET 3.5 base

---

## Proposed Soluion #4: ["Delegate of Doom"](https://groups.google.com/d/msg/net-http-abstractions/rD18Hj1DwdQ/d_nrDyOLqcIJ)

Accepted!

    [lang=cs]
    public delegate void AppDelegate(
        IDictionary<string, object> env,
        ResultDelegate result,
        Action<Exception> fault);
    
    public delegate void ResultDelegate(
        string status,
        IDictionary<string, IEnumerable<string>> headers,
        BodyDelegate body);
    
    public delegate void BodyDelegate(
        Func<ArraySegment<byte>, bool> write,
        Func<Action, bool> flush,
        Action<Exception> end,
        CancellationToken cancellationToken);

***

## Problem: Implementations Cease

"Delegate of Doom" proved too difficult for most

---

## Solution: Gate Helper Library

---

## ???

![When your library dependency becomes a helper dependency](http://www.topito.com/wp-content/uploads/2013/01/code-34.gif)

*from [The Reality of a Developer's Life - in GIFs, Of Course](http://server.dzone.com/articles/reality-developers-life-gifs)*

***

# WIN!

## SignalR 

---

## Problem: ["Delegate of Doom" doesn't work for SignalR](https://groups.google.com/d/msg/net-http-abstractions/Cbvy2x27Few/Oor2UwA0W1wJ)

---

## Double-tap

* Nancy
* OWIN
* Rack
* Web API

---

## Single-tap

* ASP.NET MVC
* Fubu MVC
* node.js
* Razor
* SignalR

---

## Explained

> There are ways to run a single-tap web framework on a double-tap owin pipeline, but they all boil down to buffering write data.... Increased complexity, more memory to hold the body until the fwk returns, and cpu spent copying that data… Plus for fully synchronous web frameworks it means every response body will be entirely buffered because there’s no way for the server to deliver the output stream until the initial call returns. ... it’s pretty extreme compared to just passing the server’s output stream and a response header idictionary in the original call.

---

## Explained (cont)

> Running a double-tap framework on a single-tap pipeline by comparison is easy – the adapter just calls framework then calls its callback with the output stream.

---

## Solution

    [lang=cs]
    using AppFunc = Func<IDitionary<string, object>, Task>

---

## `Task`?

***

# Push to 1.0

---

## Microsoft adopts OWIN as Katana

---

## New Implementations

* [Fix](https://github.com/FixProject/Fix)
* [Dyfrig](https://github.com/panesofglass/dyfrig)
* [SimpleOwin](https://github.com/prabirshrestha/simple-owin)
* [Simple.Owin](https://github.com/EddieGarmon/Simple.Owin)

---

## owin.dll

Specified type aliases removed, but `IAppBuilder` remains:

    [lang=cs]
    public interface IAppBuilder
    {
        IDictionary<string, object> Properties { get; }
        IAppBuilder Use(object middleware, params object[] args);
        object Build(Type returnType);
        IAppBuilder New();
    }

***

## Adoption!

***

## Implementations

---

## [Katana (Microsoft.Owin)](http://www.nuget.org/packages/Microsoft.Owin/)

---

## [Fix](http://www.nuget.org/packages/Fix/0.8.1-pre)

---

## [Dyfrig](http://www.nuget.org/packages/Dyfrig/)

***

## Frameworks

---

## [SignalR](http://www.nuget.org/packages/Microsoft.AspNet.SignalR.Owin/)

---

## [NancyFx](http://nancyfx.org/)

---

## [ASP.NET Web API](http://www.nuget.org/packages/Microsoft.AspNet.WebApi.Owin/)

---

## FubuMVC

---

## Simple.Web

***

## Servers

---

## [System.Web](http://www.nuget.org/packages/Microsoft.Owin.Host.SystemWeb/)

---

## [HttpListener](http://www.nuget.org/packages/Microsoft.Owin.Host.HttpListener/)

---

## [IIS](http://www.nuget.org/packages/Microsoft.Owin.Host.IIS/1.0.0-alpha1)

---

## [Nowin](https://github.com/Bobris/Nowin)

---

## [Fracture](https://github.com/fractureio/fracture.http)

***

## Middleware

---

## [CORS](http://www.nuget.org/packages/Microsoft.Owin.Cors/)

---

## [Security](http://www.nuget.org/packages?q=owin+security)

---

## [Routing (Superscribe)](http://roysvork.wordpress.com/2013/09/22/why-you-shouldnt-use-a-web-framework-to-build-your-next-api/)

---

## [Diagnostics](http://www.nuget.org/packages/Microsoft.Owin.Diagnostics/)

---

## [Glimpse v2.0](https://github.com/Glimpse/Glimpse/pull/738)

***

## Put it All Together!

    [lang=cs]
    public class Startup {
        public void Configuration(IAppBuilder app) {
            app.Properties["host.AppName"] = "composed app";
            app.UseCors(Cors.CorsOptions.AllowAll)
               .UseWebApi(new HttpConfiguration())
               .MapHubs("signalr", new HubConfiguration())
               .UseNancy();
        }
    }

***

# Looking Ahead

***

## ASP.NET vNext

---

## BHAG Unlocked!
### MVC 6

---

## [Assembly Neutral Interfaces](http://davidfowl.com/assembly-neutral-interfaces-implementation/)
### Time to revisit interface?

---

## Faster Servers!

* Kestrel
* WebListener
* Helios (IIS without System.Web)

***

## OWIN Management Committee

***

## [Specification Updates](https://github.com/owin/owin/issues)

***

# Join Us!

http://owin.org

http://github.com/owin/owin

http://groups.google.com/group/net-http-abstractions

***

# A Brief History of OWIN

***

## A Sing Along

***

### With Apologies to Rupert Holmes

***

### I was tired of my WebForms
### and thought Sinatra was cool.

_(verse 1)_

***

### I wrote my own library
### then found that others had, too.

* [Frank](http://frankfs.net/) (mine)
* Boat (repo removed)
* [Figment](http://bugsquash.blogspot.com/2010/08/figment-web-dsl-for-f.html)
* [Kayak](http://web.archive.org/web/20101102133523/http://kayakhttp.com/)
* [NancyFx](http://nancyfx.com)
* [Nina](http://jondot.github.io/nina/) (no updates in 4 years)
* [OpenRasta](http://openrasta.org/)
* [Suave](http://suave.io/)
* [WCF Web API](https://wcf.codeplex.com/)

***

### So I wrote a quick email
### and asked to collaborate.

September 7, 2010

> I’ve noticed a trend recently in projects each of us has worked/is working on and wondered if we might be willing to pool our resources....
>
> ...
>
> If you are interested in working together, please let me know.... I’m sure we could agree on a common platform for our efforts.

- [Ryan Riley](https://twitter.com/panesofglass) to [Benjamin van der Veen](https://twitter.com/bvanderveen), [Mauricio Scheffer](https://twitter.com/mausch), and [Scott Koon](https://twitter.com/lazycoder)

***

### Within a month we had submissions
### for proposed interfaces

September 27, 2010

Benjamin van der Veen proposed the first set of interfaces:

    [lang=cs]
    public interface IHttpResponder {
        IObservable<IHttpServerResponse> Respond(
            IHttpServerRequest request,
            IDictionary<string, object> context);
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

***

### Then we ran into problems:
### people have opinions.

_(chorus)_

***

### We worked out the details
### and decided on a protocol.

(Just like the cool kids)

* [Rack](https://rack.github.io/) (Ruby)
* [WSGI](http://wsgi.readthedocs.org/en/latest/#) (Python)

***

### So we bought the domain name
### and started writing the specs.

***

### We set an audacious goal
### to run MVC on OWIN.

***

### We were blocked by factions:

* OO (object-oriented)
* FP (functional)
* Dynamic

_(verse 2)_

***

### We disagreed on dependencies
### as well as interface sig(nature)s.

***

### So we used only delegates,
### as well as FCL types.

***

### And for the dynamic languages?
### `IDictionary<string, object>`

***

### But how to represent async?
### We had a longer debate.

_(chorus)_

***

### `Stream`s were considered "too heavy";
### `IObservable<T>` was too new.

***

### `Task` limited framework versions
### to only .NET 4.0

***

### So in the end we decided
### on the ["Delegate of Doom"](https://groups.google.com/d/msg/net-http-abstractions/rD18Hj1DwdQ/d_nrDyOLqcIJ)

    [lang=cs]
    public delegate void AppDelegate(
        IDictionary<string, object> env,
        ResultDelegate result,
        Action<Exception> fault);
    
    public delegate void ResultDelegate(
        string status,
        IDictionary<string, IEnumerable<string>> headers,
        BodyDelegate body);
    
    public delegate void BodyDelegate(
        Func<ArraySegment<byte>, bool> write,
        Func<Action, bool> flush,
        Action<Exception> end,
        CancellationToken cancellationToken);

***

### We finally had a solution,
### but it created problems.

_(verse 3)_

***

### Whereas before we had prototypes,
### now we had only a spec.

***

### A helper library was proposed,
### and soon Gate was built.

***

### But what nobody noticed
### was now we had a dependency!

![When your library dependency becomes a helper dependency](http://www.topito.com/wp-content/uploads/2013/01/code-34.gif)

*from [The Reality of a Developer's Life - in GIFs, Of Course](http://server.dzone.com/articles/reality-developers-life-gifs)*

***

### And then along came SignalR.
### We wanted to claim victory.

_(chorus)_

***

### We only had one problem:
### ["Delegate of Doom" wouldn't work](https://groups.google.com/d/msg/net-http-abstractions/Cbvy2x27Few/Oor2UwA0W1wJ)

***

### Back to the drawing board we went:
### Single- or double-tap?

---

## Double-tap

* Nancy
* OWIN
* Rack
* Web API

---

## Single-tap

* ASP.NET MVC
* Fubu MVC
* node.js
* Razor
* SignalR

---

## Explained

> There are ways to run a single-tap web framework on a double-tap owin pipeline, but they all boil down to buffering write data.... Increased complexity, more memory to hold the body until the fwk returns, and cpu spent copying that data… Plus for fully synchronous web frameworks it means every response body will be entirely buffered because there’s no way for the server to deliver the output stream until the initial call returns. ... it’s pretty extreme compared to just passing the server’s output stream and a response header idictionary in the original call.

---

## Explained (cont)

> Running a double-tap framework on a single-tap pipeline by comparison is easy – the adapter just calls framework then calls its callback with the output stream.

***

### And came up with a solution,
### just this simple delegate:

    [lang=cs]
    using AppFunc = Func<IDitionary<string, object>, Task>

---

## `Task`?

***

### Now we had real users
### and started gaining adoption

_(verse 4)_

***

### We knew its name in an instant;
### we knew the F5 experience.

***

### It was our old friend ASP.NET,
### and we said, "Ah! It's you."

***

### Then we laughed for a moment,
### and we said, "We never knew ..."

***

### that you would go build vNext
### and [Assembly Neutral Interfaces](http://davidfowl.com/assembly-neutral-interfaces-implementation/)

_(chorus)_

***

### and deliver MVC 6
### on top of an OWIN framework.

***

### If you like writing middleware pipelines
### with `.Use` and `.Map` extensions,

***

### Then you're the framework we've looked for;
### let's go build new web apps!

***

## One more time?

***

## OWIN Management Committee

***

## [Specification Updates](https://github.com/owin/owin/issues)

***

# Join Us!

http://owin.org

http://github.com/owin/owin

http://groups.google.com/group/net-http-abstractions
