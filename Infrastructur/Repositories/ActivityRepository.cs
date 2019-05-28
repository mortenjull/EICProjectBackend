using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Domain.DBEntities;
using Domain.DBEntities.Entities;
using Domain.ModelEntities;

namespace Infrastructur.Repositories
{
    public class ActivityRepository : IActivityRepository<Activity>
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;

        public ActivityRepository(IDbConnection connection, IDbTransaction transaction)
        {
            this._connection = connection;
            this._transaction = transaction;
        }
        public Activity Create(Activity activity)
        {
            try
            {
                string createActivityQuery = "INSERT INTO EIC.dbo.Activity (Created, ActivityName) " +
                                             "VALUES (@Created, @ActivityName); " +
                                             "SELECT SCOPE_IDENTITY();";
                int result = this._connection.ExecuteScalar<int>(createActivityQuery, activity, this._transaction);
                if (result <= 0)
                    return null;

                activity.Id = result;
                return activity;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<Activity> Read(int id)
        {
            try
            {
                string readActivityQuery = "SELECT * FROM EIC.dbo.Activity " +
                                           "WHERE Id = " + id + ";";

                var result = await this._connection.QueryAsync<Activity>(readActivityQuery, null, this._transaction);
                if (result == null)
                    return null;

                return result.First();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<Activity>> ReadAll()
        {
            try
            {
                string readAllActivityQuery = "SELECT * FROM EIC.dbo.Activity; ";


                var result = await this._connection.QueryAsync<Activity>(readAllActivityQuery, null, this._transaction);
                if (result == null || !result.Any())
                    return null;

                return result.ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public Activity Update(Activity entity)
        {
            try
            {
                string updateActivityQuery = "UPDATE EIC.dbo.Activity " +
                                             "SET ActivityName = @ActivityName " +
                                             "WHERE Id = @Id";

                var result = this._connection.Execute(updateActivityQuery, entity, this._transaction);
                if (result <= 0)
                    return null;

                return entity;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                string deleteActivityQuery = "DELETE FROM EIC.dbo.Activity " +
                                             "WHERE Id = " + id + ";";

                var result = this._connection.Execute(deleteActivityQuery, null, this._transaction);
                if (result <= 0)
                    return false;

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool CreateActivityResearcher(ActivityResearcher model)
        {
            try
            {
                string createActivityResearcher = "INSERT INTO EIC.dbo.ActivityResearcher (Created, ResearcherId,                                               UserId, ActivityId) " +
                                                        "VALUES (@Created, @ResearcherId, @UserId, @ActivityId); " +
                                                        "SELECT SCOPE_IDENTITY();";

                var result = this._connection.Execute(createActivityResearcher, model, this._transaction);
                if (result <= 0)
                    return false;

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool DeleteActivityResearcher(int id)
        {
            try
            {
                string deleteActivityResearcherQuery = "DELETE FROM EIC.dbo.ActivityResearcher " +
                                             "WHERE Id = " + id + ";";

                var result = this._connection.Execute(deleteActivityResearcherQuery, null, this._transaction);
                if (result <= 0)
                    return false;

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<List<ActivityWithUserModel>> ReadAllActivityForResearcher(int id)
        {
            try
            {
                string readAllActivityForResearcherQuery = "SELECT [Activity].ActivityName, ActivityResearcher.Id, " +
                                                           "ActivityResearcher.Created, [User].Id, [User].Username " +
                                                           "FROM [EIC].[dbo].[Activity] " +
                                                           "left JOIN ActivityResearcher on Activity.Id = ActivityResearcher.ActivityId " +
                                                           "left JOIN Researcher on ActivityResearcher.ResearcherId = Activity.Id " +
                                                           "left JOIN [User] on ActivityResearcher.UserId = [User].Id " +
                                                           "WHERE ActivityResearcher.ResearcherId = " + id + ";";

                var results = new List<ActivityWithUserModel>();
                var result = await this._connection.QueryAsync<Activity, ActivityResearcher, User, Activity>(readAllActivityForResearcherQuery, (activity, activityResearcher, user) =>
                {
                    var model = results.FirstOrDefault(c => c.Id == activityResearcher.Id);
                    if (model == null)
                    {
                        var activityWithUserModel = new ActivityWithUserModel();
                        activityWithUserModel.Id = activityResearcher.Id;
                        activityWithUserModel.Created = activityResearcher.Created;
                        results.Add(activityWithUserModel);
                        model = activityWithUserModel;
                    }

                    if (model.ActivityName == null && activity != null)
                    {
                        model.ActivityName = activity.ActivityName;
                    }
                    if (model.Username == null && user != null)
                    {
                        model.Username = user.UserName;
                    }

                    return null;
                }, transaction: this._transaction);
                if (result == null)
                    return null;

                return results;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public bool DeleteActivityReseacherViaResearcherId(int id)
        {
            try
            {
                string deleteActivityQuery = "DELETE FROM EIC.dbo.Activity " +
                                             "WHERE ResearcherId = " + id + ";";

                var result = this._connection.Execute(deleteActivityQuery, null, this._transaction);
                if (result <= 0)
                    return false;

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
