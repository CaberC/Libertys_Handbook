namespace Handbook.Models{
    class SavedResources{
        public static bool Create(int PersonID, int ResourceID ){
            try{
                Start sqlConn = new Start();
                sqlConn.UseConn("INSERT INTO savedresources (PersonID, ResourceID) VALUES ("+PersonID+","+ResourceID+");");
                return true;   
            }catch(Exception e){
                Console.WriteLine(e);
                return false;
            }
        }
        public static List<string[]> Read(int PersonID){
            try{
                Start sqlConn = new Start();
                List<string[]> outList = sqlConn.UseConn("SELECT * FROM savedresources WHERE PersonID ="+PersonID+";");
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
                sqlConn.UseConn("DELETE FROM savedresources WHERE PersonID = "+PersonID+";");
                return true;   
            }catch(Exception e){
                Console.WriteLine(e);
                return false;
            }
        }
        public static bool DeleteResource(int ResourceID){
            try{
                Start sqlConn = new Start();
                sqlConn.UseConn("DELETE FROM savedresources WHERE ResourceID = "+ResourceID+";");
                return true;   
            }catch(Exception e){
                Console.WriteLine(e);
                return false;
            }
        }
        
    }

}