﻿using ApiWeb.Database;
using ApiWeb.Models;
using ApiWeb.Repositories.TheRepository;
using System.Collections;

namespace ApiWeb.Repositories.TokenRepository
{
    public class TokenService : Repository<TheToken>, ITokenService
    {
        public TokenService(TheContext context) : base(context)
        {
        }
        public async Task<bool> SaveToken(Guid userId, string Token, string Type)
        {
            var TheToken = new TheToken
            {
                UserId = userId,
                Token = Token,
                Type = Type

            };
            await AddAsync(TheToken);
            await SaveAsync();
            return true;
        }
        public async Task<bool> IsTokenValid(string token) => (await GetAllAsync()).Where(t => t.Token == token).FirstOrDefault()!=null;
        public async Task<TheToken> OneRefreshToken(Guid id) => (await GetOneGuidIdAsync(id));
        public async Task<TheToken> OneRefreshTokenOnUserId(Guid userId) => (await GetAllAsync()).Where(id => id.UserId == userId).FirstOrDefault();
        public async Task<List<TheToken>> AllRefreshTokens(Guid userId) => (await GetAllAsync()).Where(id => id.UserId == userId).ToList();

    }
}
