namespace Handbook.Models{
    class SavedResources{
        public static bool Create(int PersonID, int ResourceID ){
            try{
                Start sqlConn = new Start();
                sqlConn.addParam("@PersonID", System.Data.SqlDbType.Int, PersonID);
                sqlConn.addParam("@ResourceID", System.Data.SqlDbType.Int, ResourceID);
                sqlConn.UseParam("INSERT INTO savedresources (PersonID, ResourceID) VALUES (@PersonID, @ResourceID);");
                return true;   
            }catch(Exception e){
                Console.WriteLine(e);
                return false;
            }
        }
        public static List<string[]> Read(int PersonID){
            try{
                Start sqlConn = new Start();
                sqlConn.addParam("@PersonID", System.Data.SqlDbType.Int, PersonID);
                List<string[]> outList = sqlConn.UseParam("SELECT * FROM savedresources WHERE PersonID = @PersonID;");
                return outList;   
            }catch(Exception e){
                Console.WriteLine(e);
                List<string[]> empty = new List<string[]>();
                return empty;
            }
        }
        public static bool DeletePerson(int PersonID){
            try{
                Start sqlConn = new Start();
                sqlConn.addParam("@PersonID", System.Data.SqlDbType.Int, PersonID);
                sqlConn.UseParam("DELETE FROM savedresources WHERE PersonID = @PersonID;");
                return true;   
            }catch(Exception e){
                Console.WriteLine(e);
                return false;
            }
        }
        public static bool DeleteResource(int ResourceID){
            try{
                Start sqlConn = new Start();
                sqlConn.addParam("@ResourceID", System.Data.SqlDbType.Int, ResourceID);
                sqlConn.UseParam("DELETE FROM savedresources WHERE ResourceID = @ResourceID;");
                return true;   
            }catch(Exception e){
                Console.WriteLine(e);
                return false;
            }
        }
        
    }

}