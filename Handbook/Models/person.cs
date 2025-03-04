using Microsoft.Data.SqlClient;
namespace Program{
    class Person{
        //string types is char[255] types in database
        private string UserName;
        private string Email;
        private string Password;
        private int Zip;
        private float Range;
        public Person(string UserName, string Email, string Password, int Zip, float Range){
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
        public string GetUserName(){
            return UserName;
        }
        public string GetEmail(){
            return Email;
        }
        public string GetPassword(){
            return Password;
        }
        public int GetZip(){
            return Zip;
        }
        public float GetRange(){
            return Range;
        }

        public bool Create(){
            try{
                if (UserName == null){
                    throw new Exception("UserName CANNOT BE NULL");
                }
                Start sqlConn = new Start();
                string maxStr = sqlConn.UseConn("SELECT MAX(ID) FROM person;");
                string[] strArr = Start.SplitReader(maxStr);
                int maxInt;
                if (int.TryParse(strArr[0], out maxInt)){
                    maxInt = maxInt+1;
                    sqlConn.UseConn("INSERT INTO person (ID, UserName, Email, Password, Zip, Range) VALUES ("+maxInt+", \'"+UserName+"\', \'"+Email+"\', \'"+Password+"\', "+Zip+", "+Range+");");
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
                string str = sqlConn.UseConn("SELECT * FROM person WHERE UserName = \'"+UserName+"\' AND Email = \'"+Email+"\';");
                string[] strArr = Start.SplitReader(str);
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
                string[] strArr = Start.SplitReader(str);
                UserName = strArr[1];
                Email = strArr[2];
                Password = strArr[3];
                Zip = int.Parse(strArr[4]);
                Range = float.Parse(strArr[5]);
                return true;   
            }catch(Exception e){
                Console.WriteLine(e);
                return false;
            }
        }
        public bool Update(int ID){
            try{
                Start sqlConn = new Start();
                sqlConn.UseConn("UPDATE person SET UserName = \'"+UserName+"\', Email = \'"+Email+"\', Password = \'"+Password+"\', Zip = "+Zip+", Range = "+Range+" WHERE ID = "+ID+";");
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
                sqlConn.UseConn("DELETE FROM path WHERE PersonID = "+ID+";");
                sqlConn.UseConn("DELETE FROM person WHERE ID = "+ID+" AND UserName = \'"+UserName+"\';");
                return true;   

            }catch(Exception e){
                Console.WriteLine(e);
                return false;
            }
        }
        
    }
}