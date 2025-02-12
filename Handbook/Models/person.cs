using Microsoft.Data.SqlClient;
namespace Program{
    class Person{
        //string types is char[255] types in database
        private string UserName;
        private string Email;
        private string Password;
        private int Zip;
        private float Range;
        public void Setup(string UserName, string Email, string Password, int Zip, float Range){
            this.UserName = UserName;
            this.Email = Email;
            this.Password = Password;
            this.Zip = Zip;
            this.Range = Range;
        }
        public void SetUserName(string UserName){
            this.UserName = UserName;
        }
        public void SetEmail(string Email){
            this.Email = Email;
        }
        public void SetPassword(string Password){
            this.Password = Password;
        }
        public void SetZip(int Zip){
            this.Zip = Zip;
        }
        public void SetRange(float Range){
            this.Range = Range;
        }
        public bool Create(){
            try{
                Start sqlConn = new Start();
                string maxStr = sqlConn.UseConn("SELECT MAX(ID) FROM person;");
                int maxInt;
                if (int.TryParse(maxStr, out maxInt)){
                    sqlConn.UseConn("INSERT INTO person VALUES("+(maxInt++)+", "+UserName+", "+Email+", "+Password+", "+Zip+", "+Range+" );");
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
                string str = sqlConn.UseConn("SELECT * FROM person WHERE UserName = "+UserName+" AND Email = "+Email+";");
                string[] strArr = SplitReader(str);
                str = strArr[0];
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
                string str = sqlConn.UseConn("SELECT * FROM person WHERE ID = "+ID+";");
                string[] strArr = SplitReader(str);
                //finish implimenting: read from array into class vars + gets for vars + general update from vars
                return true;   
            }catch(Exception e){
                Console.WriteLine(e);
                return false;
            }
        }
        public bool Update(int ID){
            try{
                Start sqlConn = new Start();
                sqlConn.UseConn("UPDATE person SET password = "+Password+" WHERE ID = "+ID+";");
                return true;   
            }catch(Exception e){
                Console.WriteLine(e);
                return false;
            }
        }
        public bool Delete(int ID){
            try{
                Start sqlConn = new Start();
                sqlConn.UseConn("DELETE FROM person WHERE ID = "+ID+" AND UserName = "+UserName+";");
                return true;   

            }catch(Exception e){
                Console.WriteLine(e);
                return false;
            }
        }
        private string[] SplitReader(string str){
            string[] outStr = new string[0];
            string[] sub = str.Split(',');

            foreach(string s in sub){
                int index = s.IndexOf(':')+1;
                outStr.Append(s.Substring(index));
            }

            return outStr;
        }
    }
}