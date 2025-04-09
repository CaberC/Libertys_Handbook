namespace Handbook.Models{
    class Path{
        private string Title;
        protected Date Posted = new Date();
        private string PathStr;
        private int PersonID;
        private bool isPerson = false;

        public Path(string Title, string PathStr, int PersonID){
            this.Title = Title;
            Posted.Today();
            this.PathStr = PathStr;
            try{
                Start sqlConn = new Start();
                sqlConn.addParam("@PersonID", System.Data.SqlDbType.Int, PersonID);
                List<string[]> possibleID = sqlConn.UseParam("SELECT * FROM person WHERE ID = @PersonID;");
                if (possibleID.Count!=1){
                    throw new Exception("NO PERSON FOUND WITH THAT ID");
                }else{
                    isPerson = true;
                    this.PersonID = PersonID;
                }
            }catch(Exception e){
                Console.WriteLine(e);
            }
            
        }
        public void SetTitle(string Title){
            this.Title = Title;
        }
        public void SetDate(){
            Posted.Today();
        }
        public void SetPathStr(string PathStr){
            this.PathStr = PathStr;
        }
        public void SetPersonID(int PersonID){
            try{
                Start sqlConn = new Start();
                sqlConn.addParam("@PersonID", System.Data.SqlDbType.Int, PersonID);
                List<string[]> possibleID = sqlConn.UseParam("SELECT * FROM person WHERE ID = @PersonID;");
                if (possibleID.Count!=1){
                    throw new Exception("NO PERSON FOUND WITH THAT ID");
                }else{
                    isPerson = true;
                    this.PersonID = PersonID;
                }
            }catch(Exception e){
                Console.WriteLine(e);
            }
        }
        public string GetTitle(){
            return Title;
        }
        public string GetDate(){
            return Posted.ToString();
        }
        public string GetPathStr(){
            return PathStr;
        }
        public int GetPersonID(){
            return PersonID;
        }

        public bool Create(){
            try{
                if (isPerson){
                    throw new Exception("PersonID must exsist");
                }
                Start sqlConn = new Start();
                string[] strPath = sqlConn.UseConn("SELECT MAX(ID) FROM path;")[0];
                int maxInt;
                if (int.TryParse(strPath[0], out maxInt)){
                    maxInt = maxInt+1;
                    sqlConn.addParam("@Title", System.Data.SqlDbType.VarChar, Title);
                    sqlConn.addParam("@PathStr", System.Data.SqlDbType.VarChar, PathStr);
                    sqlConn.addParam("@PersonID", System.Data.SqlDbType.Int, PersonID);
                    sqlConn.UseParam("INSERT INTO path (ID, Title, Posted, PathStr, PersonID) VALUES ("+maxInt+", @Title, \'"+Posted.ToString()+"\', @PathStr, @PersonID);");
                }else{
                    throw new Exception("NOT AN INTEGER "+strPath[0]);
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
                sqlConn.addParam("@PersonID", System.Data.SqlDbType.Int, PersonID);
                string[] strPath = sqlConn.UseParam("SELECT * FROM path WHERE Title = @Title AND PersonID = @PersonID;")[0];
                string str = strPath[0];
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
                string[] strArr = sqlConn.UseParam("SELECT * FROM path WHERE ID = @ID;")[0];
                Title = strArr[1];
                Posted = Posted.ToDate(strArr[2]);
                PathStr = strArr[3];
                PersonID = int.Parse(strArr[4]);
                return true;
            }catch(Exception e){
                Console.WriteLine(e);
                return false;
            }
        }
        public bool Update(int ID){
            try{
                Start sqlConn = new Start();
                sqlConn.addParam("@Title", System.Data.SqlDbType.VarChar, Title);
                sqlConn.addParam("@PathStr", System.Data.SqlDbType.VarChar, PathStr);
                sqlConn.addParam("@PersonID", System.Data.SqlDbType.Int, PersonID);
                sqlConn.addParam("@ID", System.Data.SqlDbType.Int, ID);
                sqlConn.UseParam("UPDATE path SET Title = @Title, Posted = \'"+Posted+"\', PathStr = @PathStr, PersonID = @PersonID WHERE ID = @ID;");
                return true;   
            }catch(Exception e){
                Console.WriteLine(e);
                return false;
            }
        }
        public bool Delete(int ID){
            try{
                Start sqlConn = new Start();
                sqlConn.addParam("@ID", System.Data.SqlDbType.Int, ID);
                sqlConn.addParam("@Title", System.Data.SqlDbType.VarChar, Title);
                sqlConn.UseParam("DELETE FROM path WHERE ID = @ID AND Title = @Title;");
                return true;   

            }catch(Exception e){
                Console.WriteLine(e);
                return false;
            }
        }
        public static bool DeletePerson(int PersonID){
            try{
                Start sqlConn = new Start();
                sqlConn.addParam("@PersonID", System.Data.SqlDbType.Int, PersonID);
                sqlConn.UseParam("DELETE FROM path WHERE PersonID = @PersonID;");
                return true;   

            }catch(Exception e){
                Console.WriteLine(e);
                return false;
            }
        }
    }
}