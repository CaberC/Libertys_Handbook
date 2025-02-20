
using Microsoft.IdentityModel.Tokens;

namespace Program{
    class Path{
        private int ID;
        private string Title;
        private Date Posted = new Date();
        private string PathStr;
        private int PersonID;
        private bool isPerson = false;

        public Path(string Title, string PathStr, int PersonID){
            this.Title = Title;
            Posted.Today();
            this.PathStr = PathStr;
            try{
                Start sqlConn = new Start();
                string maxStr = sqlConn.UseConn("SELECT * FROM person WHERE ID = "+PersonID+";");
                if (!maxStr.IsNullOrEmpty()){
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
                string maxStr = sqlConn.UseConn("SELECT * FROM person WHERE ID = \'"+PersonID+"\';");
                if (!maxStr.IsNullOrEmpty()){
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
                string maxStr = sqlConn.UseConn("SELECT MAX(ID) FROM path;");
                string[] strArr = Start.SplitReader(maxStr);
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
    }
}