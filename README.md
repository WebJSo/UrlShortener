# UrlShortener


Overview
The URL Shortener is a website which takes a URL input and then returns a short version of the URL, which can be used to redirect to the original URL.
The URLs can be added, updated and removed.
The solution is .NET Core 3.1 MVC, class libraries and xUnit. There are 5 projects in the solution:

MVC
MVC project takes the requests, utilises IShortUrlService and returns the views/website.

Repos
Repos project performs CRUD operations using LightDb package. 
LightDb chosen because it allows the project to function without requiring set up or configurion. 
LightDb does not allow async or mocking.
Repos can be replaced with alternative implementations e.g. Entity Framework.

Services
Services project provides access to CRUD functionality using IShortUrlRepo and utilises ShortUrlExtensions methods to perform URL operations.

Shared
Shared project provides access to shared extensions, interfaces and models.
ShortUrlExtensions methods allows creation of short URL, formatting of URL and validation of URL.
ShortUrlExtensions creates the short url using a unique Id from the database, which is then base 64 encoded and trimmed.

UnitTests
UnitTests project uses xUnit. ShortUrlExtensions and ShortUrlService are tested using ShortUrlTestRepo to ensure logic, URL formatting and validation standards are maintained.

Note:
When starting the project ShortUrlLightDb.db LightDb should be created in the root of the MVC project.
