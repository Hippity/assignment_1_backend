using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MovieBackend.Configurations;
using MovieBackend.Models;
using MovieBackend.Models.Api;
using MovieBackend.Interaces;
using RestSharp;

namespace MovieBackend.Services
{
    public class TmdbService : ITmdbService
    {
        private readonly RestClient _client;
        private readonly string _api_key;
        private readonly IMemoryCacheService _cache;

        public TmdbService(IOptions<TmdbSettings> settings, IMemoryCacheService cache)
        {
            _client = new RestClient(settings.Value.BASE_URL);
            _api_key = settings.Value.API_KEY;
            _cache = cache;
        }

        public async Task<MovieDetails> GetMovieDetailsAsync(int movieId)
        {
            string cacheKey = $"MovieDetails_{movieId}";

            if (_cache.TryGetValue(cacheKey, out MovieDetails movieDetails))
            {
                return movieDetails;
            }

            var request = new RestRequest($"movie/{movieId}", Method.Get);
            request.AddQueryParameter("language", "en-US");

            request.AddHeader("Authorization", $"Bearer {_api_key}");
            request.AddHeader("Accept", "application/json");

            var response = await _client.ExecuteAsync<MovieDetails>(request);

            if (!response.IsSuccessful || response.Data == null)
            {
                return null;
                throw new Exception("Failed to fetch movie details from TMDB.");
            }

            movieDetails = response.Data;

            // Cache the result with a sliding expiration of 5 minutes
            _cache.Set(cacheKey, movieDetails, TimeSpan.FromMinutes(5));

            return movieDetails;

        }

        public async Task<MovieListResponse> GetPopularMoviesAsync(int page = 1)
        {

            string cacheKey = $"PopularMovies_{page}";

            if (_cache.TryGetValue(cacheKey, out MovieListResponse movieListResponse))
            {
                return movieListResponse;
            }

            var request = new RestRequest("movie/popular", Method.Get);
            request.AddQueryParameter("language", "en-US");
            request.AddQueryParameter("page", page.ToString());

            request.AddHeader("Authorization", $"Bearer {_api_key}");
            request.AddHeader("Accept", "application/json");

            var response = await _client.ExecuteAsync<MovieListResponse>(request);

            if (!response.IsSuccessful || response.Data == null)
            {
                return null;
                throw new Exception("Failed to fetch popular movies from TMDB.");
            }

            movieListResponse = response.Data;

            _cache.Set(cacheKey, movieListResponse, TimeSpan.FromMinutes(5));

            return movieListResponse;
        }

        public async Task<MovieListResponse> SearchMoviesAsync(string query, int page = 1)
        {
            string cacheKey = $"SearchMovies_{query}_{page}";
            if (_cache.TryGetValue(cacheKey, out MovieListResponse movieListResponse))
            {
                return movieListResponse;
            }

            var request = new RestRequest("search/movie", Method.Get);
            request.AddQueryParameter("language", "en-US");
            request.AddQueryParameter("query", query);
            request.AddQueryParameter("page", page.ToString());
            request.AddHeader("Authorization", $"Bearer {_api_key}");
            request.AddHeader("Accept", "application/json");

            var response = await _client.ExecuteAsync<MovieListResponse>(request);
            if (!response.IsSuccessful || response.Data == null)
            {
                return null;
                throw new Exception("Failed to search movies from TMDB.");
            }

            movieListResponse = response.Data;
            _cache.Set(cacheKey, movieListResponse, TimeSpan.FromMinutes(5));
            return movieListResponse;
        }

        public async Task<GenreListResponse> GetGenresAsync()
        {
            string cacheKey = "Genres";
            if (_cache.TryGetValue(cacheKey, out GenreListResponse genreListResponse))
            {
                return genreListResponse;
            }

            var request = new RestRequest("genre/movie/list", Method.Get);
            request.AddQueryParameter("language", "en-US");
            request.AddHeader("Authorization", $"Bearer {_api_key}");
            request.AddHeader("Accept", "application/json");

            var response = await _client.ExecuteAsync<GenreListResponse>(request);
            if (!response.IsSuccessful || response.Data == null)
            {
                return null;
                throw new Exception("Failed to fetch genres from TMDB.");
            }

            genreListResponse = response.Data;
            // Cache genres for longer as they don't change often
            _cache.Set(cacheKey, genreListResponse, TimeSpan.FromHours(24));
            return genreListResponse;
        }

        public async Task<MovieListResponse> GetMoviesByGenreAsync(int genreId, int page = 1)
        {
            string cacheKey = $"MoviesByGenre_{genreId}_{page}";
            if (_cache.TryGetValue(cacheKey, out MovieListResponse movieListResponse))
            {
                return movieListResponse;
            }

            var request = new RestRequest("discover/movie", Method.Get);
            request.AddQueryParameter("language", "en-US");
            request.AddQueryParameter("with_genres", genreId.ToString());
            request.AddQueryParameter("page", page.ToString());
            request.AddHeader("Authorization", $"Bearer {_api_key}");
            request.AddHeader("Accept", "application/json");

            var response = await _client.ExecuteAsync<MovieListResponse>(request);
            if (!response.IsSuccessful || response.Data == null)
            {
                return null;
                throw new Exception("Failed to fetch movies by genre from TMDB.");
            }

            movieListResponse = response.Data;
            _cache.Set(cacheKey, movieListResponse, TimeSpan.FromMinutes(5));
            return movieListResponse;
        }

        public async Task<MovieCreditResponse> GetMovieCredits(int movieId)
        {
            string cacheKey = $"MovieCredits_{movieId}";
            if (_cache.TryGetValue(cacheKey, out MovieCreditResponse movieCreditResponse))
            {
                return movieCreditResponse;
            }

            var request = new RestRequest($"movie/{movieId}/credits", Method.Get);
            request.AddQueryParameter("language", "en-US");
            request.AddHeader("Authorization", $"Bearer {_api_key}");
            request.AddHeader("Accept", "application/json");

            var response = await _client.ExecuteAsync<MovieCreditResponse>(request);
            if (!response.IsSuccessful || response.Data == null)
            {
                return null;
                throw new Exception("Failed to fetch movies by genre from TMDB.");
            }

            movieCreditResponse = response.Data;
            _cache.Set(cacheKey, movieCreditResponse, TimeSpan.FromMinutes(5));
            return movieCreditResponse;
        }

    }
}
