using Microsoft.Extensions.DependencyInjection;
using Movies.Api.Sdk;
using Movies.Contracts.Requests;
using Refit;
using System.Text.Json;

//var moviesApi = RestService.For<IMoviesApi>("https://localhost:5001");
var services = new ServiceCollection();

services
    .AddHttpClient()
    .AddSingleton<AuthTokenProvider>()
    .AddRefitClient<IMoviesApi>(s => new RefitSettings
    {
        AuthorizationHeaderValueGetter = async (request, cancellationToken) => await s.GetRequiredService<AuthTokenProvider>().GetTokenAsync()
    })
    .ConfigureHttpClient(x =>
        x.BaseAddress = new Uri("https://localhost:5001"));

var provider = services.BuildServiceProvider();
var moviesApi = provider.GetRequiredService<IMoviesApi>();

// Create new movie
var newMovie = moviesApi.CreateMovieAsync(new CreateMovieRequest 
{
    Title = "Bring Her Back",
    YearOfRelease = 2025,
    Genres = new[] { "Comedy" }
});
Console.WriteLine(JsonSerializer.Serialize(newMovie.Result));

// Get movie
var movie = await moviesApi.GetMovieAsync("bring-her-back-2025");
Console.WriteLine(JsonSerializer.Serialize(movie));

// Update movie
var updatedMovie = await moviesApi.UpdateMovieAsync(movie.Id, new UpdateMovieRequest
{
    Title = "Bring Her Back",
    YearOfRelease = 2025,
    Genres = new[] { "Horror" }
});
Console.WriteLine(JsonSerializer.Serialize(updatedMovie));

// Get all movies
var request = new GetAllMoviesRequest
{
    Title = null,
    Year = 2025,
    SortBy = null,
    Page = 1,
    PageSize = 3
};
var allMovies = await moviesApi.GetMoviesAsync(request);
Console.WriteLine(JsonSerializer.Serialize(allMovies));

// Delete movie
await moviesApi.DeleteMovieAsync(updatedMovie.Id);