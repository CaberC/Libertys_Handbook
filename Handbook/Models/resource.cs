using Microsoft.IdentityModel.Tokens;

namespace Handbook.Models{
    class Resource{
        //string types is char[255] types in database
        private string Title;
        private int Category;
        private int Zip;
        private string Provider;
        private string Details;
        public Resource(string Title, int Category, int Zip, string Provider, string Details){
            this.Title = Title;
            this.Category = Category;
            this.Provider = Provider;
            this.Zip = Zip;
            this.Details = Details;
        }
        public Resource(string strID){
            this.Title = "";
            this.Category = -1;
            this.Provider = "";
            this.Zip = -1;
            this.Details = "";
            try{
                if(int.TryParse(strID,out int ID)){
                    if (!Read(ID)){
                        throw new Exception("NO RESOURCE FOUND WITH ID");
                    }
                }else{
                    throw new Exception("NOT AN ID ENTER AN INT");
                }
            }catch(Exception e){
                Console.WriteLine(e);
            }
        }
        private void SetTitle(string Title){
            this.Title = Title;
        }
        public void SetCategory(int Category){
            this.Category = Category;
        }
        public void SetProvider(string Provider){
            this.Provider = Provider;
        }
        private void SetZip(int Zip){
            this.Zip = Zip;
        }
        public void SetDetails(string Details){
            this.Details = Details;
        }
        public string GetTitle(){
            return Title;
        }
        public int GetCategory(){
            return Category;
        }
        public string GetProvider(){
            return Provider;
        }
        public int GetZip(){
            return Zip;
        }
        public string GetDetails(){
            return Details;
        }

        public bool Create(){
            try{
                if ((Title == null) || (Provider == null) || (Details == null)){
                    throw new Exception("Required globals CANNOT BE NULL");
                }
                Start sqlConn = new Start();
                string[] strReader = sqlConn.UseConn("SELECT MAX(ID) FROM resource;")[0];
                int rows = Resource.CheckAvaialability(Title, Zip);
                string str = strReader[0];
                int maxInt;
                if (int.TryParse(str, out maxInt)){
                    maxInt = maxInt+1;
                    if(rows!=0){
                        throw new Exception("TITLES MUST BE UNIQUE IN EACH ZIP CODE");
                    }
                    sqlConn.addParam("@ID", System.Data.SqlDbType.Int, maxInt);
                    sqlConn.addParam("@Title", System.Data.SqlDbType.VarChar, Title);
                    sqlConn.addParam("@Category", System.Data.SqlDbType.VarChar, Category);
                    sqlConn.addParam("@Zip", System.Data.SqlDbType.Int, Zip);
                    sqlConn.addParam("@Provider", System.Data.SqlDbType.VarChar, Provider);
                    sqlConn.addParam("@Details", System.Data.SqlDbType.VarChar, Details);
                    var val = sqlConn.UseParam("INSERT INTO resource (ID, Title, Category, Zip, Provider, Details)"+ 
                        "VALUES (@ID, @Title, @Category, @Zip, @Provider, @Details);");
                }else{
                    throw new Exception("NOT AN INTEGER "+str);
                }
                return true;   
            }catch(Exception e){
                Console.WriteLine(e);
                return false;
            }
        }
        public int GetID(){
            try{
                Start sqlConn = new Start();
                sqlConn.addParam("@Title", System.Data.SqlDbType.VarChar, Title);
                sqlConn.addParam("@Zip", System.Data.SqlDbType.Int, Zip);
                string[] strReader = sqlConn.UseParam("SELECT * FROM resource WHERE Title = @Title AND Zip= @Zip;")[0];
                string str = strReader[0];
                int maxInt;
                if (int.TryParse(str, out maxInt)){
                    return maxInt;
                }else{
                    throw new Exception("NOT AN INTEGER "+str);
                }   
            }catch(Exception e){
                Console.WriteLine(e);
                return -1; 
            }
        }
        public bool Read(int ID){
            try{
                Start sqlConn = new Start();
                sqlConn.addParam("@ID", System.Data.SqlDbType.Int, ID);
                string[] strArr = sqlConn.UseParam("SELECT * FROM resource WHERE ID = @ID;")[0];
                Title = strArr[1];
                string str = strArr[2];
                int cat;
                if (int.TryParse(str, out cat)){
                    this.Category = cat;
                }else{
                    Category = -1;
                }
                str = strArr[3];
                int zip;
                if (int.TryParse(str, out zip)){
                    this.Zip = zip;
                }else{
                    Zip = -1;
                }
                Provider = strArr[4];
                Details = strArr[5];
                return true;   
            }catch(Exception e){
                Console.WriteLine(e);
                return false;
            }
        }
        public static List<string[]> GetResources(int page){
            var conn = new Models.Start();
            conn.addParam("@OFFSET", System.Data.SqlDbType.Int, page*5);
            List<string[]> listRes = conn.UseParam("SELECT * FROM resource ORDER BY Zip OFFSET @offset ROWS FETCH NEXT 5 ROWS ONLY;");
            return listRes;
        }

        public bool Update(int ID){
            try{
                Start sqlConn = new Start();
                if (Resource.CheckAvaialability(Title, Zip)==1){
                    sqlConn.addParam("@Category", System.Data.SqlDbType.VarChar, Category);
                    sqlConn.addParam("@Provider", System.Data.SqlDbType.VarChar, Provider);
                    sqlConn.addParam("@Details", System.Data.SqlDbType.VarChar, Details);
                    sqlConn.addParam("@ID", System.Data.SqlDbType.Int, ID);
                    sqlConn.UseParam("UPDATE resource SET Category = @Category, Provider = @Provider, Details = @Details WHERE ID = @ID;");
                    return true;
                }else{
                    return false;
                }  
            }catch(Exception e){
                Console.WriteLine(e);
                return false;
            }
        }
        public bool Delete(int ID){
            try{
                Start sqlConn = new Start();
                SavedResources.DeleteResource(ID);
                sqlConn.addParam("@ID", System.Data.SqlDbType.Int, ID);
                sqlConn.addParam("@Title", System.Data.SqlDbType.VarChar, Title);
                sqlConn.UseParam("DELETE FROM resource WHERE ID = @ID AND Title = @Title;");
                return true;   

            }catch(Exception e){
                Console.WriteLine(e);
                return false;
            }
        }
        
        private static int CheckAvaialability(string Title, int Zip){
            try{
                Start sqlConn = new Start();
                sqlConn.addParam("@Title", System.Data.SqlDbType.VarChar, Title);
                sqlConn.addParam("@Zip", System.Data.SqlDbType.Int, Zip);
                List<string[]> rows = sqlConn.UseParam("SELECT * FROM resource WHERE Title = @Title AND ZIP = @Zip;");
                if (rows == null){
                    return -1;
                }
                return rows.Count();   

            }catch(Exception e){
                Console.WriteLine(e);
                return -1;
            }
        }
        public static List<string[]> ResourceSearch(string KeyWord, int Zip, int Category){
            var conn = new Models.Start();
            List<string[]> list = new List<string[]>();
            string sql = "SELECT * FROM resource";
            if(!string.IsNullOrEmpty(KeyWord)){
                conn.addParam("@KeyWord", System.Data.SqlDbType.VarChar, "%"+KeyWord.Trim()+"%");
                sql = sql + " WHERE Title LIKE @KeyWord";
                
            }
            if(Zip>=0){
                if(!string.IsNullOrEmpty(KeyWord)){
                    sql = sql + " AND";
                }else{
                    sql = sql + " WHERE";
                }
                conn.addParam("@Zip", System.Data.SqlDbType.Int, Zip);
                sql = sql + " ZIP = @Zip";
            }
            if(Category>=0){
                if(!string.IsNullOrEmpty(KeyWord) || Zip>=0){
                    sql = sql + " AND";
                }else{
                    sql = sql + " WHERE";
                }
                conn.addParam("@Cat", System.Data.SqlDbType.Int, Category);
                sql = sql + " Category = @Cat";
            }
            try{
                list = conn.UseParam(sql+";");
            }catch (System.Exception e){
                Console.WriteLine(e.Message);
            }
            return list;
        }
    }
}