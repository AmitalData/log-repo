#if false

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Server.Infrastructure.Helpers
{
    public static class DbContextBaseTestClass
    {
        private static DbCommand GetUpdateBatchCommand<TEntity>(this Table<TEntity> table, IQueryable<TEntity> entities, Expression<Func<TEntity, TEntity>> evaluator) where TEntity : class
        {
            var updateCommand = table.Context.GetCommand(entities);

            var setSB = new StringBuilder();
            int memberInitCount = 1;

#if false
           
            // Process the MemberInitExpression (there should only be one in the evaluator Lambda) to convert the expression tree
            // into a valid DbCommand.  The Visit<> method will only process expressions of type MemberInitExpression and requires
            // that a MemberInitExpression be returned - in our case we'll return the same one we are passed since we are building
            // a DbCommand and not 'really using' the evaluator Lambda.
            evaluator.Visit<MemberInitExpression>(delegate(MemberInitExpression expression)
            {
                if (memberInitCount > 1)
                {
                    throw new NotImplementedException("Currently only one MemberInitExpression is allowed for the evaluator parameter.");
                }
                memberInitCount++;

                setSB.Append(GetDbSetStatement<TEntity>(expression, table, updateCommand));

                return expression; // just return passed in expression to keep 'visitor' happy.
            });

            
#endif
            // Complete the command text by concatenating bits together.
            updateCommand.CommandText = string.Format("UPDATE {0}\r\n{1}\r\n\r\n{2}",
                                                            table.GetDbName(),									// Database table name
                                                            setSB.ToString(),									// SET fld = {}, fld2 = {}, ...
                                                            GetBatchJoinQuery<TEntity>(table, entities));	// Subquery join created from entities command text

            if (updateCommand.CommandText.IndexOf("[arg0]") >= 0 || updateCommand.CommandText.IndexOf("NULL AS [EMPTY]") >= 0)
            {
                // TODO (Chris): Probably a better way to determine this by using an visitor on the expression before the
                //				 var selectExpression = Expression.Call... method call (search for that) and see which funcitons
                //				 are being used and determine if supported by LINQ to SQL
                throw new NotSupportedException(string.Format("The evaluator Expression<Func<{0},{0}>> has processing that needs to be performed once the query is returned (i.e. string.Format()) and therefore can not be used during batch updating.", table.GetType()));
            }

            return updateCommand;
        }

        private static string GetDbSetStatement<TEntity>(MemberInitExpression memberInitExpression, Table<TEntity> table, DbCommand updateCommand) where TEntity : class
        {
            var entityType = typeof(TEntity);

            if (memberInitExpression.Type != entityType)
            {
                throw new NotImplementedException(string.Format("The MemberInitExpression is intializing a class of the incorrect type '{0}' and it should be '{1}'.", memberInitExpression.Type, entityType));
            }

            var setSB = new StringBuilder();

            var tableName = table.GetDbName();
            var metaTable = table.Context.Mapping.GetTable(entityType);
            // Used to look up actual field names when MemberAssignment is a constant,
            // need both the Name (matches the property name on LINQ object) and the
            // MappedName (db field name).
            var dbCols = from mdm in metaTable.RowType.DataMembers
                         select new { mdm.MappedName, mdm.Name };

            // Walk all the expression bindings and generate SQL 'commands' from them.  Each binding represents a property assignment
            // on the TEntity object initializer Lambda expression.
            foreach (var binding in memberInitExpression.Bindings)
            {
                var assignment = binding as MemberAssignment;

                if (binding == null)
                {
                    throw new NotImplementedException("All bindings inside the MemberInitExpression are expected to be of type MemberAssignment.");
                }

                // TODO (Document): What is this doing?  I know it's grabbing existing parameter to pass into Expression.Call() but explain 'why'
                //					I assume it has something to do with fact we can't just access the parameters of assignment.Expression?
                //					Also, any concerns of whether or not if there are two params of type entity type?
                ParameterExpression entityParam = null;
#if false
                assignment.Expression.Visit<ParameterExpression>(delegate(ParameterExpression p) { if (p.Type == entityType) entityParam = p; return p; });

#endif
                // Get the real database field name.  binding.Member.Name is the 'property' name of the LINQ object
                // so I match that to the Name property of the table mapping DataMembers.
                string name = binding.Member.Name;
                var dbCol = (from c in dbCols
                             where c.Name == name
                             select c).FirstOrDefault();

                if (dbCol == null)
                {
                    throw new ArgumentOutOfRangeException(name, string.Format("The corresponding field on the {0} table could not be found.", tableName));
                }

                var mappedName = dbCol.MappedName.StartsWith("[") ? dbCol.MappedName : string.Format("[{0}]", dbCol.MappedName);

                // If entityParam is NULL, then no references to other columns on the TEntity row and need to eval 'constant' value...
                if (entityParam == null)
                {
                    // Compile and invoke the assignment expression to obtain the contant value to add as a parameter.
                    var constant = Expression.Lambda(assignment.Expression, null).Compile().DynamicInvoke();

                    // use the MappedName from the table mapping DataMembers - that is field name in DB table.
                    if (constant == null)
                    {
                        setSB.AppendFormat("{0} = null, ", mappedName);
                    }
                    else
                    {
                        // Add new parameter with massaged name to avoid clashes.
                        setSB.AppendFormat("{0} = @p{1}, ", mappedName, updateCommand.Parameters.Count);
                        updateCommand.Parameters.Add(new SqlParameter(string.Format("@p{0}", updateCommand.Parameters.Count), constant));
                    }
                }
                else
                {
                    // TODO (Documentation): Explain what we are doing here again, I remember you telling me why we have to call but I can't remember now.
                    // Wny are we calling Expression.Call and what are we passing it?  Below comments are just 'made up' and probably wrong.

                    // Create a MethodCallExpression which represents a 'simple' select of *only* the assignment part (right hand operator) of
                    // of the MemberInitExpression.MemberAssignment so that we can let the Linq Provider do all the 'sql syntax' generation for
                    // us. 
                    //
                    // For Example: TEntity.Property1 = TEntity.Property1 + " Hello"
                    // This selectExpression will be only dealing with TEntity.Property1 + " Hello"
                    var selectExpression = Expression.Call(
                                                typeof(Queryable),
                                                "Select",
                                                new Type[] { entityType, assignment.Expression.Type },

                    // TODO (Documentation): How do we know there are only 'two' parameters?  And what is Expression.Lambda
                        //						 doing?  I assume it's returning a type of assignment.Expression.Type to match above?

                                                Expression.Constant(table),
                                                Expression.Lambda(assignment.Expression, entityParam));

                    setSB.AppendFormat("{0} = {1}, ",
                                            mappedName,
                                            GetDbSetAssignment(table, selectExpression, updateCommand, name));
                }
            }

            var setStatements = setSB.ToString();
            return "SET " + setStatements.Substring(0, setStatements.Length - 2); // remove ', '
        }

        private static void ValidateExpression(ITable table, Expression expression)
        {
            // Simply compile the expression to see if it will work.
            // Needed to change this to use expression tree.  Original code (below) could fail with DataContextInterceptor.

            var compile = Expression.Call(
                Expression.Property(Expression.Constant(table.Context), "Provider"),
                "Compile",
                null,
                Expression.Constant(expression));

            Expression.Lambda<Action>(compile).Compile()();
            /*
                        var context = table.Context;
                        PropertyInfo providerProperty = context.GetType().GetProperty( "Provider", BindingFlags.Instance | BindingFlags.NonPublic );
                        var provider = providerProperty.GetValue( context, null );
                        var compileMI = provider.GetType().GetMethod( "System.Data.Linq.Provider.IProvider.Compile", BindingFlags.Instance | BindingFlags.NonPublic );

                        compileMI.Invoke( provider, new object[] { expression } );*/
        }
        private static string GetDbSetAssignment(ITable table, MethodCallExpression selectExpression, DbCommand updateCommand, string bindingName)
        {
            ValidateExpression(table, selectExpression);

            // Convert the selectExpression into an IQueryable query so that I can get the CommandText
            var selectQuery = (table as IQueryable).Provider.CreateQuery(selectExpression);

            // Get the DbCommand so I can grab relavent parts of CommandText to construct a field 
            // assignment and based on the 'current TEntity row'.  Additionally need to massage parameter 
            // names from temporary command when adding to the final update command.
            var selectCmd = table.Context.GetCommand(selectQuery);
            var selectStmt = selectCmd.CommandText;
            selectStmt = selectStmt.Substring(7,									// Remove 'SELECT ' from front ( 7 )
                                        selectStmt.IndexOf("\r\nFROM ") - 7)		// Return only the selection field expression
                                    .Replace("[t0].", "")							// Remove table alias from the select
                                    .Replace(" AS [value]", "")					// If the select is not a direct field (constant or expression), remove the field alias
                                    .Replace("@p", "@p" + bindingName);			// Replace parameter name so doesn't conflict with existing ones.

            foreach (var selectParam in selectCmd.Parameters.Cast<DbParameter>())
            {
                var paramName = string.Format("@p{0}", updateCommand.Parameters.Count);

                // DataContext.ExecuteCommand ultimately just takes a object array of parameters and names them p0-N.  
                // So I need to now do replaces on the massaged value to get it in proper format.
                selectStmt = selectStmt.Replace(
                                    selectParam.ParameterName.Replace("@p", "@p" + bindingName),
                                    paramName);

                updateCommand.Parameters.Add(new SqlParameter(paramName, selectParam.Value));
            }

            return selectStmt;
        }
        private static string GetBatchJoinQuery<TEntity>(Table<TEntity> table, IQueryable<TEntity> entities) where TEntity : class
        {
            var metaTable = table.Context.Mapping.GetTable(typeof(TEntity));

            var keys = from mdm in metaTable.RowType.DataMembers
                       where mdm.IsPrimaryKey
                       select new { mdm.MappedName };

            var joinSB = new StringBuilder();
            var subSelectSB = new StringBuilder();

            foreach (var key in keys)
            {
                joinSB.AppendFormat("j0.[{0}] = j1.[{0}] AND ", key.MappedName);
                // For now, always assuming table is aliased as t0.  Should probably improve at some point.
                // Just writing a smaller sub-select so it doesn't get all the columns of data, but instead
                // only the primary key fields used for joining.
                subSelectSB.AppendFormat("[t0].[{0}], ", key.MappedName);
            }

            var selectCommand = table.Context.GetCommand(entities);
            var select = selectCommand.CommandText;

            var join = joinSB.ToString();

            if (join == "")
            {
                throw new MissingPrimaryKeyException(string.Format("{0} does not have a primary key defined.  Batch updating/deleting can not be used for tables without a primary key.", metaTable.TableName));
            }

#region - Email Regarding code below
            /*
                So…to be honest an old employee helped me write some of this library.  Specifically the code around Expression tree manipulation and visiting.  I’ve reached out to him but testing…

                a) When you have the CompiledQuery.Compile() version of code, when I attempt to get the underlying ‘sql’ query that the variable posts would be created by, I am returned “SELECT NULL AS [EMPTY]” from the following code:

			                var selectCommand = table.Context.GetCommand( entities );
			                var select = selectCommand.CommandText;

                   I’m not sure why LINQ to SQL is returning that ‘empty’ query to represent posts variable.  And if there is something else I can evaluate/visit to find the real query.  Similar problem found here: http://stackoverflow.com/questions/1719264/how-to-extract-the-sql-command-from-a-complied-linq-query I’m still googling.

                b) Workaround #1: How often are you calling this method?  Would assume not very often?  If you remove the CompiledQuery.Compile() from you code it works.  Obviously it’ll have to compile the query each time, but if only running a handful of times, probably not a problem.

                c) Workaround #2: If you change your static Func<> into a ‘real’ static function() { } it works as well.  I’m not versed enough in ‘compiled code’ versus CompiledQuery.Compile() to know the actual differences in how the compiler might optimized the static function() and any performance hits you might hit, but this might be acceptable solution as well.

                I’ll let you know if I find anything.

                On Aug 18, 2015, at 6:30 AM, Tomas Pettersson <Tomas.Pettersson@firefly.se> wrote:

                Hi
                Here is the code that tries to do an update batch:
                           using (DataClasses1DataContext dbContext = new DataClasses1DataContext())
                           {
                               var posts = getXMgdParamRows(dbContext, 100);
                               dbContext.T_AllMgdParams.UpdateBatch(posts, p => new T_AllMgdParam { Copy = 2 });
                           }


                And getXMgdParamRows looks like this:
                       public static Func<DataClasses1DataContext, int, IQueryable<T_AllMgdParam>>
                            getXMgdParamRows = CompiledQuery.Compile((DataClasses1DataContext dcFrom, int getRows) =>
                            (from a in dcFrom.T_AllMgdParams
                             orderby a.DateAndTime
                             where a.Copy != 2
                             select a).Take(getRows));


                Yes, it's the same table.
                /Tomas
            */
            #endregion

            // Had to comment out again b/c if where has enumerable.Select( => ).Contains( ... ) you get a NULL AS [EMPTY] and the BatchJoin was returning entire query
            // which corrupted the join sql
            /*
            else if ( select.IndexOf( "NULL AS [EMPTY]" ) >= 0 )
            {
                return select;  // calling function will throw exception
            }
            */

            join = join.Substring(0, join.Length - 5);											// Remove last ' AND '
#region - Better ExpressionTree Handling Needed -
            /*
			
			Below is a sample query where the let statement was used to simply the 'where clause'.  However, it produced an extra level
			in the query.
			 
			var manage =
				from u in User
				join g in Groups on u.User_Group_id equals g.gKey into groups
				from g in groups.DefaultIfEmpty()
				let correctGroup = groupsToManage.Contains( g.gName ) || ( groupsToManage.Contains( "_GLOBAL" ) && g.gKey == null )
				where correctGroup && ( users.Contains( u.User_Authenticate_id ) || userEmails.Contains( u.User_Email ) ) || userKeys.Contains( u.User_id )
				select u;
			 
			Produces this SQL:
			SELECT [t2].[User_id] AS [uKey], [t2].[User_Authenticate_id] AS [uAuthID], [t2].[User_Email] AS [uEmail], [t2].[User_Pin] AS [uPin], [t2].[User_Active] AS [uActive], [t2].[uAdminAuthID], [t2].[uFailureCount]
			FROM (
				SELECT [t0].[User_id], [t0].[User_Authenticate_id], [t0].[User_Email], [t0].[User_Pin], [t0].[User_Active], [t0].[uFailureCount], [t0].[uAdminAuthID], 
					(CASE 
						WHEN [t1].[gName] IN (@p0) THEN 1
						WHEN NOT ([t1].[gName] IN (@p0)) THEN 0
						ELSE NULL
					 END) AS [value]
				FROM [User] AS [t0]
				LEFT OUTER JOIN [Groups] AS [t1] ON [t0].[User_Group_id] = ([t1].[gKey])
				) AS [t2]
			WHERE (([t2].[value] = 1) AND (([t2].[User_Authenticate_id] IN (@p1)) OR ([t2].[User_Email] IN (@p2)))) OR ([t2].[User_id] IN (@p3))			 
			
			If I put the entire where in one line...
			where 	( groupsToManage.Contains( g.gName ) || ( groupsToManage.Contains( "_GLOBAL" ) && g.gKey == null ) ) && 
					( users.Contains( u.User_Authenticate_id ) || userEmails.Contains( u.User_Email ) ) || userKeys.Contains ( u.User_id )

			I get this SQL:
			SELECT [t0].[User_id] AS [uKey], [t0].[User_Authenticate_id] AS [uAuthID], [t0].[User_Email] AS [uEmail], [t0].[User_Pin] AS [uPin], [t0].[User_Active] AS [uActive], [t0].[uAdminAuthID], [t0].[uFailureCount]
			FROM [User] AS [t0]
			LEFT OUTER JOIN [Groups] AS [t1] ON [t0].[User_Group_id] = ([t1].[gKey])
			WHERE (([t1].[gName] IN (@p0)) AND (([t0].[User_Authenticate_id] IN (@p1)) OR ([t0].[User_Email] IN (@p2)))) OR ([t0].[User_id] IN (@p3))			
			
			The second 'cleaner' SQL worked with my original 'string parsing' of simply looking for [t0] and stripping everything before it
			to get rid of the SELECT and any 'TOP' clause if present.  But the first SQL introduced a layer which caused [t2] to be used.  So
			I have to do a bit different string parsing.  There is probably a more efficient way to examine the ExpressionTree and figure out
			if something like this is going to happen.  I will explore it later.
			*/
            #endregion

            var endSelect = select.IndexOf("[t");													// Get 'SELECT ' and any TOP clause if present
            var selectClause = select.Substring(0, endSelect);
            var selectTableNameStart = endSelect + 1;												// Get the table name LINQ to SQL used in query generation
            var selectTableName = select.Substring(selectTableNameStart,							// because I have to replace [t0] with it in the subSelectSB
                                        select.IndexOf("]", selectTableNameStart) - (selectTableNameStart));

            // TODO (Chris): I think instead of searching for ORDER BY in the entire select statement, I should examine the ExpressionTree and see
            // if the *outer* select (in case there are nested subselects) has an orderby clause applied to it.
            var needsTopClause = selectClause.IndexOf(" TOP ") < 0 && select.IndexOf("\r\nORDER BY ") > 0;

            var subSelect = selectClause
                                + (needsTopClause ? "TOP 100 PERCENT " : "")							// If order by in original select without TOP clause, need TOP
                                + subSelectSB.ToString()												// Append just the primary keys.
                                             .Replace("[t0]", string.Format("[{0}]", selectTableName));
            subSelect = subSelect.Substring(0, subSelect.Length - 2);									// Remove last ', '

            subSelect += select.Substring(select.IndexOf("\r\nFROM ")); // Create a sub SELECT that *only* includes the primary key fields

            var batchJoin = String.Format("FROM {0} AS j0 INNER JOIN (\r\n\r\n{1}\r\n\r\n) AS j1 ON ({2})\r\n", table.GetDbName(), subSelect, join);
            return batchJoin;
        }

        private static string GetDbName<TEntity>(this Table<TEntity> table) where TEntity : class
        {
            var entityType = typeof(TEntity);
            var metaTable = table.Context.Mapping.GetTable(entityType);
            var tableName = metaTable.TableName;

            string[] parts = tableName.Split('.');
            tableName = string.Join(".", parts.Select(p => p.StartsWith("[") ? p : string.Format("[{0}]", p)));

            return tableName;
        }


#if false
        public static class ExpressionExtensions
        {
            public static Expression Visit<T>(
                this Expression exp,
                Func<T, Expression> visitor,
                bool visitReplacement = true) where T : Expression
            {
                return ExpressionVisitor<T>.Visit(exp, visitor, visitReplacement);
            }

            public static TExp Visit<T, TExp>(
                this TExp exp,
                Func<T, Expression> visitor,
                bool visitReplacement = true)
                where T : Expression
                where TExp : Expression
            {
                return (TExp)ExpressionVisitor<T>.Visit(exp, visitor, visitReplacement);
            }

            public static Expression<TDelegate> Visit<T, TDelegate>(
                this Expression<TDelegate> exp,
                Func<T, Expression> visitor,
                bool visitReplacement = true) where T : Expression
            {
                return ExpressionVisitor<T>.Visit<TDelegate>(exp, visitor, visitReplacement);
            }

            public static IQueryable<TSource> Visit<T, TSource>(
                this IQueryable<TSource> source,
                Func<T, Expression> visitor,
                bool visitReplacement = true) where T : Expression
            {
                return source.Provider.CreateQuery<TSource>(ExpressionVisitor<T>.Visit(source.Expression, visitor, visitReplacement));
            }
        }

        /// <summary>
        /// This class visits every Parameter expression in an expression tree and calls a delegate
        /// to optionally replace the parameter.  This is useful where two expression trees need to
        /// be merged (and they don't share the same ParameterExpressions).
        /// </summary>
        public class ExpressionVisitor<T> : ExpressionVisitor where T : Expression
        {
            private Func<T, Expression> visitor;
            private bool visitReplacement;

            public ExpressionVisitor(Func<T, Expression> visitor, bool visitReplacement = true)
            {
                this.visitor = visitor;
                this.visitReplacement = visitReplacement;
            }

            public static Expression Visit(
                Expression exp,
                Func<T, Expression> visitor,
                bool visitReplacement = true)
            {
                return new ExpressionVisitor<T>(visitor, visitReplacement).Visit(exp);
            }

            public static Expression<TDelegate> Visit<TDelegate>(
                Expression<TDelegate> exp,
                Func<T, Expression> visitor,
                bool visitReplacement = true)
            {
                return (Expression<TDelegate>)new ExpressionVisitor<T>(visitor, visitReplacement).Visit(exp);
            }

            public override Expression Visit(Expression exp)
            {
                var result = (exp is T && visitor != null) ? visitor((T)exp) : exp;

                return (result != exp && !visitReplacement) ? result : base.Visit(result);
            }




        }

        
#endif

    }
}


#endif