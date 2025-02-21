﻿// Copyright (c) .NET Core Community. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

// ReSharper disable once CheckNamespace
namespace DotNetCore.CAP
{
    public class PostgreSqlOptions : EFOptions
    {
        /// <summary>
        /// Gets or sets the database's connection string that will be used to store database entities.
        /// </summary>
        public string ConnectionString { get; set; } = default!;
    }

    internal class ConfigurePostgreSqlOptions : IConfigureOptions<PostgreSqlOptions>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ConfigurePostgreSqlOptions(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public void Configure(PostgreSqlOptions options)
        {
            if (options.DbContextType == null) return;
            
            using var scope = _serviceScopeFactory.CreateScope();
            var provider = scope.ServiceProvider;
            using var dbContext = (DbContext) provider.GetRequiredService(options.DbContextType);
            options.ConnectionString = dbContext.Database.GetDbConnection().ConnectionString;
        }
    }
}