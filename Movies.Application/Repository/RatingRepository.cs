﻿using Dapper;
using Movies.Application.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Repository
{
    public class RatingRepository : IRatingRepository
    {
        private readonly IDBConnectionFactory _dbConnectionFactory;

        public RatingRepository(IDBConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<float?> GetRatingAsync(Guid movieId, CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
            return await connection.QuerySingleOrDefaultAsync<float?>(new CommandDefinition("""
                select round(avg(r.rating), 1) from ratings r
                where movieid = @movieId
                """, new { movieId }, cancellationToken: token));
        }

        public async Task<(float? Rating, int? UserRating)> GetRatingAsync(Guid movieId, Guid userId, CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
            return await connection.QuerySingleOrDefaultAsync<(float?, int?)>(new CommandDefinition("""
                select roung(avg(rating), 1),
                        (select rating 
                         from ratings
                         where movieid = @movieId
                           and userid = @userId
                        limit 1)
                from ratings
                where movieid = @movieId
                """, new { movieId, userId}, cancellationToken: token));
        }
    }
}
