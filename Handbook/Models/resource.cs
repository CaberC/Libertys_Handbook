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
        public void SetTitle(string Title){
            this.Title = Title;
        }
        public void SetCategory(int Category){
            this.Category = Category;
        }
        public void SetProvider(string Provider){
            this.Provider = Provider;
        }
        public void SetZip(int Zip){
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
                string str = strReader[0];
                int maxInt;
                if (int.TryParse(str, out maxInt)){
                    maxInt = maxInt+1;
                    sqlConn.UseConn("INSERT INTO resource (ID, Title, Category, Zip, Provider, Details) VALUES ("+maxInt+", \'"+Title+"\', "+Category+", "+Zip+", \'"+Provider+"\', \'"+Details+"\');");
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
                string[] strReader = sqlConn.UseConn("SELECT * FROM resource WHERE Title = \'"+Title+"\' AND Zip= "+Zip+";")[0];
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
                string[] strArr = sqlConn.UseConn("SELECT * FROM resource WHERE ID = "+ID+";")[0];
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
        public static List<string[]> GetResources(int Offset, int Rows){
            var conn = new Models.Start();
            List<string[]> listRes = conn.UseConn("SELECT * FROM resource ORDER BY Zip OFFSET "+Offset+" ROWS FETCH NEXT "+Rows+" ROWS ONLY;");
            return listRes;
            
        }
        public bool Update(int ID){
            try{
                Start sqlConn = new Start();
                sqlConn.UseConn("UPDATE resource SET Title = \'"+Title+"\', Category = "+Category+", Zip = "+Zip+", Provider = \'"+Provider+"\', Details = \'"+Details+"\' WHERE ID = "+ID+";");
                return true;   
            }catch(Exception e){
                Console.WriteLine(e);
                return false;
            }
        }
        //ADD DELETE FROM ALL TABLES AS THEY'RE ADDED
        public bool Delete(int ID){
            try{
                Start sqlConn = new Start();
                SavedResources.DeleteResource(ID);
                sqlConn.UseConn("DELETE FROM resource WHERE ID = "+ID+" AND Title = \'"+Title+"\';");
                return true;   

            }catch(Exception e){
                Console.WriteLine(e);
                return false;
            }
        }
        
    }
}