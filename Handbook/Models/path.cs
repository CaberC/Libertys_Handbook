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
                string[] possibleID = sqlConn.UseConn("SELECT * FROM person WHERE ID = "+PersonID+";");
                if (string.IsNullOrEmpty(possibleID[0])){
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
                string[] possibleID = sqlConn.UseConn("SELECT * FROM person WHERE ID = \'"+PersonID+"\';");
                if (string.IsNullOrEmpty(possibleID[0])){
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
                string[] maxStr = sqlConn.UseConn("SELECT MAX(ID) FROM path;");
                string[] strArr = Start.SplitReader(maxStr[0]);
                int maxInt;
                if (int.TryParse(strArr[0], out maxInt)){
                    maxInt = maxInt+1;
                    sqlConn.UseConn("INSERT INTO path (ID, Title, Posted, PathStr, PersonID) VALUES ("+maxInt+", \'"+Title+"\', \'"+Posted.ToString()+"\', \'"+PathStr+"\', "+PersonID+");");
                }else{
                    throw new Exception("NOT AN INTEGER "+maxStr);
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
                string[] strPath = sqlConn.UseConn("SELECT * FROM path WHERE Title = \'"+Title+"\' AND PersonID = \'"+PersonID+"\';");
                string[] strArr = Start.SplitReader(strPath[0]);
                string str = strArr[0];
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
                string[] str = sqlConn.UseConn("SELECT * FROM path WHERE ID = "+ID+";");
                string[] strArr = Start.SplitReader(str[0]);
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
                sqlConn.UseConn("UPDATE path SET Title = \'"+Title+"\', Posted = \'"+Posted+"\', PathStr = \'"+PathStr+"\', PersonID = "+PersonID+" WHERE ID = "+ID+";");
                return true;   
            }catch(Exception e){
                Console.WriteLine(e);
                return false;
            }
        }
        public bool Delete(int ID){
            try{
                Start sqlConn = new Start();
                sqlConn.UseConn("DELETE FROM path WHERE ID = "+ID+" AND Title = \'"+Title+"\';");
                return true;   

            }catch(Exception e){
                Console.WriteLine(e);
                return false;
            }
        }
    }
}