namespace Bike_Backend.Function
{
    public class TSQLModel
    {
        /// <summary>
        /// 加入TSQL
        /// </summary>
        /// <param name="query">要加入交易的SQL語法</param>
        /// <param name="IsTest">如果為True，則此語法只做測試(測完Rollback)</param>
        /// <returns></returns>
        public static string GetQuery(string query, bool IsTest)
        {
            var EndTag = IsTest ?
                "ROLLBACK TRANSACTION" :
                @"IF @R <= 1 
                    COMMIT TRANSACTION
                  ELSE 
                    ROLLBACK TRANSACTION";

            return $@"BEGIN TRANSACTION 
                        DECLARE @R int
                        {query} 
                        select @R = @@ROWCOUNT
                        {EndTag}  ";
        }
    }
}
