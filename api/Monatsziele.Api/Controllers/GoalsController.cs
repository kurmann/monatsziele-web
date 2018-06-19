﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Monatsziele.Api.Models;

namespace Monatsziele.Api.Controllers
{
    [Route("[controller]")]
    public class GoalsController : Controller
    {
        private IConfiguration Configuration { get; }
        private IMapper Mapper { get; }

        public GoalsController(IConfiguration configuration, IMapper mapper)
        {
            Configuration = configuration;
            Mapper = mapper;
        }

        protected const string TableStorageConnectionStringName = "monatsziele_AzureStorageConnectionString";

        [HttpGet]
        public Goal[] Get()
        {
            var goalEntities = GetGoalsEntities().Result;
            var goals = Mapper.Map<Goal[]>(goalEntities);
            return goals;
        }

        [HttpGet("{id}")]
        public Goal[] Get(Guid id)
        {
            var goalEntity = GetGoalEntity(id);
            return null;
        }

        private async Task<TableResult> GetGoalEntity(Guid id)
        {
            var cloudTable = await GetTable("Goals");
            var tableOperation = TableOperation.Retrieve<GoalEntity>("kurmannwillisau@me.com", id.ToString());

            return cloudTable.ExecuteAsync(tableOperation)?.Result;
        }

        private async Task<List<GoalEntity>> GetGoalsEntities()
        {
            var cloudTable = await GetTable("Goals");
            var continuationToken = default(TableContinuationToken);
            var goals = new List<GoalEntity>();

            var generateFilterCondition = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "kurmannwillisau@me.com");
            var query = new TableQuery<GoalEntity>().Where(generateFilterCondition);

            do
            {
                var tableQuerySegment = await cloudTable.ExecuteQuerySegmentedAsync(query, continuationToken);
                continuationToken = tableQuerySegment.ContinuationToken;
                goals.AddRange(tableQuerySegment.Results);
            } while (continuationToken != null);

            return goals;
        }

        private async Task<CloudTable> GetTable(string tableName)
        {
            // get config
            var connectionString = Configuration.GetConnectionString("monatsziele_AzureStorageConnectionString");
            var storageAccount = CloudStorageAccount.Parse(connectionString);

            // table storage client
            var tableClient = storageAccount.CreateCloudTableClient();

            // get table
            var tableReference = tableClient.GetTableReference(tableName);

            // create if not exists
            await tableReference.CreateIfNotExistsAsync();

            return tableReference;
        }
    }
}