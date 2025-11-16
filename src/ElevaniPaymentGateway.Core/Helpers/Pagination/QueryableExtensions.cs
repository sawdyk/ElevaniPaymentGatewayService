using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace ElevaniPaymentGateway.Core.Helpers.Pagination
{
    public static class QueryableExtensions
    {
        public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
        this IQueryable<T> query,
        PaginationParams paginationParams,
        CancellationToken cancellationToken = default)
        {
            var totalRecords = await query.CountAsync(cancellationToken);

            //Apply sorting if specified
            //if (!string.IsNullOrWhiteSpace(paginationParams.SortBy))
            //{
            //    var property = typeof(T).GetProperty(paginationParams.SortBy,
            //        BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            //    if (property != null)
            //    {
            //        var parameter = Expression.Parameter(typeof(T), "x");
            //        var propertyAccess = Expression.Property(parameter, property);
            //        var lambda = Expression.Lambda<Func<T, object>>(
            //            Expression.Convert(propertyAccess, typeof(object)), parameter);

            //        query = paginationParams.SortDescending
            //            ? query.OrderByDescending(lambda)
            //            : query.OrderBy(lambda);
            //    }
            //}

            // Apply pagination
            var skip = (paginationParams.PageNumber - 1) * paginationParams.PageSize;
            var data = await query
                .Skip(skip)
                .Take(paginationParams.PageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<T>
            {
                PageNumber = paginationParams.PageNumber,
                PageSize = paginationParams.PageSize,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)paginationParams.PageSize),
                TotalRecords = totalRecords,
                Data = data
            };
        }

        // Fixed ApplyFilter method that handles various expression types
        public static IQueryable<T> ApplyFilter<T>(
            this IQueryable<T> query,
            string searchTerm,
            params Expression<Func<T, string>>[] propertySelectors)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || !propertySelectors.Any())
                return query;

            // Create the parameter once for all expressions
            var parameter = Expression.Parameter(typeof(T), "x");
            var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            var searchConstant = Expression.Constant(searchTerm.ToLower());

            // Create a list to hold our conditional expressions
            var conditions = new List<Expression>();

            foreach (var selector in propertySelectors)
            {
                // Safely extract the property path by analyzing the selector expression
                string propertyPath = ExtractPropertyPath(selector);
                if (string.IsNullOrEmpty(propertyPath))
                    continue;

                // Build property access expression
                Expression propertyAccess = BuildPropertyAccessExpression(parameter, propertyPath);

                // Add null check
                var nullCheck = Expression.NotEqual(propertyAccess, Expression.Constant(null));

                // Add string contains check (with ToLower())
                var toLower = Expression.Call(propertyAccess, toLowerMethod);
                var contains = Expression.Call(toLower, containsMethod, searchConstant);

                // Combine both checks (property != null && property.ToLower().Contains(term))
                var condition = Expression.AndAlso(nullCheck, contains);
                conditions.Add(condition);
            }

            if (!conditions.Any())
                return query;

            // Combine all conditions with OR
            var combinedCondition = conditions.Aggregate(Expression.OrElse);

            // Create final lambda expression
            var lambda = Expression.Lambda<Func<T, bool>>(combinedCondition, parameter);

            return query.Where(lambda);
        }

        // Helper method to extract property path from expression
        private static string ExtractPropertyPath<T>(Expression<Func<T, string>> selector)
        {
            if (selector.Body is MemberExpression memberExp)
            {
                return memberExp.Member.Name;
            }
            else if (selector.Body is UnaryExpression unaryExp && unaryExp.Operand is MemberExpression memberExpOperand)
            {
                return memberExpOperand.Member.Name;
            }
            else if (selector.Body is MethodCallExpression methodCallExp)
            {
                // Handle method call expressions - may need to be customized for your specific case
                return null;
            }

            // Add more cases as needed for other expression types
            return null;
        }

        // Helper method to build property access expression
        private static Expression BuildPropertyAccessExpression(ParameterExpression parameter, string propertyPath)
        {
            var properties = propertyPath.Split('.');
            Expression propertyAccess = parameter;

            foreach (var prop in properties)
            {
                propertyAccess = Expression.Property(propertyAccess, prop);
            }

            return propertyAccess;
        }

        // Search configuration classes
        public class SearchConfig<T>
        {
            public string SearchTerm { get; set; }
            public List<SearchProperty<T>> SearchProperties { get; set; } = new();
            //public int Page { get; set; } = 1;
            //public int PageSize { get; set; } = 10;
            public bool CaseSensitive { get; set; } = false;
        }

        public class SearchProperty<T>
        {
            public string PropertyPath { get; set; }
            public Expression<Func<T, object>> PropertyExpression { get; set; }
            public SearchType SearchType { get; set; } = SearchType.Contains;
            //public double Weight { get; set; } = 1.0; // For future relevance scoring
        }

        public static IQueryable<T> DynamicSearch<T>(this IQueryable<T> query, SearchConfig<T> searchConfig)
        {
            if (string.IsNullOrWhiteSpace(searchConfig.SearchTerm) ||
                searchConfig.SearchProperties == null ||
                !searchConfig.SearchProperties.Any())
                return query;

            var parameter = Expression.Parameter(typeof(T), "x");
            Expression searchExpression = null;

            foreach (var searchProperty in searchConfig.SearchProperties)
            {
                Expression condition = null;

                // Use PropertyExpression if provided, otherwise use PropertyPath
                if (searchProperty.PropertyExpression != null)
                {
                    condition = BuildSearchConditionFromExpression(parameter, searchProperty.PropertyExpression,
                        searchConfig.SearchTerm, searchProperty.SearchType, searchConfig.CaseSensitive);
                }
                else if (!string.IsNullOrEmpty(searchProperty.PropertyPath))
                {
                    condition = BuildSearchCondition(parameter, searchProperty.PropertyPath,
                        searchConfig.SearchTerm, searchProperty.SearchType, searchConfig.CaseSensitive);
                }

                if (condition != null)
                {
                    searchExpression = searchExpression == null ? condition : Expression.OrElse(searchExpression, condition);
                }
            }

            if (searchExpression != null)
            {
                var lambda = Expression.Lambda<Func<T, bool>>(searchExpression, parameter);
                return query.Where(lambda);
            }

            return query;
        }

        #region SEARCHING NESTED PROPERTIES
        public static IQueryable<T> DynamicSearch<T>(this IQueryable<T> query, string searchTerm, params string[] propertyPaths)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || propertyPaths == null || !propertyPaths.Any())
                return query;

            var parameter = Expression.Parameter(typeof(T), "x");
            Expression searchExpression = null;

            foreach (var propertyPath in propertyPaths)
            {
                var condition = BuildSearchCondition(parameter, propertyPath, searchTerm);
                if (condition != null)
                {
                    searchExpression = searchExpression == null ? condition : Expression.OrElse(searchExpression, condition);
                }
            }

            if (searchExpression != null)
            {
                var lambda = Expression.Lambda<Func<T, bool>>(searchExpression, parameter);
                return query.Where(lambda);
            }

            return query;
        }

        private static Expression BuildSearchCondition(ParameterExpression parameter, string propertyPath,
       string searchTerm, SearchType searchType, bool caseSensitive)
        {
            var propertyExpression = BuildPropertyExpression(parameter, propertyPath);
            if (propertyExpression == null)
                return null;

            return BuildSearchConditionCore(propertyExpression, searchTerm, searchType, caseSensitive);
        }

        private static Expression BuildSearchCondition(ParameterExpression parameter, string propertyPath, string searchTerm)
        {
            var propertyExpression = BuildPropertyExpression(parameter, propertyPath);
            if (propertyExpression == null)
                return null;

            var propertyType = propertyExpression.Type;

            // Handle enums by converting search term and comparing directly
            if (propertyType.IsEnum)
            {
                return BuildEnumSearchExpression(propertyExpression, searchTerm, propertyType);
            }

            // Handle strings
            if (propertyType == typeof(string))
            {
                return BuildStringContainsExpression(propertyExpression, searchTerm);
            }

            // Handle numeric types
            if (IsNumericType(propertyType))
            {
                return BuildNumericSearchExpression(propertyExpression, searchTerm, propertyType);
            }

            // For other types, try to convert to string using EF Functions
            return BuildEFStringSearchExpression(propertyExpression, searchTerm);
        }

        public enum SearchType
        {
            Contains,
            StartsWith,
            EndsWith,
            Equals,
            GreaterThan,
            LessThan
        }


        // Helper class for parameter replacement in expressions
        private class ParameterReplacer : ExpressionVisitor
        {
            private readonly ParameterExpression _oldParameter;
            private readonly ParameterExpression _newParameter;

            public ParameterReplacer(ParameterExpression oldParameter, ParameterExpression newParameter)
            {
                _oldParameter = oldParameter;
                _newParameter = newParameter;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                return node == _oldParameter ? _newParameter : base.VisitParameter(node);
            }
        }

        private static Expression BuildStringSearchExpression(Expression propertyExpression, string searchTerm,
        SearchType searchType, bool caseSensitive)
        {
            var nullCheck = Expression.NotEqual(propertyExpression, Expression.Constant(null));

            Expression stringExpression = propertyExpression;
            Expression searchTermExpression = Expression.Constant(searchTerm);

            // Apply case sensitivity
            if (!caseSensitive)
            {
                var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
                stringExpression = Expression.Call(propertyExpression, toLowerMethod);
                searchTermExpression = Expression.Constant(searchTerm.ToLower());
            }

            Expression comparisonExpression = searchType switch
            {
                SearchType.Contains => Expression.Call(stringExpression,
                    typeof(string).GetMethod("Contains", new[] { typeof(string) }), searchTermExpression),
                SearchType.StartsWith => Expression.Call(stringExpression,
                    typeof(string).GetMethod("StartsWith", new[] { typeof(string) }), searchTermExpression),
                SearchType.EndsWith => Expression.Call(stringExpression,
                    typeof(string).GetMethod("EndsWith", new[] { typeof(string) }), searchTermExpression),
                SearchType.Equals => Expression.Equal(stringExpression, searchTermExpression),
                _ => Expression.Call(stringExpression,
                    typeof(string).GetMethod("Contains", new[] { typeof(string) }), searchTermExpression)
            };

            return Expression.AndAlso(nullCheck, comparisonExpression);
        }
        private static Expression BuildSearchConditionCore(Expression propertyExpression, string searchTerm,
        SearchType searchType, bool caseSensitive)
        {
            var propertyType = propertyExpression.Type;

            // Handle enums by converting search term and comparing directly
            if (propertyType.IsEnum)
            {
                return BuildEnumSearchExpression(propertyExpression, searchTerm, propertyType);
            }

            // Handle strings
            if (propertyType == typeof(string))
            {
                return BuildStringSearchExpression(propertyExpression, searchTerm, searchType, caseSensitive);
            }

            // Handle numeric types
            if (IsNumericType(propertyType))
            {
                return BuildNumericSearchExpression(propertyExpression, searchTerm, propertyType, searchType);
            }

            // For other types, try to convert to string using EF Functions
            return BuildEFStringSearchExpression(propertyExpression, searchTerm);
        }

        private static Expression BuildSearchConditionFromExpression<T>(ParameterExpression parameter,
        Expression<Func<T, object>> propertyExpression, string searchTerm, SearchType searchType, bool caseSensitive)
        {
            // Replace the parameter in the expression
            var visitor = new ParameterReplacer(propertyExpression.Parameters[0], parameter);
            var body = visitor.Visit(propertyExpression.Body);

            // Handle boxing/unboxing for object expressions
            if (body is UnaryExpression unary && unary.NodeType == ExpressionType.Convert)
            {
                body = unary.Operand;
            }

            return BuildSearchConditionCore(body, searchTerm, searchType, caseSensitive);
        }

        private static Expression BuildPropertyExpression(ParameterExpression parameter, string propertyPath)
        {
            var properties = propertyPath.Split('.');
            Expression expression = parameter;

            foreach (var property in properties)
            {
                var propertyInfo = expression.Type.GetProperty(property, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (propertyInfo == null)
                    return null;

                expression = Expression.Property(expression, propertyInfo);
            }

            return expression;
        }

        private static Expression BuildEnumSearchExpression(Expression propertyExpression, string searchTerm, Type enumType)
        {
            var enumNames = Enum.GetNames(enumType);
            var matchingEnums = enumNames
                .Where(name => name.ToLower().Contains(searchTerm.ToLower()))
                .Select(name => Enum.Parse(enumType, name))
                .ToList();

            if (!matchingEnums.Any())
                return Expression.Constant(false);

            Expression condition = null;
            foreach (var enumValue in matchingEnums)
            {
                var enumConstant = Expression.Constant(enumValue, enumType);
                var equalExpression = Expression.Equal(propertyExpression, enumConstant);
                condition = condition == null ? equalExpression : Expression.OrElse(condition, equalExpression);
            }

            return condition;
        }

        private static Expression BuildStringContainsExpression(Expression propertyExpression, string searchTerm)
        {
            var nullCheck = Expression.NotEqual(propertyExpression, Expression.Constant(null));
            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);

            var lowerPropertyExpression = Expression.Call(propertyExpression, toLowerMethod);
            var lowerSearchTerm = Expression.Constant(searchTerm.ToLower());
            var containsExpression = Expression.Call(lowerPropertyExpression, containsMethod, lowerSearchTerm);

            return Expression.AndAlso(nullCheck, containsExpression);
        }

        private static Expression BuildNumericSearchExpression(Expression propertyExpression, string searchTerm, Type propertyType)
        {
            // Try to parse the search term as the numeric type
            var underlyingType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

            if (TryParseNumeric(searchTerm, underlyingType, out var numericValue))
            {
                var constant = Expression.Constant(numericValue, propertyType);
                return Expression.Equal(propertyExpression, constant);
            }

            return Expression.Constant(false);
        }

        private static Expression BuildNumericSearchExpression(Expression propertyExpression, string searchTerm,
        Type propertyType, SearchType searchType)
        {
            var underlyingType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

            if (TryParseNumeric(searchTerm, underlyingType, out var numericValue))
            {
                var constant = Expression.Constant(numericValue, propertyType);

                return searchType switch
                {
                    SearchType.Equals => Expression.Equal(propertyExpression, constant),
                    SearchType.GreaterThan => Expression.GreaterThan(propertyExpression, constant),
                    SearchType.LessThan => Expression.LessThan(propertyExpression, constant),
                    _ => Expression.Equal(propertyExpression, constant)
                };
            }

            return Expression.Constant(false);
        }

        private static Expression BuildEFStringSearchExpression(Expression propertyExpression, string searchTerm)
        {
            // Use EF.Functions.Like for database-translatable string operations
            var efFunctionsProperty = Expression.Property(null, typeof(EF), nameof(EF.Functions));
            var likeMethod = typeof(DbFunctionsExtensions).GetMethod(nameof(DbFunctionsExtensions.Like),
                new[] { typeof(DbFunctions), typeof(string), typeof(string) });

            // Convert property to string using CAST
            var castMethod = typeof(DbFunctionsExtensions).GetMethod("Cast", new[] { typeof(DbFunctions), typeof(object) });
            var stringCast = Expression.Call(castMethod.MakeGenericMethod(typeof(string)), efFunctionsProperty,
                Expression.Convert(propertyExpression, typeof(object)));

            var likePattern = Expression.Constant($"%{searchTerm}%");
            return Expression.Call(likeMethod, efFunctionsProperty, stringCast, likePattern);
        }

        private static bool IsNumericType(Type type)
        {
            var underlyingType = Nullable.GetUnderlyingType(type) ?? type;
            return underlyingType == typeof(int) || underlyingType == typeof(long) ||
                   underlyingType == typeof(decimal) || underlyingType == typeof(double) ||
                   underlyingType == typeof(float) || underlyingType == typeof(short) ||
                   underlyingType == typeof(byte) || underlyingType == typeof(uint) ||
                   underlyingType == typeof(ulong) || underlyingType == typeof(ushort);
        }

        private static bool TryParseNumeric(string value, Type type, out object result)
        {
            result = null;
            try
            {
                if (type == typeof(int)) { result = int.Parse(value); return true; }
                if (type == typeof(long)) { result = long.Parse(value); return true; }
                if (type == typeof(decimal)) { result = decimal.Parse(value); return true; }
                if (type == typeof(double)) { result = double.Parse(value); return true; }
                if (type == typeof(float)) { result = float.Parse(value); return true; }
                if (type == typeof(short)) { result = short.Parse(value); return true; }
                if (type == typeof(byte)) { result = byte.Parse(value); return true; }
                // Add more numeric types as needed
            }
            catch
            {
                // Parsing failed
            }
            return false;
        }
    }
    #endregion
}
